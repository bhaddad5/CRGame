using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Assets.GameModel.XmlParsers;
using UnityEngine;

namespace Assets.GameModel.XmlParsers
{
	public class XmlResolver
	{
		public GameData LoadXmlData(string xmlDataPath)
		{
			string gameData = File.ReadAllText(xmlDataPath);
			var serializer = new XmlSerializer(typeof(GameDataXml));
			var data = (GameDataXml) serializer.Deserialize(new StringReader(gameData));
			
			return data.FromXml();
		}

		public string SerializeXmlData(GameData data)
		{
			var xml = GameDataXml.ToXml(data);
			var serializer = new XmlSerializer(typeof(GameDataXml));
			using (StringWriter textWriter = new StringWriter())
			{
				serializer.Serialize(textWriter, xml);
				return textWriter.ToString();
			}
		}
	}
}
