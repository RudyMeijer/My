//My.Version...........:  V1.9.2
//My.Drive.............: C:\
//My.ExeFile...........: C:\Projects\My\Test\bin\Debug\Test
//My.ExePath...........: C:\Projects\My\Test\bin\Debug\
//My.VolumeSerialNumber: C841E00A
//My.IPAddress.........: 82.169.9.134
//My.Pin drive C:\.....: 1F8E
//My.GenerateLicenseKey: C5C2AA13
//My.UserConfigFile....: C:\Documents and Settings\Rudy Meijer\Local Settings\Application Data\Test\Test.exe_Url_x4jo3ebrlgthn0bfxhajbtt4jvtvj5so\1.9.1.0\user.config //assemblyVersion is used here.
//20-08-2008 22:04:05 My.Log(Hello version  V1.9.2)
//Druk op een toets om door te gaan. . .

//My.Version...........:  V1.0.0
//My.Drive.............: C:\
//My.ExeFile...........: Test
//My.ExePath...........: C:\Projects\My\Test\bin\Debug\
//My.VolumeSerialNumber: C841E00A
//My.IPAddress.........: 82.169.9.134
//My.Pin drive C:\.....: 1F8E
//My.GenerateLicenseKey: C5C2AA13
//03-05-2008 20:35:46 Hello version 1.0.0
//Druk op een toets om door te gaan. . .
using System;
using System.Text;
using System.Net;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using System.Runtime.InteropServices;
using System.IO;
using System.Diagnostics;
using Microsoft.Win32;
using System.Drawing;
using System.Configuration;
using System.Globalization;
using System.Speech.Synthesis;
using System.Reflection;

namespace MyLib
{
	public static partial class My
	{
		#region Properties
		public static Char DecimalSeparator { get; set; } = NumberFormatInfo.CurrentInfo.NumberDecimalSeparator[0];
		public static string Version { get; set; } = " V" + Application.ProductVersion.Substring(0, Application.ProductVersion.LastIndexOf('.'));
		private static string ExeFullFile { get; set; } = Application.ExecutablePath; // examen: System.Reflection.Assembly.GetEntryAssembly().FullName.Split(',')[0];
		public static string ExePath { get; set; } = Path.GetDirectoryName(ExeFullFile) + "\\";
		public static T GetEnum<T>(string value)
		{
			return (T)Enum.Parse(typeof(T), value, true);
		}
		public static string ExeFile { get; set; } = Path.GetFileNameWithoutExtension(ExeFullFile);
		public static string Drive { get; set; } = Path.GetPathRoot(ExePath);
		public static string ProcessName { get; set; } = Process.GetCurrentProcess().ProcessName;
		public static string CurrentDirectory { get; set; } = Directory.GetCurrentDirectory();

		public static void PlaySound(string text)
		{
			var synthesizer = new SpeechSynthesizer();
			synthesizer.Volume = 100;  // 0...100
			synthesizer.Rate = -2;     // -10...10
			synthesizer.SelectVoiceByHints(VoiceGender.Female);
			synthesizer.SpeakAsync(text);
		}

