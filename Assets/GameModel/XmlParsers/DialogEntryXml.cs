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
	public class DialogEntryXml
	{
		[XmlAttribute] [DefaultValue("")] public string Speaker = "";
		[XmlAttribute] [DefaultValue("")] public string CustomSpeakerId = "";

		[XmlAttribute] [DefaultValue("")] public string Dialog = "";

		[XmlAttribute] [DefaultValue(false)] public bool InPlayerOffice = false;
		[XmlAttribute] [DefaultValue("")] public string CustomBackground = "";
		[XmlAttribute] [DefaultValue("")] public string NpcImage = "";

		public SerializedDialogEntry FromXml()
		{
			return new SerializedDialogEntry()
			{
				CurrSpeaker = Speaker,
				CustomSpeakerReference = CustomSpeakerId,
				Text = Dialog,
				NpcImage = NpcImage,
				InPlayerOffice = InPlayerOffice,
				CustomBackground = CustomBackground,
			};
		}
	}
}
