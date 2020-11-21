﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Assets.GameModel.XmlParsers
{
	public class InteractionXml
	{
		[XmlAttribute] [DefaultValue("")] public string Id = "";
		[XmlAttribute] [DefaultValue("")] public string Name = "";

		[XmlAttribute] [DefaultValue("")] public string Category = "";

		[XmlAttribute] [DefaultValue(1)] public int TurnCost = 1;
		[XmlAttribute] [DefaultValue(0)] public float EgoCost = 0;
		[XmlAttribute] [DefaultValue(0)] public float MoneyCost = 0;

		[XmlAttribute] [DefaultValue("")] public string RequiredInteractions = "";
		[XmlAttribute] [DefaultValue("")] public string RequiredPolicies = "";
		[XmlAttribute] [DefaultValue(false)] public bool RequiredControl = false;
		[XmlAttribute] [DefaultValue("")] public string RequiredDepartmentsControled = "";
		[XmlAttribute] [DefaultValue(-1)] public float RequiredAmbition = -1;
		[XmlAttribute] [DefaultValue(-1)] public float RequiredPride = -1;

		[XmlAttribute] [DefaultValue(false)] public bool Repeatable = false;
		[XmlAttribute] [DefaultValue(false)] public bool Completed = false;
		
		[XmlElement("InteractionResult", typeof(InteractionResultXml))]
		public InteractionResultXml[] InteractionResults = new InteractionResultXml[0];

		public Interaction FromXml()
		{
			List<InteractionResult> results = new List<InteractionResult>();
			foreach (var resultXml in InteractionResults)
			{
				results.Add(resultXml.FromXml());
			}

			return new Interaction()
			{
				Id = Id,
				Name = Name,
				TurnCost = TurnCost,
				EgoCost = EgoCost,
				MoneyCost = MoneyCost,
				RequiredInteractions = RequiredInteractions.XmlStringToList(),
				RequiredDepartmentsControled = RequiredDepartmentsControled.XmlStringToList(),
				RequiredPolicies = RequiredPolicies.XmlStringToList(),
				RequiredAmbition = RequiredAmbition,
				RequiredPride = RequiredPride,
				RequiredControl = RequiredControl,
				Repeatable = Repeatable,
				Completed = Completed,
				InteractionResults = results,
				Category = (Interaction.InteractionCategory)Enum.Parse(typeof(Interaction.InteractionCategory), Category)
			};
		}
	}
}
