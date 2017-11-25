using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static My;

namespace UnitTests
{
	[TestClass()]
	public class XmlTests
	{
		[TestMethod()]
		public void SaveFileTest()
		{
			var p = new Person() { name = "rudy", adres = "Profeinthovenstraat" };
			//Xml.SaveFile(p, "person.xml");
			//Assert.IsTrue();
		}
	}

	internal class Person
	{
		public Person()
		{
		}

		public string name { get; set; }
		public string adres { get; set; }
	}
}