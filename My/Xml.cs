using System;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Xml.Serialization;

namespace MyLib
{
	public class Xml
	{
		public static void SaveFile(object cls, string filename)
		{
			My.WriteToFile(filename, Serialize(cls));
		}
		public static string Serialize(object cls)
		{
			XmlSerializer xs = new XmlSerializer(cls.GetType());
			using (MemoryStream ms = new MemoryStream())
			{
				xs.Serialize(ms, cls);
				return new UTF8Encoding().GetString(ms.ToArray());
			}
		}
		public static string SerializeJson(object obj)
		{
			DataContractJsonSerializer js = new DataContractJsonSerializer(obj.GetType());
			using (MemoryStream ms = new MemoryStream())
			{
				js.WriteObject(ms, obj);
				return new UTF8Encoding().GetString(ms.ToArray());
			}
		}
		public static T LoadFile<T>(string filename)
		{
			return Deserialize<T>(My.ReadFromFile(filename));
		}
		//public static T DeserializeJson<T>(string JsonString)
		//{
		//	object obj = null;
		//	//XmlSerializer xs = new XmlSerializer(typeof(T));
		//	DataContractJsonSerializer js = new DataContractJsonSerializer(typeof(T));
		//	if (JsonString.Length > 0)
		//		using (StringReader sr = new StringReader(JsonString))
		//		{
		//			obj = js.ReadObject(sr);
		//		}
		//	return (T)obj;
		//}
		public static T Deserialize<T>(string xmlString)
		{
			object obj = null;
			XmlSerializer xs = new XmlSerializer(typeof(T));
			if (xmlString.Length > 0)
				using (StringReader sr = new StringReader(xmlString))
				{
					obj = xs.Deserialize(sr);
				}
			return (T)obj;
		}
		public static void SaveBinary(object cls, string filename)
		{
			using (FileStream fs = new FileStream(filename, FileMode.Create))
			{
				BinaryFormatter formatter = new BinaryFormatter();
				try
				{
					formatter.Serialize(fs, cls);
				}
				catch (SerializationException e)
				{
					Console.WriteLine("Failed to serialize. Reason: " + e.Message);
					throw;
				}
			}
		}
		public static object LoadBinary(string filename)
		{
			object cls = null;
			if (File.Exists(filename))
				using (FileStream fs = new FileStream(filename, FileMode.Open))
				{
					BinaryFormatter formatter = new BinaryFormatter();
					try
					{
						cls = formatter.Deserialize(fs);
					}
					catch (SerializationException e)
					{
						Console.WriteLine("Failed to deserialize. Reason: " + e.Message);
						throw;
					}
				}
			return cls;
		}
	}
}
