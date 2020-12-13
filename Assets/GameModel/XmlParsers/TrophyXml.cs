using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Assets.GameModel.XmlParsers
{
	public class TrophyXml
	{
		[XmlAttribute] [DefaultValue("")] public string Id = "";
		[XmlAttribute] [DefaultValue("")] public string Name = "";
		[XmlAttribute] [DefaultValue("")] public string Image = "";
		[XmlAttribute] [DefaultValue(false)] public bool Owned = false;

		public Trophy FromXml()
		{
			return new Trophy()
			{
				Id = Id,
				Name = Name,
				Image = ImageLookup.Trophies.GetImage(Image),
				Owned = Owned,
			};
		}

		public static TrophyXml ToXml(Trophy ob)
		{
			return new TrophyXml()
			{
				Id = ob.Id,
				Name = ob.Name,
				Image = ob.Image.name,
				Owned = ob.Owned,
			};
		}
	}
}
