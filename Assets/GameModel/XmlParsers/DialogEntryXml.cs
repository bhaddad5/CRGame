using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Assets.GameModel.XmlParsers
{
	public class DialogEntryXml
	{
		[XmlAttribute] [DefaultValue(false)] public bool IsPlayer = false;
		[XmlAttribute] [DefaultValue("")] public string Dialog = "";
		[XmlAttribute] [DefaultValue("")] public string NpcImage = "";

		public DialogEntry FromXml()
		{
			return new DialogEntry()
			{
				IsPlayer = IsPlayer,
				Text = Dialog,
				NpcImage = NpcImage,
			};
		}
	}
}
