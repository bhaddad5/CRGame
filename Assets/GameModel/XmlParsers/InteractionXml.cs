using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Assets.GameModel.XmlParsers
{
	public class InteractionXml
	{
		[XmlAttribute] [DefaultValue("")] public string Id = "";
		[XmlAttribute] [DefaultValue("")] public string Name = "";
		[XmlAttribute] [DefaultValue("")] public string Dialog = "";

		[XmlAttribute] [DefaultValue(1)] public int TurnCost = 1;
		[XmlAttribute] [DefaultValue(0)] public float EgoCost = 0;
		[XmlAttribute] [DefaultValue(0)] public float MoneyCost = 0;

		[XmlAttribute] [DefaultValue("")] public string RequiredPolicies = "";
		[XmlAttribute] [DefaultValue(false)] public bool RequiredControl = false;
		[XmlAttribute] [DefaultValue(-1)] public float RequiredAmbition = -1;
		[XmlAttribute] [DefaultValue(-1)] public float RequiredPride = -1;

		[XmlElement("Effect", typeof(EffectXml))]
		public EffectXml[] Effects = new EffectXml[0];

		public Interaction FromXml()
		{
			List<Effect> effects = new List<Effect>();
			foreach (var e in Effects)
			{
				effects.Add(e.FromXml());
			}

			return new Interaction()
			{
				Id = Id,
				Name = Name,
				Dialog = Dialog,
				TurnCost = TurnCost,
				EgoCost = EgoCost,
				MoneyCost = MoneyCost,
				RequiredPolicies = RequiredPolicies.XmlStringToList(),
				RequiredAmbition = RequiredAmbition,
				RequiredPride = RequiredPride,
				RequiredControl = RequiredControl,
				Effects = effects,
			};
		}
	}
}
