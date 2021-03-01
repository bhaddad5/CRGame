using System.Collections.Generic;
using System.ComponentModel;
using System.Xml.Serialization;

namespace Assets.GameModel.XmlParsers
{
	public class OfficeLayoutXml
	{
		[XmlAttribute] [DefaultValue(.5f)] public float XPercentage = .5f;
		[XmlAttribute] [DefaultValue(.5f)] public float YPercentage = .5f;
		[XmlAttribute] [DefaultValue(200f)] public float Width = 200f;

		public Fem.OfficeLayout FromXml()
		{
			return new Fem.OfficeLayout()
			{
				X = XPercentage,
				Y = YPercentage,
				Width = Width,
			};
		}

		public static OfficeLayoutXml ToXml(Fem.OfficeLayout ob)
		{
			return new OfficeLayoutXml()
			{
				Width = ob.Width,
				XPercentage = ob.X,
				YPercentage = ob.Y,
			};
		}
	}

	public class FemXml
	{
		[XmlAttribute] [DefaultValue("")] public string Id = "";
		[XmlAttribute] [DefaultValue("")] public string FirstName = "";
		[XmlAttribute] [DefaultValue("")] public string LastName = "";
		[XmlAttribute] [DefaultValue(0)] public int Age = 0;
		[XmlAttribute] [DefaultValue(false)] public bool Controlled = false;
		[XmlAttribute] [DefaultValue(0)] public float Ambition = 0;
		[XmlAttribute] [DefaultValue(0)] public float Pride = 0;

		[XmlAttribute] [DefaultValue("")] public string BackgroundImage = "";

		[XmlElement("OfficeLayout", typeof(OfficeLayoutXml))]
		public OfficeLayoutXml[] OfficeLayout = new OfficeLayoutXml[0];

		[XmlElement("Trait", typeof(TraitXml))]
		public TraitXml[] Traits = new TraitXml[0];

		[XmlElement("Trophy", typeof(TrophyXml))]
		public TrophyXml[] Trophies = new TrophyXml[0];

		[XmlElement("Interaction", typeof(InteractionXml))]
		public InteractionXml[] Interactions = new InteractionXml[0];

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

			List<Trophy> trophies = new List<Trophy>();
			foreach (var trophyXml in Trophies ?? new TrophyXml[0])
			{
				trophies.Add(trophyXml.FromXml());
			}

			OfficeLayoutXml layoutXml = new OfficeLayoutXml();
			if ((OfficeLayout?.Length ?? 0) > 0)
			{
				layoutXml = OfficeLayout[0];
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
				Trophies = trophies,
				BackgroundImage = ImageLookup.Backgrounds.GetImage(BackgroundImage),
				Layout = layoutXml.FromXml(),
			};
		}

		public static FemXml ToXml(Fem ob)
		{
			List<InteractionXml> interactions = new List<InteractionXml>();
			foreach (var interaction in ob.Interactions)
			{
				interactions.Add(InteractionXml.ToXml(interaction));
			}

			List<TraitXml> traits = new List<TraitXml>();
			foreach (var trait in ob.Traits)
			{
				traits.Add(TraitXml.ToXml(trait));
			}

			List<TrophyXml> trophies = new List<TrophyXml>();
			foreach (var trophy in ob.Trophies)
			{
				trophies.Add(TrophyXml.ToXml(trophy));
			}

			return new FemXml()
			{
				Id = ob.Id,
				Ambition = ob.Ambition,
				Controlled = ob.Controlled,
				Pride = ob.Pride,
				FirstName = ob.FirstName,
				LastName = ob.LastName,
				Age = ob.Age,
				Interactions = interactions.ToArray(),
				Traits = traits.ToArray(),
				Trophies = trophies.ToArray(),
				BackgroundImage = ob.BackgroundImage.name,
				OfficeLayout = new []{OfficeLayoutXml.ToXml(ob.Layout)}
			};
		}
	}
}