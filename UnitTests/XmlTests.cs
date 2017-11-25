using Microsoft.VisualStudio.TestTools.UnitTesting;
namespace My.UnitTests
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
			Assert.IsTrue(x.name =="rudy");
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