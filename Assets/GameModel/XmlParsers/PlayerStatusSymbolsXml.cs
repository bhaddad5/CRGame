using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Xml.Serialization;

namespace Assets.GameModel.XmlParsers
{
	public class PlayerStatusSymbolsXml
	{
		[XmlAttribute] [DefaultValue("")] public string CarName = "";
		[XmlAttribute] [DefaultValue("")] public string CarImage = "";

		public PlayerStatusSymbols FromXml()
		{
			return new PlayerStatusSymbols()
			{
				CarImage = ImageLookup.StatusSymbols.GetImage(CarImage),
				CarName = CarName,
			};
		}

		public static PlayerStatusSymbolsXml ToXml(PlayerStatusSymbols ob)
		{
			return new PlayerStatusSymbolsXml()
			{
				CarImage = ob.CarName,
				CarName = ob.CarName,
			};
		}
	}
}
