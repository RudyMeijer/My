using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyLib
{
	public static class Extensions
	{
		/// <summary>
		/// Compare two object if they are binary equal.
		/// Both objects are serialized to a byte array en then compared byte by byte.
		/// </summary>
		/// <param name="obj1"></param>
		/// <param name="obj2"></param>
		/// <returns></returns>
		public static bool IsEqual(this object obj1, object obj2)
		{
			//
			// Check for null.
			//
			if (obj1 == null && obj2 == null) return true;
			if (obj1 == null || obj2 == null) return false;
			var s1 = Xml.Serialize(obj1);
			var s2 = Xml.Serialize(obj2);
			if (s1.Length != s2.Length) return false;
			//
			// Compare each character of xml string.
			//
			for (int i = 0; i < s1.Length; i++)if (s1[i] != s2[i]) return false;
			return true;
		}
		public static T Clone<T>(this object obj)
		{
			//
			// If object is null then return default type.
			//
			if (obj == null) return default(T);
			var s1 = Xml.Serialize(obj);
			return Xml.Deserialize<T>(s1);
		}
	}
}
