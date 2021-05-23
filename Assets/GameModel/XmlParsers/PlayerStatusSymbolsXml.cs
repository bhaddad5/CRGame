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
			LocationLayout layout = null;
			if ((CarLayout?.Length ?? 0) > 0)
			{
				layout = CarLayout[0].FromXml();
			}

			return new PlayerStatusSymbols()
			{
				CarImage = ImageLookup.StatusSymbols.GetImage(CarImage),
				CarName = CarName,
				CarLayout = layout,
			};
		}

		public static PlayerStatusSymbolsXml ToXml(PlayerStatusSymbols ob)
		{
			LocationLayoutXml[] layoutXml = null;
			if(ob.CarLayout != null)
				layoutXml = new []{LocationLayoutXml.ToXml(ob.CarLayout)};
			return new PlayerStatusSymbolsXml()
			{
				CarImage = ob.CarName,
				CarName = ob.CarName,
				CarLayout = layoutXml,
			};
		}
	}
}
