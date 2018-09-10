using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace MyLib
{
	public static class Extensions
	{
		/// <summary>
		/// Compare two objects if they are binary equal.
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
		public static object DeepClone(this object obj)
		{
			if (obj == null) return null;
			object objResult = null;
			using (MemoryStream ms = new MemoryStream())
			{
				BinaryFormatter bf = new BinaryFormatter();
				bf.Serialize(ms, obj);

				ms.Position = 0;
				objResult = bf.Deserialize(ms);
			}
			return objResult;
		}
	}
}
