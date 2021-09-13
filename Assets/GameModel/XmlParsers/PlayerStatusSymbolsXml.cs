using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Xml.Serialization;
using GameModel.Serializers;

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
		
		public SerializedPlayerStatysSymbols FromXml()
		{
			return new SerializedPlayerStatysSymbols()
			{
				CarImage = CarImage,
				CarName = CarName,
				SuitsImage = SuitsImage,
				SuitsName = SuitsName,
				JewleryCuffs = JewleryCuffs,
				JewleryPen = JewleryPen,
				JewleryRing = JewleryRing,
				JewleryWatch = JewleryWatch,
			};
		}
	}
}