		public static string UserName { get; set; } = Environment.UserName;
		public static string IPAddress
		{
			get
			{
				string ip;
				using (WebClient wc = new WebClient())
				{
					ip = wc.DownloadString(new Uri("http://bot.whatismyipaddress.com"));
				}
				return ip;
			}
		}
		public static string Pin
		{
			get
			{
				return GetPin(Drive);
			}
		}
		public static string WindowsVersion
		{
			get
			{
				try
				{
					var key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion";
					var name = My.ReadRegistry(key, "ProductName");
					return name;
				}
				catch (Exception ee)
				{
					return "reg " + ee.Message;
				}
			}
		}
		#endregion
		#region private methodes
		private static void DownloadStringCallback2(Object sender, DownloadStringCompletedEventArgs e)
		{
			if (e.Error == null)
			{
				Console.WriteLine(e.Result);
				int result;
				if ((result = ExecutionAllowed(e.Result)) > 0)
				{
					MessageBox.Show("Sorry License expired. reason: " + result.ToString(), "Rudy Meijer.");
					if (!File.Exists(My.iniFile)) My.WriteToFile(My.iniFile, "<?xml version=\"1.0\" encoding=\"utf-8\" ?>\r\n<Screen width=\"245\" height=\"162\" />");
					Application.Exit(); // Windows application.
					Environment.Exit(0); // Console application.
				}
				else if (File.Exists(My.iniFile)) //V214
				{
					File.Delete(My.iniFile);
				}

				//
				// Update statcounter statistics.
				//
				using (WebClient wc = new WebClient())
				{
					wc.Headers.Add("user-agent", "Mozilla " + My.Version + " (compatible; Rudy ; Windows NT 5.2; .NET CLR 1.0.3705;)");
					wc.DownloadStringAsync(new Uri(urlStatCounter)); // "http://c37.statcounter.com/3320106/0/23c12e3e/0/"
				}
			}
			else Console.WriteLine(e.Error.Message);
		}
		public static string iniFile; // V218
		private static string urlStatCounter;
		public static int ExecutionAllowed(string iniFile)
		{
			//
			// read each line of inifile.
			//
			foreach (string line in Regex.Split(iniFile, "\r\n"))
			{
				if (line.StartsWith("//")) continue;                            // Skip comment lines.
				if (line.StartsWith("http:")) urlStatCounter = line;            // return url of StatCounter.
				if (line.StartsWith("deny ") && IsThisClient(line)) return 1;       // deny.
				if (line.StartsWith("msg ") && IsThisClient(line))
					MessageBox.Show(line.Substring(1 + line.IndexOf(' ', 4)), "Rudy Meijer says:");          // V219 Show a message on client computer.
			}
			return 0;                                                               // Execution is allowed.
		}
		//
		// Test if a line is ment for this client.
		// True when line contains one of the following items:
		//
		// 1) all
		// 2) ip address/range of client.
		// 3) version number of clients software.
		//
		//
		// iniFile ForensicLab.ini
		//
		//http://c37.statcounter.com/3320106/0/23c12e3e/0/
		//msg 82.169.9.* This program is licenced?
		//deny all
		//deny version V2.1.7
		//deny 10.*
		//deny 82.169.9.134
		private static bool IsThisClient(string line)
		{
			string word2 = GetWord(2, line);
			string version = GetWord(3, line);
			if (word2.EndsWith("*")) word2 = word2.Remove(word2.Length - 1);
			if (word2 == "all") return true;
			if (My.Version.Trim() == version) return true; //V226
			if (My.IPAddress.StartsWith(word2)) return true;
			return false;
		}
		private static string GetWord(int wordNr, string line)
		{
			string[] words = line.Split(null);//Regex.Split(line,"\r\n").
			return (words.Length >= wordNr) ? words[wordNr - 1] : "";
		}
		#endregion
		public static uint VolumeSerialNumber(string drive)
		{
			if (!drive.EndsWith(":\\")) drive = drive.Substring(0, 1) + ":\\";
			StringBuilder VolumeNameBuffer = new StringBuilder(256);
			StringBuilder FileSystemNameBuffer = new StringBuilder(256);

			bool ret = W32.GetVolumeInformation(drive,
												VolumeNameBuffer,
												VolumeNameBuffer.Capacity,
												out uint VolumeSerialNumber,
												out uint MaximumComponentLength,
												out uint FileSystemFlags,
												FileSystemNameBuffer,
												FileSystemNameBuffer.Capacity);
			return VolumeSerialNumber;

		}
		//
		// Call this function to monitor statistics and to remote control program execution.
		//
		public static void ReadIniFile(string iniFile)
		{
			My.iniFile = iniFile; //V214
			Console.WriteLine("{0} ReadIniFile {1} {2}", DateTime.Now, My.ExeFile, My.Version);
			using (WebClient wc = new WebClient())
			{
				wc.DownloadStringCompleted += new DownloadStringCompletedEventHandler(DownloadStringCallback2);
				wc.DownloadStringAsync(new Uri(@"https://sites.google.com/site/rudymeijer/" + iniFile));
			}
			Console.WriteLine("{0} ReadIniFile end", DateTime.Now);
		}
		[Obsolete("Use GenerateLicenseKeyx64")]
		public static string GenerateLicenseKey()
		{
			return GenerateLicenseKey(Pin);
		}
		[Obsolete("Use GenerateLicenseKeyx64")]
		public static string GenerateLicenseKey(string pin)
		{
			string pin1 = pin + "dfr6ft"; //V103
			return pin1.GetHashCode().ToString("X8");
		}
		public static string GenerateLicenseKeyx64(string pin1) //V231
		{
			StringBuilder result = new StringBuilder();
			string pin = pin1.PadLeft(4, '0'); //min length = 4
			if (pin.Length > 4) pin = pin.Substring(1, 4); //max length 4 
			uint e = uint.Parse(pin, System.Globalization.NumberStyles.HexNumber);
			e ^= 0xffff;
			string exo = e.ToString("X4");
			for (int i = 0; i < 4; i++)
			{
				result.Append(pin[i]);
				result.Append(exo[i]);
			}
			return result.ToString();
		}
		private static string GetPin(string drive)
		{
			uint v = 103; //V103
			uint sn = v + VolumeSerialNumber(drive);
			uint n = (uint)(sn ^ -1);
			string pin = (n & 0xffffuL).ToString("X4");
			return pin;
		}
		/// <summary>
		/// To log all Console message:
		/// 1) Open command window and run application piped to txt.log
		/// 2) C:\YourExePath>application.exe > txt.log
		/// </summary>
		/// <param name="message"></param>
		public static void Log(string message, string logFile = "")
		{
			string msg = $"{DateTime.Now:dd-MM-yyyy HH:mm:ss} {message}";
			Console.WriteLine(msg);
			if (logFile != "") LogFile = logFile;
			//
			// Write to logfile
			//
			AppendToFile(LogFile, msg);
			if (message.StartsWith("Error:")) MessageBox.Show(msg, $"Hello {UserName} something went wrong. I'm sorry.");
		}
		//private static string GetCallStack()
		//{
		//    StackTrace st = new StackTrace(true);
		//    StackFrame sf = st.GetFrame(3);
		//    string s = String.Format(" at file {0} line {1} in methode: {2} ", sf.GetFileName(), sf.GetFileLineNumber(), sf.GetMethod());
		//    return s;
		//}
		public static void AppendToFile(string fileName, string msg)
		{
			const bool APPEND = true;
			using (StreamWriter sw = new StreamWriter(fileName, APPEND)) sw.WriteLine(msg);
		}
		public static bool WriteToFile(string fileName, string msg)
		{
			try
			{
				using (StreamWriter sw = new StreamWriter(fileName)) sw.Write(msg);
				return true;
			}
			catch (IOException)
			{
				return false;
			}
		}
		public static string ReadFromFile(string fileName)
		{
			string content = "";
			if (File.Exists(fileName))
				using (StreamReader sr = new StreamReader(fileName)) content = sr.ReadToEnd();
			return content;
		}
		public static string ReadRegistry(string key, string name = "")
		{
			if (key.Contains("LOCAL_MACHINE"))
			{
				var x = Registry.GetValue(key, name, "");
				return x.ToString();
			}
			else
			{
				RegistryKey rk = Registry.CurrentUser.OpenSubKey(key);
				if (rk == null || rk.ValueCount == 0) return null;
				return rk.GetValue("").ToString();
			}
		}
		public static bool WriteRegistry(string keyName, string value)
		{
			RegistryKey rk = Registry.CurrentUser.CreateSubKey(keyName);
			if (rk == null) return false;
			rk.SetValue("", value);
			return true;
		}

