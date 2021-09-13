using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Xml.Serialization;
using GameModel.Serializers;
using UnityEngine;

namespace Assets.GameModel.XmlParsers
{
	public class PolicyXml
	{
		[XmlAttribute] [DefaultValue("")] public string Id = "";
		[XmlAttribute] [DefaultValue("")] public string Name = "";
		[XmlAttribute] [DefaultValue(false)] public bool Active = false;

        [XmlAttribute] [DefaultValue("")] public string Image = "";
        [XmlAttribute] [DefaultValue("")] public string Description = "";

		[XmlElement("Requirements", typeof(ActionRequirementsXml))]
		public ActionRequirementsXml[] Requirements = new ActionRequirementsXml[0];

		[XmlElement("Effect", typeof(EffectXml))]
		public EffectXml[] Effects = new EffectXml[0];

		public SerializedPolicy FromXml()
		{
			List<SerializedEffect> effects = new List<SerializedEffect>();
			foreach (var effectXml in Effects)
			{
				effects.Add(effectXml.FromXml());
			}

			return new SerializedPolicy()
			{
				Id = Id,
				Name = Name,
				Active = Active,
				Description = Description,
                Image = Image,
                Requirements = Requirements[0].FromXml(),
				Effects = effects,
			};
		}
	}
}