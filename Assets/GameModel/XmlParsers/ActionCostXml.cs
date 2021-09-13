﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using GameModel.Serializers;

namespace Assets.GameModel.XmlParsers
{
	public class ActionCostXml
	{
		[XmlAttribute] [DefaultValue(1)] public int TurnCost = 1;
		[XmlAttribute] [DefaultValue(0)] public float EgoCost = 0;
		[XmlAttribute] [DefaultValue(0)] public float MoneyCost = 0;
		[XmlAttribute] [DefaultValue(0)] public float SpreadsheetsCost = 0;
		[XmlAttribute] [DefaultValue(0)] public float CultureCost = 0;
		[XmlAttribute] [DefaultValue(0)] public float BrandCost = 0;
		[XmlAttribute] [DefaultValue(0)] public float RevanueCost = 0;
		[XmlAttribute] [DefaultValue(0)] public float PatentsCost = 0;
		[XmlAttribute] [DefaultValue(0)] public int HornicalCost = 0;

		public SerializedActionCost FromXml()
		{
			return new SerializedActionCost()
			{
				EgoCost = EgoCost,
				MoneyCost = MoneyCost,
				SpreadsheetsCost = SpreadsheetsCost,
				CultureCost = CultureCost,
				BrandCost = BrandCost,
				RevanueCost = RevanueCost,
				PatentsCost = PatentsCost,
				HornicalCost = HornicalCost,
			};
		}
	}
}