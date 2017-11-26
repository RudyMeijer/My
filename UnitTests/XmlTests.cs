using Microsoft.VisualStudio.TestTools.UnitTesting;
using MyLib;
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

		[TestMethod()]
		public void SerializeJsonTest()
		{
			string expect = "{\"adres\":\"Profeinthovenstraat\",\"name\":\"rudy\"}";
			var p = new Person() { name = "rudy", adres = "Profeinthovenstraat" };
			var x = Xml.SerializeJson(p);
			Assert.IsTrue(x== expect, $"Error in json");
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