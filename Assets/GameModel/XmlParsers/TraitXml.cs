using System.Collections.Generic;
using System.ComponentModel;
using System.Xml.Serialization;

namespace Assets.GameModel.XmlParsers
{
	public class TraitXml
	{
		[XmlAttribute] [DefaultValue("")] public string Id = "";
		[XmlAttribute] [DefaultValue("")] public string Name = "";

		[XmlElement("FreeEffect", typeof(EffectXml))]
		public EffectXml[] FreeEffects = new EffectXml[0];
		[XmlElement("ControlledEffect", typeof(EffectXml))]
		public EffectXml[] ControlledEffects = new EffectXml[0];

		public Trait FromXml()
		{
			List<Effect> effects = new List<Effect>();
			foreach (var effectXml in FreeEffects)
			{
				effects.Add(effectXml.FromXml());
			}
			List<Effect> ctrlEffects = new List<Effect>();
			foreach (var effectXml in ControlledEffects)
			{
				ctrlEffects.Add(effectXml.FromXml());
			}

			return new Trait()
			{
				Id = Id,
				Name = Name,
				FreeEffects = effects,
				ControlledEffects = ctrlEffects,
			};
		}
	}
}