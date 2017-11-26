﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using MyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTests
{
	[TestClass()]
	public class ExtensionsTests
	{
		[TestMethod()]
		public void IsEqualTest()
		{
			object obj0=null;
			var obj1 = new Person() { name = "rudy", adres = "Profeinthovenstraat" };
			var obj2 = new Person() { name = "rudy", adres = "Profeinthovenstraat" };
			var obj3 = new Person() { name = "rudy", adres = "Profeinth0venstraat" };
			Assert.IsTrue(obj0.IsEqual(obj0),$"obj3 not equal.");
			Assert.IsTrue(obj1.IsEqual(obj2),$"object are not equal.");
			Assert.IsTrue(!obj1.IsEqual(obj3 ),$"object are equal.");
		}
	}
}