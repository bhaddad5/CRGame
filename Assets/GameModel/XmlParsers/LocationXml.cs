using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Xml.Serialization;
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

		[XmlElement("Fem", typeof(FemXml))]
		public FemXml[] Fems = new FemXml[0];

		[XmlElement("Policy", typeof(PolicyXml))]
		public PolicyXml[] Policies = new PolicyXml[0];

		[XmlElement("Mission", typeof(MissionXml))]
		public MissionXml[] Missions = new MissionXml[0];

		public Location FromXml()
		{
			List<Fem> fems = new List<Fem>();
			foreach (var femXml in Fems)
			{
				fems.Add(femXml.FromXml());
			}

			List<Policy> policies = new List<Policy>();
			foreach (var policyXml in Policies ?? new PolicyXml[0])
			{
				policies.Add(policyXml.FromXml());
			}

			List<Mission> missions = new List<Mission>();
			foreach (var missionXml in Missions ?? new MissionXml[0])
			{
				missions.Add(missionXml.FromXml());
			}

			return new Location()
			{
				Id = Id,
				Name = Name,
				ClosedOnWeekends = ClosedOnWeekends,
				Accessible = Accessible,
				Fems = fems,
				Policies = policies,
				Missions = missions,
				UiPosition = new Vector2(UiPosX, UiPosY),
				BackgroundImage = ImageLookup.Backgrounds.GetImage(BackgroundImage),
				Icon = ImageLookup.Icons.GetImage(LocationIcon),
			};
		}

		public static LocationXml ToXml(Location ob)
		{
			List<FemXml> fems = new List<FemXml>();
			foreach (var fem in ob.Fems)
			{
				fems.Add(FemXml.ToXml(fem));
			}

			List<PolicyXml> policies = new List<PolicyXml>();
			foreach (var policy in ob.Policies)
			{
				policies.Add(PolicyXml.ToXml(policy));
			}

			List<MissionXml> missions = new List<MissionXml>();
			foreach (var mission in ob.Missions)
			{
				missions.Add(MissionXml.ToXml(mission));
			}

			return new LocationXml()
			{
				Id = ob.Id,
				Name = ob.Name,
				ClosedOnWeekends = ob.ClosedOnWeekends,
				Accessible = ob.Accessible,
				Fems = fems.ToArray(),
				Policies = policies.ToArray(),
				Missions = missions.ToArray(),
				UiPosX = ob.UiPosition.x,
				UiPosY = ob.UiPosition.y,
				BackgroundImage = ob.BackgroundImage.name,
				LocationIcon = ob.Icon.name,
			};
		}
	}
}