using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Xml.Serialization;
using Assets.GameModel;
using Assets.GameModel.XmlParsers;
using UnityEngine;

public class MissionXml
{
	[XmlAttribute] [DefaultValue("")] public string MissionName = "";
	[XmlAttribute] [DefaultValue("")] public string MissionDescription = "";
	[XmlAttribute] [DefaultValue("")] public string MissionImage = "";

	[XmlAttribute] [DefaultValue("")] public string FemId = "";
	[XmlAttribute] [DefaultValue("")] public string InteractionId = "";

	[XmlElement("Effect", typeof(EffectXml))]
	public EffectXml[] Effects = new EffectXml[0];

	public Mission FromXml()
	{
		List<Effect> effects = new List<Effect>();
		foreach (var effectXml in Effects)
		{
			effects.Add(effectXml.FromXml());
		}

		return new Mission()
		{
			MissionName = MissionName,
			MissionDescription = MissionDescription,
			MissionImage = ImageLookup.Missions.GetImage(MissionImage),
			FemId = FemId,
			InteractionId = InteractionId,
			Rewards = effects,
		};
	}

	public static MissionXml ToXml(Mission ob)
	{
		List<EffectXml> effects = new List<EffectXml>();
		foreach (var effect in ob.Rewards)
		{
			effects.Add(EffectXml.ToXml(effect));
		}

		return new MissionXml()
		{
			MissionImage = ob.MissionImage.name,
			MissionDescription = ob.MissionDescription,
			MissionName = ob.MissionName,
			FemId = ob.FemId,
			InteractionId = ob.InteractionId,
			Effects = effects.ToArray(),
		};
	}
}
