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
		[TestMethod()]
		public void SaveListJsonFileTest()
		{
			var list = new List<Person>();
			list.Add(new Person() { name = "Rudy", adres = "Profein{thovenstraat" });
			list.Add(new Person() { name = "Henk", adres = "Bathmens,eweg 11" });
			Xml.SaveJsonFile(list, "persons.json");
			var x = Xml.LoadJsonFile<List<Person>>("persons.json");
			Assert.IsTrue(x[0].name == "Rudy");
			Assert.IsTrue(x[1].name == "Henk");
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