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
		public GameData LoadXmlData()
		{
			var gameData = (TextAsset) Resources.Load("XmlData/GameData");

			var serializer = new XmlSerializer(typeof(GameDataXml));
			var data = (GameDataXml) serializer.Deserialize(new StringReader(gameData.text));
			
			return data.FromXml();
		}
	}
}
