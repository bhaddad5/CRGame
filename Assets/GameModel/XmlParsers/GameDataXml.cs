using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Xml.Serialization;

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

		[XmlElement("Location", typeof(LocationXml))]
		public LocationXml[] Locations = new LocationXml[0];

		public GameData FromXml()
		{
			List<Location> locations = new List<Location>();
			foreach (var locationXml in Locations)
			{
				locations.Add(locationXml.FromXml());
			}

			return new GameData()
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
			};
		}

		public static GameDataXml ToXml(GameData ob)
		{
			List<LocationXml> locations = new List<LocationXml>();
			foreach (var loc in ob.Locations)
			{
				locations.Add(LocationXml.ToXml(loc));
			}

			return new GameDataXml()
			{
				PlayerName = ob.PlayerName,
				TurnNumber = ob.TurnNumber,
				Ego = ob.Ego,
				Funds = ob.Funds,
				Power = ob.Power,
				Patents = ob.Patents,
				CorporateCulture = ob.CorporateCulture,
				Spreadsheets = ob.Spreadsheets,
				Brand = ob.Brand,
				Revenue = ob.Revenue,
				Hornical = ob.Hornical,
				Locations = locations.ToArray(),
			};
		}
	}
}