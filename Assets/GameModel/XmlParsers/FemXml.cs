using System.Collections.Generic;
using System.ComponentModel;
using System.Xml.Serialization;

namespace Assets.GameModel.XmlParsers
{
	public class FemXml
	{
		[XmlAttribute] [DefaultValue("")] public string Id = "";
		[XmlAttribute] [DefaultValue("")] public string FirstName = "";
		[XmlAttribute] [DefaultValue("")] public string LastName = "";
		[XmlAttribute] [DefaultValue(0)] public int Age = 0;
		[XmlAttribute] [DefaultValue(false)] public bool Controlled = false;
		[XmlAttribute] [DefaultValue(0)] public float Ambition = 0;
		[XmlAttribute] [DefaultValue(0)] public float Pride = 0;

		[XmlElement("Interaction", typeof(InteractionXml))]
		public InteractionXml[] Interactions = new InteractionXml[0];

		[XmlElement("Trait", typeof(TraitXml))]
		public TraitXml[] Traits = new TraitXml[0];

		public Fem FromXml()
		{
			List<Interaction> interactions = new List<Interaction>();
			foreach (var interactionXml in Interactions ?? new InteractionXml[0])
			{
				interactions.Add(interactionXml.FromXml());
			}

			List<Trait> traits = new List<Trait>();
			foreach (var traitXml in Traits ?? new TraitXml[0])
			{
				traits.Add(traitXml.FromXml());
			}

			return new Fem()
			{
				Id = Id,
				Ambition = Ambition,
				Controlled = Controlled,
				Pride = Pride,
				FirstName = FirstName,
				LastName = LastName,
				Age = Age,
				Interactions = interactions,
				Traits = traits,
			};
		}
	}
}