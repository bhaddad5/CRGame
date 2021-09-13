using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Assets.GameModel.XmlParsers;
using GameModel.Serializers;
using UnityEngine;

namespace Assets.GameModel.XmlParsers
{
	public class XmlResolver
	{
		public SerializedGameData LoadXmlData(string xmlDataPath)
		{
			string gameData = File.ReadAllText(xmlDataPath);
			var serializer = new XmlSerializer(typeof(GameDataXml));
			var data = (GameDataXml) serializer.Deserialize(new StringReader(gameData));
			
			return data.FromXml();
		}
	}
}
