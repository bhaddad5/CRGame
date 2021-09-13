using System.Collections.Generic;
using System.ComponentModel;
using System.Xml.Serialization;
using GameModel.Serializers;

namespace Assets.GameModel.XmlParsers
{
	public class LocationLayoutXml
	{
		[XmlAttribute] [DefaultValue(0)] public float XPercentage = 0;
		[XmlAttribute] [DefaultValue(0)] public float YPercentage = 0;
		[XmlAttribute] [DefaultValue(100f)] public float Width = 100;
	}

	public class NpcXml
	{
		[XmlAttribute] [DefaultValue("")] public string Id = "";
		[XmlAttribute] [DefaultValue("")] public string FirstName = "";
		[XmlAttribute] [DefaultValue("")] public string LastName = "";
		[XmlAttribute] [DefaultValue(0)] public int Age = 0;
		[XmlAttribute] [DefaultValue(true)] public bool IsControllable = true;
		[XmlAttribute] [DefaultValue(false)] public bool Controlled = false;
		[XmlAttribute] [DefaultValue(0)] public float Ambition = 0;
		[XmlAttribute] [DefaultValue(0)] public float Pride = 0;
		[XmlAttribute] [DefaultValue("")] public string RequiredVisibilityInteraction = "";
		[XmlAttribute] [DefaultValue("")] public string BackgroundImage = "";

		[XmlElement("LocationLayout", typeof(LocationLayoutXml))]
		public LocationLayoutXml[] LocationLayout = new LocationLayoutXml[0];

		[XmlElement("PersonalLayout", typeof(LocationLayoutXml))]
		public LocationLayoutXml[] PersonalLayout = new LocationLayoutXml[0];
		
		[XmlElement("Trophy", typeof(TrophyXml))]
		public TrophyXml[] Trophies = new TrophyXml[0];

		[XmlElement("Interaction", typeof(InteractionXml))]
		public InteractionXml[] Interactions = new InteractionXml[0];

		public SerializedNpc FromXml()
		{
			List<SerializedInteraction> interactions = new List<SerializedInteraction>();
			foreach (var interactionXml in Interactions ?? new InteractionXml[0])
			{
				interactions.Add(interactionXml.FromXml());
			}
			
			List<SerializedTrophy> trophies = new List<SerializedTrophy>();
			foreach (var trophyXml in Trophies ?? new TrophyXml[0])
			{
				trophies.Add(trophyXml.FromXml());
			}

			LocationLayoutXml layoutXml = new LocationLayoutXml();
			if ((LocationLayout?.Length ?? 0) > 0)
			{
				layoutXml = LocationLayout[0];
			}

			LocationLayoutXml personalLayoutXml = new LocationLayoutXml();
			if ((PersonalLayout?.Length ?? 0) > 0)
			{
				personalLayoutXml = PersonalLayout[0];
			}

			return new SerializedNpc()
			{
				Id = Id,
				Ambition = Ambition,
				Controlled = Controlled,
				Pride = Pride,
				FirstName = FirstName,
				LastName = LastName,
				Age = Age,
				Interactions = interactions,
				Trophies = trophies,
				RequiredVisibilityInteraction = RequiredVisibilityInteraction,
				BackgroundImage = BackgroundImage,
				LocationLayoutXPos = layoutXml.XPercentage,
				LocationLayoutYPos = layoutXml.YPercentage,
				LocationLayoutWidth = layoutXml.Width,
				PersonalLayoutXPos = personalLayoutXml.XPercentage,
				PersonalLayoutYPos = personalLayoutXml.YPercentage,
				PersonalLayoutWidth = personalLayoutXml.Width,
				IsControllable = IsControllable,
			};
		}
	}
}