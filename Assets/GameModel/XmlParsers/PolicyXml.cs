using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Xml.Serialization;
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

		[XmlElement("Effect", typeof(EffectXml))]
		public EffectXml[] Effects = new EffectXml[0];

		[XmlElement("Requirements", typeof(ActionRequirementsXml))]
		public ActionRequirementsXml[] Requirements = new ActionRequirementsXml[0];

		public Policy FromXml()
		{
			List<Effect> effects = new List<Effect>();
			foreach (var effectXml in Effects)
			{
				effects.Add(effectXml.FromXml());
			}

			return new Policy()
			{
				Id = Id,
				Name = Name,
				Active = Active,
				Description = Description,
                Image = ImageLookup.Policies.GetImage(Image),
                Requirements = Requirements[0].FromXml(),
				Effects = effects,
			};
		}

		public static PolicyXml ToXml(Policy ob)
		{
			List<EffectXml> effects = new List<EffectXml>();
			foreach (var effect in ob.Effects)
			{
				effects.Add(EffectXml.ToXml(effect));
			}

			return new PolicyXml()
			{
				Id = ob.Id,
				Name = ob.Name,
				Active = ob.Active,
				Description = ob.Description,
				Image = ob.Image.name,
				Requirements = new[] { ActionRequirementsXml.ToXml(ob.Requirements) },
				Effects = effects.ToArray(),
			};
		}
	}
}