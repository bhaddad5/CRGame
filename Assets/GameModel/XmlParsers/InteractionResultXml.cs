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
	public class InteractionResultXml
	{
		[XmlAttribute] [DefaultValue(1)] public int Probability = 1;

		[XmlElement("DialogEntry", typeof(DialogEntryXml))]
		public DialogEntryXml[] DialogEntries = new DialogEntryXml[0];

		[XmlElement("Popup", typeof(PopupXml))]
		public PopupXml Popup = null;

		[XmlElement("Effect", typeof(EffectXml))]
		public EffectXml[] Effects = new EffectXml[0];

		public SerializedInteractionResult FromXml()
		{
			List<SerializedDialogEntry> dialogs = new List<SerializedDialogEntry>();
			foreach (var dialogEntryXml in DialogEntries ?? new DialogEntryXml[0])
			{
				dialogs.Add(dialogEntryXml.FromXml());
			}

			List<SerializedEffect> effects = new List<SerializedEffect>();
			foreach (var effectXml in Effects)
			{
				effects.Add(effectXml.FromXml());
			}

			List<SerializedPopup> popups = new List<SerializedPopup>();
			if (Popup != null)
			{
				popups.Add(Popup.FromXml());
			}

			return new SerializedInteractionResult()
			{
				Probability = Probability,
				Dialogs = dialogs,
				OptionalPopups = popups,
				Effects = effects,
			};
		}
	}
}
