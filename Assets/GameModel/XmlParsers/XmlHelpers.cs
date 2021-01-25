using System;
using System.Collections.Generic;

namespace Assets.GameModel.XmlParsers
{
	public static class XmlHelpers
	{
		public static List<string> XmlStringToList(this string str)
		{
			List<string> res = new List<string>();
			foreach (string item in str.Split(','))
			{
				var i = item.Trim();
				if (!String.IsNullOrEmpty(i))
					res.Add(i);
			}

			return res;
		}

		public static string ListToXmlString(this List<string> strs)
		{
			string res = "";
			foreach (var str in strs)
			{
				res += str + ",";
			}
			return res;
		}
	}
}
