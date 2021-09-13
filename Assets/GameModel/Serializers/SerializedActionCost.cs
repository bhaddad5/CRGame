using System;
using System.Collections;
using System.Collections.Generic;
using Assets.GameModel;
using UnityEngine;

namespace GameModel.Serializers
{
	[Serializable]
	public struct SerializedActionCost
	{
		public float EgoCost;
		public float MoneyCost;
		public float SpreadsheetsCost;
		public float CultureCost;
		public float PatentsCost;
		public float BrandCost;
		public float RevanueCost;
		public int HornicalCost;

		public static SerializedActionCost Serialize(ActionCost ob)
		{
			return new SerializedActionCost()
			{
				EgoCost = ob.EgoCost,
				MoneyCost = ob.MoneyCost,
				SpreadsheetsCost = ob.SpreadsheetsCost,
				CultureCost = ob.CultureCost,
				BrandCost = ob.BrandCost,
				RevanueCost = ob.RevanueCost,
				PatentsCost = ob.PatentsCost,
				HornicalCost = ob.HornicalCost,
			};
		}

		public static ActionCost Deserialize(SerializedActionCost ob)
		{
			var res = new ActionCost()
			{
				EgoCost = ob.EgoCost,
				MoneyCost = ob.MoneyCost,
				SpreadsheetsCost = ob.SpreadsheetsCost,
				CultureCost = ob.CultureCost,
				BrandCost = ob.BrandCost,
				RevanueCost = ob.RevanueCost,
				PatentsCost = ob.PatentsCost,
				HornicalCost = ob.HornicalCost,
			};

			return res;
		}

		public static ActionCost ResolveReferences(DeserializedDataAccessor deserializer, ActionCost data, SerializedActionCost ob)
		{
			
			return data;
		}
	}
}