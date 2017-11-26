using Microsoft.VisualStudio.TestTools.UnitTesting;
using MyLib;
using System.Collections.Generic;

namespace UnitTests
{
	[TestClass()]
	public class XmlTests
	{
		[TestMethod()]
		public void SaveFileTest()
		{
			var p = new Person() { name = "rudy", adres = "Profeinthovenstraat" };
			Xml.SaveFile(p, "person.xml");
			var x = Xml.LoadFile<Person>("person.xml");
			Assert.IsTrue(x.name == "rudy");
		}

		//[TestMethod()]
		//public void SerializeJsonTest()
		//{
		//	string expect = "{\"adres\":\"Profeinthovenstraat\",\"name\":\"rudy\"}";
		//	var p = new Person() { name = "rudy", adres = "Profeinthovenstraat" };
		//	var x = Xml.SerializeJson(p);
		//	Assert.IsTrue(x == expect, $"Error in json");
		//}
		[TestMethod()]
		public void SaveJsonFileTest()
		{
			var p = new Person() { name = "rudy", adres = "Profeinthovenstraat" };
			Xml.SaveJsonFile(p, "person.json");
			var x = Xml.LoadJsonFile<Person>("person.json");
			Assert.IsTrue(x.name == "rudy");

		}
	}
		public class Person
	{
		public Person()
		{
		}

		public string name { get; set; }
		public string adres { get; set; }
	}
}