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
		[XmlAttribute] [DefaultValue("")] public string Speaker = "";
		[XmlAttribute] [DefaultValue("")] public string CustomSpeakerId = "";

		[XmlAttribute] [DefaultValue("")] public string Dialog = "";

		[XmlAttribute] [DefaultValue(false)] public bool InPlayerOffice = false;
		[XmlAttribute] [DefaultValue("")] public string CustomBackground = "";
		[XmlAttribute] [DefaultValue("")] public string NpcImage = "";

		public DialogEntry FromXml()
		{
			DialogEntry.Speaker.TryParse(Speaker, out DialogEntry.Speaker speaker);
			return new DialogEntry()
			{
				CurrSpeaker = speaker,
				CustomSpeakerId = CustomSpeakerId,
				Text = Dialog,
				NpcImage = NpcImage,
				InPlayerOffice = InPlayerOffice,
				CustomBackground = ImageLookup.Backgrounds.GetImage(CustomBackground),
			};
		}

		public static DialogEntryXml ToXml(DialogEntry ob)
		{
			return new DialogEntryXml()
			{
				Speaker = ob.CurrSpeaker.ToString(),
				CustomSpeakerId = ob.CustomSpeakerId,
				Dialog = ob.Text,
				InPlayerOffice = ob.InPlayerOffice,
				NpcImage = ob.NpcImage,
				CustomBackground = ob.CustomBackground?.name,
			};
		}
	}
}
