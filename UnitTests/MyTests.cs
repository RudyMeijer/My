﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using MyLib;
using System.Diagnostics;
using System.IO;
namespace UnitTests
{
	[TestClass]
	public class MyTests
	{
		[TestMethod]
		//
		// Run this test in debug mode to see results in output window.
		//
		public void Property()
		{
			Debug.WriteLine($"My.Version = {My.Version}");
			Debug.WriteLine($"My.Pin = {My.Pin}");
			Debug.WriteLine($"My.Drive = {My.Drive}");
			Debug.WriteLine($"My.IPAddress = {My.IPAddress}");
			Debug.WriteLine($"My.ProcessName = {My.ProcessName}");
			Debug.WriteLine($"My.VolumeSerialNumber = {My.VolumeSerialNumber("c:")}");
			Debug.WriteLine($"My.DecimalSeparator = {My.DecimalSeparator}");

			Debug.WriteLine($"My.ExeFile = {My.ExeFile}");
			Debug.WriteLine($"My.ExePath = {My.ExePath}");
			Debug.WriteLine($"My.UserConfigFile = {My.UserConfigFile}");
			Debug.WriteLine($"My.UserName = {My.UserName}");
			Debug.WriteLine($"My.WindowsVersion = {My.WindowsVersion}");
			Assert.IsTrue(My.ExeFile == "testhost.x86", "Error exefile");
		}
		[TestMethod]
		public void Val()
		{
			Debug.WriteLine($"My.Val(12.34 KB) = {My.Val("12.34 kb")} bytes.");
			Assert.IsTrue(My.Val("12,34 Kb") == 12340, $"My.Val 12,34 Kb = {My.Val("12,34 Kb")}");
			Assert.IsTrue(My.Val("12.34 Mb") == 12340000, $"My.Val 12.34 Mb = {My.Val("12.34 Mb")}");
		}
		[TestMethod]
		public void Registry()
		{
			My.WriteRegistry("rudy", "123");
			var r = My.ReadRegistry("rudy");
			Assert.IsTrue(r == "123", $"Registry 123 = {r}");
		}
		[TestMethod]
		public void ReadIniFile()
		{
			My.ReadIniFile("ForensicLab.ini");
		}
		[TestMethod]
		public void ReadWriteFile()
		{
			My.WriteToFile("test.txt", "Hello world");
			var msg = My.ReadFromFile("test.txt");
			Debug.WriteLine($"ReadWriteFile: { msg}");
			Assert.IsTrue(msg == "Hello world", $"msg = {msg}");
		}
		[TestMethod]
		public void GenerateKey()
		{
			var license = My.GenerateLicenseKeyx64(My.Pin);
			Assert.IsTrue(license == "A5E12D0F", $"Invalid licenseKey {license }"); // Laptop Marga
		}
		[TestMethod]
		public void MethodeTests()
		{
			var r = My.BeautySize(1235000);
			Assert.IsTrue(r == "1,24 MB", $"BeautySize = " + r);

			Assert.IsTrue(My.SetAttribute("test.txt", FileAttributes.ReadOnly), $"MySetAttribute is false");
			Assert.IsTrue(My.IsSetAttribute(new FileInfo("test.txt"), FileAttributes.Archive), $"MyIsSetAttribute is false");
			Assert.IsTrue(My.ResetAttribute("test.txt", FileAttributes.ReadOnly), $"MyResetAttribute is false");
		}

		[TestMethod()]
		public void ValidateFilenameTest()
		{
			var invalid = @"//filename ""with \\valid characters?";
			var valid = @"filename_with_valid_characters";
			var res = My.ValidateFilename(invalid);
			Assert.IsTrue(res == valid, "filename contains invalid characters.");
		}

		[TestMethod()]
		public void LerpTest()
		{
			var V0 = 10f;
			var V1 = 20f;
			var r = My.Lerp(0, V0, V1);
			Assert.IsTrue(r == V0, $"InverseLerp should be {V0} {r}");
			r = My.Lerp(0.5f, V0, V1);
			Assert.IsTrue(r == 15, $"InverseLerp should be 15f {r}");
			r = My.Lerp(1f, V0, V1);
			Assert.IsTrue(r == V1, $"InverseLerp should be {V1} {r}");


			r = My.InverseLerp(10f, V0, V1);
			Assert.IsTrue(r == 0, $"InverseLerp should be 0 {r}");
			r = My.InverseLerp(15f, V0, V1);
			Assert.IsTrue(r == 0.5, $"InverseLerp should be 0.5 {r}");
			r = My.InverseLerp(20f, V0, V1);
			Assert.IsTrue(r == 1, $"InverseLerp should be 1 {r}");
		}

		[TestMethod]
        public void VersionTest()
        {
            var expected = " V1.2.3";
            var v = My.Version;
            Assert.IsTrue(v == expected, $"Invalid version {v} should be {expected}");
        }

        [TestMethod]
        public void EnumTest()
        {
            var expected = 1;
            var v = (int)My.GetEnum<MyEnum>("Yes");
            Assert.IsTrue(v == 1, $"Invalid enum {v} should be {expected}");

            v = (int)My.GetEnum<MyEnum>("xxx");
            Assert.IsTrue(v == 0, $"Invalid enum {v} should be {expected}");

            MyEnum name = My.GetEnumByIndex<MyEnum>(1);
            Assert.IsTrue(name.ToString() == "Yes", $"Invalid enum {name} should be Yes.");
        }
        enum MyEnum
        {
            No,
            Yes
        }
    }
}
