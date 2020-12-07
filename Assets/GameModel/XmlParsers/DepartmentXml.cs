using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Xml.Serialization;
using UnityEngine;

namespace Assets.GameModel.XmlParsers
{
	public class DepartmentXml
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

		public Department FromXml()
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

			return new Department()
			{
				Id = Id,
				Name = Name,
				ClosedOnWeekends = ClosedOnWeekends,
				Accessible = Accessible,
				Fems = fems,
				Policies = policies,
				UiPosition = new Vector2(UiPosX, UiPosY),
				BackgroundImage = BackgroundImagesLookup.GetBackgroundImage(BackgroundImage),
				Icon = LocationIconLookup.GetLocationIcon(LocationIcon),
			};
		}

		public static DepartmentXml ToXml(Department ob)
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

			return new DepartmentXml()
			{
				Id = ob.Id,
				Name = ob.Name,
				ClosedOnWeekends = ob.ClosedOnWeekends,
				Accessible = ob.Accessible,
				Fems = fems.ToArray(),
				Policies = policies.ToArray(),
				UiPosX = ob.UiPosition.x,
				UiPosY = ob.UiPosition.y,
				BackgroundImage = ob.BackgroundImage.name,
				LocationIcon = ob.Icon.name,
			};
		}
	}
}