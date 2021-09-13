using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Xml.Serialization;
using GameModel.Serializers;

namespace Assets.GameModel.XmlParsers
{
	[XmlRoot(ElementName = "GameData")]
	public class GameDataXml
	{
		[XmlAttribute] [DefaultValue("")] public string PlayerName = "";
		[XmlAttribute] [DefaultValue(0)] public int TurnNumber = 0;
		[XmlAttribute] [DefaultValue(0)] public float Ego = 0;
		[XmlAttribute] [DefaultValue(0)] public float Funds = 0;
		[XmlAttribute] [DefaultValue(0)] public float Power = 0;
		[XmlAttribute] [DefaultValue(0)] public float Patents = 0;
		[XmlAttribute] [DefaultValue(0)] public float CorporateCulture = 0;
		[XmlAttribute] [DefaultValue(0)] public float Spreadsheets = 0;
		[XmlAttribute] [DefaultValue(0)] public float Brand = 0;
		[XmlAttribute] [DefaultValue(0)] public float Revenue = 0;
		[XmlAttribute] [DefaultValue(0)] public int Hornical = 0;

		[XmlElement("StatusSymbols", typeof(PlayerStatusSymbolsXml))]
		public PlayerStatusSymbolsXml[] StatusSymbols = new PlayerStatusSymbolsXml[0];

		[XmlElement("Location", typeof(LocationXml))]
		public LocationXml[] Locations = new LocationXml[0];

		public SerializedGameData FromXml()
		{
			List<SerializedLocation> locations = new List<SerializedLocation>();
			foreach (var locationXml in Locations)
			{
				locations.Add(locationXml.FromXml());
			}

			SerializedPlayerStatysSymbols statusSymbols = StatusSymbols[0].FromXml();

			return new SerializedGameData()
			{
				PlayerName = PlayerName,
				TurnNumber = TurnNumber,
				Ego = Ego,
				Funds = Funds,
				Power = Power,
				Patents = Patents,
				CorporateCulture = CorporateCulture,
				Spreadsheets = Spreadsheets,
				Brand = Brand,
				Revenue = Revenue,
				Hornical = Hornical,
				Locations = locations,
				StatusSymbols = statusSymbols,
			};
		}
	}
}