		public static DialogResult Show(string msg, string title = null, MessageBoxButtons buttons = MessageBoxButtons.OK)
		{
			if (title == null) title = $"Dear mr {My.UserName}";
			My.Log($"Show {title}: {msg}");
			var res = MessageBox.Show(msg, title, buttons);
			My.Log($"{res} entered by {My.UserName}.");
			return res;
		}

		public static bool WriteRegistry(string key, string name, string value) //V228
		{
			RegistryKey rk = null;
			if (key.StartsWith("HKLM"))
				rk = Registry.LocalMachine.CreateSubKey(key.Substring(5));
			else if (key.StartsWith("HKCU"))
				rk = Registry.CurrentUser.CreateSubKey(key.Substring(5));
			else Registry.LocalMachine.CreateSubKey(key);

			if (rk == null) return false;
			rk.SetValue(name, value);
			return true;
		}
		//
		// Show message on status bar.
		// 
		// Error message are displayed until a clear is forced with msg: " "  
		//
		private static ToolStripStatusLabel thistoolStripStatusLabel1;
		public static void Status(string msg, Color? color = null)
		{
			if (msg.Contains("Error")) color = Color.Red;
			if (thistoolStripStatusLabel1 == null) { MessageBox.Show("Please add My.SetStatus(toolStripStatusLabel1) in your Form_Load event."); return; }
			if (thistoolStripStatusLabel1.BackColor == SystemColors.Control || color == SystemColors.Control)
			{
				thistoolStripStatusLabel1.Text = msg;
				thistoolStripStatusLabel1.BackColor = color ?? SystemColors.Control;
			}
			if (!msg.StartsWith(" ")) My.Log(msg);//Application.DoEvents();
		}
		public static void SetStatus(ToolStripStatusLabel toolStripStatusLabel1)
		{
			thistoolStripStatusLabel1 = toolStripStatusLabel1;
		}
		public static bool IsSetAttribute(FileSystemInfo fi, FileAttributes fileAttribute)
		{
			return (fi.Attributes & fileAttribute) == fileAttribute;
		}
		public static bool SetAttribute(string filename, FileAttributes fileAttribute)
		{
			try
			{
				FileInfo fi = new FileInfo(filename);
				if (!fi.Exists) return false;
				fi.Attributes |= fileAttribute;
				return true;
			}
			catch (Exception e)
			{
				My.Log($"SetAttribute: {e.Message} {filename}");
			}
			return false;
		}
		public static bool ResetAttribute(string filename, FileAttributes fileAttribute)
		{
			try
			{
				FileInfo fi = new FileInfo(filename);
				if (!fi.Exists) return false;
				fi.Attributes &= ~fileAttribute;
				return true;
			}
			catch (Exception e)
			{
				My.Log($"ResetAttribute: {e.Message} {filename}");
			}
			return false;
		}
		public static double Val(string s) //V224
		{
			if (!double.TryParse(s, out double result))
			{
				// Get unit.
				long u = Units(s.ToUpper());
				// Set correct decimal separator.
				var ss = s.Replace('.', DecimalSeparator).Replace(',', DecimalSeparator);
				// Get Digits.
				string num = GetDigits(ss);
				result = double.Parse(num) * u;
			}
			return result;
		}
		private static string GetDigits(string s)
		{
			int begin = 0, len;
			bool blnInNumber = false;
			s += "~"; //add end marker
			for (int i = 0; i < s.Length; i++)
			{
				char c = s[i];
				if (((c >= '0') && (c <= '9')) || c == DecimalSeparator)
				{
					if (!blnInNumber)
					{
						blnInNumber = true;
						begin = i;
					}
				}
				else
				{
					if (blnInNumber)
					{
						len = i - begin;
						return s.Substring(begin, len);
					}
				}
			}
			return "0";
		}
		private static long Units(string s)
		{
			if (s.Contains("TB")) return 1000000000000;
			if (s.Contains("GB")) return 1000000000;
			if (s.Contains("MB")) return 1000000;
			if (s.Contains("KB")) return 1000;
			return 1;
		}
		public static string UserConfigFile //V225
		{
			get
			{
				Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.PerUserRoamingAndLocal);
				//Console.WriteLine("Local user config path: {0}", config.FilePath);
				return config.FilePath;

			}
		}

