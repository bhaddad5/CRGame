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


		[XmlElement("CarLayout", typeof(LocationLayoutXml))]
		public LocationLayoutXml[] CarLayout = new LocationLayoutXml[0];

		public PlayerStatusSymbols FromXml()
		{
			LocationLayoutXml layoutXml = new LocationLayoutXml();
			if ((CarLayout?.Length ?? 0) > 0)
			{
				layoutXml = CarLayout[0];
			}

			return new PlayerStatusSymbols()
			{
				CarImage = ImageLookup.StatusSymbols.GetImage(CarImage),
				CarName = CarName,
				CarLayout = layoutXml.FromXml(),
			};
		}

		public static PlayerStatusSymbolsXml ToXml(PlayerStatusSymbols ob)
		{
			return new PlayerStatusSymbolsXml()
			{
				CarImage = ob.CarName,
				CarName = ob.CarName,
				CarLayout = new[] { LocationLayoutXml.ToXml(ob.CarLayout) },
			};
		}
	}
}
