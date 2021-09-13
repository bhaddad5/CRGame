using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Xml.Serialization;
using GameModel.Serializers;
using UnityEngine;

namespace Assets.GameModel.XmlParsers
{
	public class LocationXml
	{
		[XmlAttribute] [DefaultValue("")] public string Id = "";
		[XmlAttribute] [DefaultValue("")] public string Name = "";
		[XmlAttribute] [DefaultValue(false)] public bool ClosedOnWeekends = false;
		[XmlAttribute] [DefaultValue(false)] public bool Accessible = false;
		[XmlAttribute] [DefaultValue(0)] public float UiPosX = 0;
		[XmlAttribute] [DefaultValue(0)] public float UiPosY = 0;

		[XmlAttribute] [DefaultValue("")] public string LocationIcon = "";
		[XmlAttribute] [DefaultValue("")] public string BackgroundImage = "";

		[XmlAttribute] [DefaultValue(false)] public bool ShowTrophyCase = false;
		[XmlAttribute] [DefaultValue(false)] public bool ShowCar = false;

		[XmlElement("Npc", typeof(NpcXml))]
		public NpcXml[] Npcs = new NpcXml[0];

		[XmlElement("Policy", typeof(PolicyXml))]
		public PolicyXml[] Policies = new PolicyXml[0];

		[XmlElement("Mission", typeof(MissionXml))]
		public MissionXml[] Missions = new MissionXml[0];

		public SerializedLocation FromXml()
		{
			List<SerializedNpc> npcs = new List<SerializedNpc>();
			foreach (var npcXml in Npcs ?? new NpcXml[0])
			{
				npcs.Add(npcXml.FromXml());
			}

			List<SerializedPolicy> policies = new List<SerializedPolicy>();
			foreach (var policyXml in Policies ?? new PolicyXml[0])
			{
				policies.Add(policyXml.FromXml());
			}

			List<SerializedMission> missions = new List<SerializedMission>();
			foreach (var missionXml in Missions ?? new MissionXml[0])
			{
				missions.Add(missionXml.FromXml());
			}

			return new SerializedLocation()
			{
				Id = Id,
				Name = Name,
				ClosedOnWeekends = ClosedOnWeekends,
				Accessible = Accessible,
				Npcs = npcs,
				Policies = policies,
				Missions = missions,
				UiPosition = new Vector2(UiPosX, UiPosY),
				BackgroundImage = BackgroundImage,
				Icon = LocationIcon,
				ShowCar = ShowCar,
				ShowTrophyCase = ShowTrophyCase,
			};
		}
	}
}