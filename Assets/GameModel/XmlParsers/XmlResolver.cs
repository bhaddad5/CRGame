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
			string gameData;
			if (File.Exists(Path.Combine(Application.streamingAssetsPath, "GameData.xml")))
			{
				gameData = File.ReadAllText(Path.Combine(Application.streamingAssetsPath, "GameData.xml"));
			}
			else
			{	
				gameData = ((TextAsset)Resources.Load("XmlData/GameData")).text;
			}

			var serializer = new XmlSerializer(typeof(GameDataXml));
			var data = (GameDataXml) serializer.Deserialize(new StringReader(gameData));
			
			return data.FromXml();
		}
	}
}
