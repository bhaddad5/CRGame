using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Xml.Serialization;
using Assets.GameModel;
using Assets.GameModel.XmlParsers;
using GameModel.Serializers;
using UnityEngine;

public class MissionXml
{
	[XmlAttribute] [DefaultValue("")] public string MissionName = "";
	[XmlAttribute] [DefaultValue("")] public string MissionDescription = "";
	[XmlAttribute] [DefaultValue("")] public string MissionImage = "";

	[XmlAttribute] [DefaultValue("")] public string NpcId = "";
	[XmlAttribute] [DefaultValue("")] public string InteractionId = "";

	[XmlElement("Effect", typeof(EffectXml))]
	public EffectXml[] Effects = new EffectXml[0];

	public SerializedMission FromXml()
	{
		List<SerializedEffect> effects = new List<SerializedEffect>();
		foreach (var effectXml in Effects)
		{
			effects.Add(effectXml.FromXml());
		}

		return new SerializedMission()
		{
			MissionName = MissionName,
			MissionDescription = MissionDescription,
			MissionImage = MissionImage,
			CompletionInteractionId = InteractionId,
			Rewards = effects,
		};
	}
}
