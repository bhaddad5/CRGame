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
		[XmlAttribute] [DefaultValue(false)] public bool Accessible = false;
		[XmlAttribute] [DefaultValue(0)] public float UiPosX = 0;
		[XmlAttribute] [DefaultValue(0)] public float UiPosY = 0;

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
				Accessible = Accessible,
				Fems = fems,
				Policies = policies,
				UiPosition = new Vector2(UiPosX, UiPosY),
			};
		}
	}
}