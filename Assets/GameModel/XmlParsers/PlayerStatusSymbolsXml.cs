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

		[XmlAttribute] [DefaultValue("")] public string SuitsName = "";
		[XmlAttribute] [DefaultValue("")] public string SuitsImage = "";

		[XmlAttribute] [DefaultValue("")] public string JewleryCuffs = "";
		[XmlAttribute] [DefaultValue("")] public string JewleryPen = "";
		[XmlAttribute] [DefaultValue("")] public string JewleryRing = "";
		[XmlAttribute] [DefaultValue("")] public string JewleryWatch = "";


		[XmlElement("CarLayout", typeof(LocationLayoutXml))]
		public LocationLayoutXml[] CarLayout = new LocationLayoutXml[0];

		public PlayerStatusSymbols FromXml()
		{
			LocationLayout carLayout = null;
			if ((CarLayout?.Length ?? 0) > 0)
			{
				carLayout = CarLayout[0].FromXml();
			}
			
			return new PlayerStatusSymbols()
			{
				CarImage = ImageLookup.StatusSymbols.GetImage(CarImage),
				CarName = CarName,
				CarLayout = carLayout,
				SuitsImage = ImageLookup.StatusSymbols.GetImage(SuitsImage),
				SuitsName = SuitsName,
				JewleryCuffs = ImageLookup.StatusSymbols.GetImage(JewleryCuffs),
				JewleryPen = ImageLookup.StatusSymbols.GetImage(JewleryPen),
				JewleryRing = ImageLookup.StatusSymbols.GetImage(JewleryRing),
				JewleryWatch = ImageLookup.StatusSymbols.GetImage(JewleryWatch),
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
				SuitsImage = ob.SuitsImage.name,
				SuitsName = ob.SuitsName,
				JewleryCuffs = ob.JewleryCuffs.name,
				JewleryPen = ob.JewleryPen.name,
				JewleryRing = ob.JewleryRing.name,
				JewleryWatch = ob.JewleryWatch.name,
			};
		}
	}
}
