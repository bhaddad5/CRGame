using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Assets.GameModel.XmlParsers
{
	public class InteractionResultXml
	{
		[XmlElement("DialogEntry", typeof(DialogEntryXml))]
		public DialogEntryXml[] DialogEntries = new DialogEntryXml[0];

		[XmlElement("Effect", typeof(EffectXml))]
		public EffectXml[] Effects = new EffectXml[0];

		public InteractionResult FromXml()
		{
			List<DialogEntry> dialogs = new List<DialogEntry>();
			foreach (var dialogEntryXml in DialogEntries)
			{
				dialogs.Add(dialogEntryXml.FromXml());
			}

			List<Effect> effects = new List<Effect>();
			foreach (var effectXml in Effects)
			{
				effects.Add(effectXml.FromXml());
			}

			return new InteractionResult()
			{
				Dialogs = dialogs,
				Effects = effects,
			};
		}

		public static InteractionResultXml ToXml(InteractionResult ob)
		{
			List<DialogEntryXml> dialogs = new List<DialogEntryXml>();
			foreach (var dialogEntry in ob.Dialogs)
			{
				dialogs.Add(DialogEntryXml.ToXml(dialogEntry));
			}

			List<EffectXml> effects = new List<EffectXml>();
			foreach (var effect in ob.Effects)
			{
				effects.Add(EffectXml.ToXml(effect));
			}

			return new InteractionResultXml()
			{
				DialogEntries = dialogs.ToArray(),
				Effects = effects.ToArray(),
			};
		}
	}
}
