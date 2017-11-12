﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTests
{
    [TestClass]
    public class TestMy
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
    }
}