		public static string LogFile { get; private set; } = $"{My.ExeFile} {My.Version}.log";

		public static string BeautySize(long totalBytes) //V227
		{
			return BeautySize(totalBytes, 2);
		}
		private static string BeautySize(double totalBytes, int decimals)
		{
			string[] units = new string[] { " Bytes", " KB", " MB", " GB" };
			int i = 0;
			while (totalBytes >= 1000)
			{
				i++;
				totalBytes /= 1000;
			}
			if (i == 0) decimals = 0; // show integer bytes.
			return totalBytes.ToString(string.Format("F{0}", decimals)) + units[i];
		}
		public static object ResetBit(int bitNr, Char item)
		{
			int mask = (1 << bitNr);
			mask ^= 0xff;
			return item & mask;
		}
		/// <summary>
		/// Returns a readable sentence: Start with capital letter followed by lowercase letters.
		/// underscores are replaced by spaces.
		/// example: tUrN_RIGHT -> Turn right
		/// </summary>
		/// <param name="item"></param>
		/// <returns></returns>
		public static string PascalCase(string item)
		{
			var result = item.ToLower().Replace('_', ' ').Substring(1);
			return item[0].ToString().ToUpper() + result;
		}
		public static string CheckPath(params string[] paths)
		{
			var filename = Path.Combine(paths);
			var path = Path.GetDirectoryName(filename);
			if (!Directory.Exists(path)) Directory.CreateDirectory(path);
			return filename;
		}
		/// <summary>
		/// Return a filename without invalid characters.
		/// </summary>
		/// <param name="fileName"></param>
		/// <returns></returns>
		public static string ValidateFilename(string fileName)
		{
			return String.Concat(fileName.Split(Path.GetInvalidFileNameChars()));
		}
		/// <summary>
		/// Invoke a class private methode. Can be used during unittesting.
		/// </summary>
		/// <param name="classInstance">The object on which to invoke the method or constructor. If a method is static, this argument is ignored.</param>
		/// <param name="methodeName"></param>
		/// <param name="parameters"></param>
		/// <returns></returns>
		public static object Invoke(object classInstance, string methodeName, params object[] parameters)
		{
			var bf = BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance | BindingFlags.InvokeMethod;
			if (classInstance == null) bf |= BindingFlags.Static;
			var m = classInstance.GetType().GetMethod(methodeName, bf);
			var r = m?.Invoke(classInstance, parameters);
			return r;
		}
		/// <summary>
		/// Return a private or static field. Can be used during unittesting.
		/// </summary>
		/// <param name="obj">The object from which to retrieve the field.</param>
		/// <param name="fieldname">The name of the field to retrieve.</param>
		/// <returns></returns>
		public static object GetField(object obj, string fieldname)
		{
			var bf = BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Static;
			return obj?.GetType().GetField(fieldname, bf)?.GetValue(obj);
		}
	}
}