using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Win32;
using System.Runtime.InteropServices;

namespace MyLib
{
	public class W32
	{
		//[StructLayout(LayoutKind.Sequential)]
		//internal struct SHFILEINFO
		//{
		//    public IntPtr hIcon;
		//    public IntPtr iIcon;
		//    public uint dwAttributes;
		//    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 260)]
		//    public string szDisplayName;
		//    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 80)]
		//    public string szTypeName;
		//}
		//[DllImport("shell32.dll")]
		//internal static extern IntPtr SHGetFileInfo(string pszPath, uint dwFileAttributes, ref SHFILEINFO psfi, uint cbSizeFileInfo, uint uFlags);

		[DllImport("Kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		internal static extern bool GetVolumeInformation(
		  string RootPathName,
		  StringBuilder VolumeNameBuffer,
		  int VolumeNameSize,
		  out uint VolumeSerialNumber,
		  out uint MaximumComponentLength,
		  out uint FileSystemFlags,
		  StringBuilder FileSystemNameBuffer,
		  int nFileSystemNameSize);

		[DllImport("user32.dll")]
		public static extern Int32 SwapMouseButton(Int32 bSwap);
	}
}

