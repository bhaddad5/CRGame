using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Xml.Serialization;
using UnityEngine;

namespace Assets.GameModel.XmlParsers
{
	public class TraitXml
	{
		[XmlAttribute] [DefaultValue("")] public string Id = "";
		[XmlAttribute] [DefaultValue("")] public string Name = "";

		[XmlElement("Effect", typeof(EffectXml))]
		public EffectXml[] PerTurnEffects = new EffectXml[0];

		public Trait FromXml()
		{
			List<Effect> effects = new List<Effect>();
			foreach (var effectXml in PerTurnEffects)
			{
				effects.Add(effectXml.FromXml());
			}

			return new Trait()
			{
				Id = Id,
				Name = Name,
				Effects = effects,
			};
		}
	}
}