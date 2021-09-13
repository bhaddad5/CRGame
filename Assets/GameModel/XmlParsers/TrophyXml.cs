using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using GameModel.Serializers;

namespace Assets.GameModel.XmlParsers
{
	public class TrophyXml
	{
		[XmlAttribute] [DefaultValue("")] public string Id = "";
		[XmlAttribute] [DefaultValue("")] public string Name = "";
		[XmlAttribute] [DefaultValue("")] public string Image = "";
		[XmlAttribute] [DefaultValue(false)] public bool Owned = false;

		public SerializedTrophy FromXml()
		{
			return new SerializedTrophy()
			{
				Id = Id,
				Name = Name,
				Image = Image,
				Owned = Owned,
			};
		}
	}
}
