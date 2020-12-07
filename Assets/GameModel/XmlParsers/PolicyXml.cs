using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Xml.Serialization;
using UnityEngine;

namespace Assets.GameModel.XmlParsers
{
	public class PolicyXml
	{
		[XmlAttribute] [DefaultValue("")] public string Id = "";
		[XmlAttribute] [DefaultValue("")] public string Name = "";
		[XmlAttribute] [DefaultValue(false)] public bool Active = false;

		[XmlElement("Effect", typeof(Effect))]
		public Effect[] PerTurnEffect = new Effect[0];

		public Policy FromXml()
		{
			return new Policy()
			{
				Id = Id,
				Name = Name,
				Active = Active,
			};
		}

		public static PolicyXml ToXml(Policy ob)
		{
			return new PolicyXml()
			{
				Id = ob.Id,
				Name = ob.Name,
				Active = ob.Active,
			};
		}
	}
}