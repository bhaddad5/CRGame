using System.Collections;
using System.Collections.Generic;
using Assets.GameModel;
using UnityEngine;

namespace Assets.GameModel.UiDisplayers
{
	public static class UiDisplayHelpers
	{
		public static string GetCostString(this ActionCost cost)
		{
			string str = "";
			if (cost.EgoCost > 0)
				str += $"-{cost.EgoCost} Ego, ";
			if (cost.MoneyCost > 0)
				str += $"-${cost.MoneyCost}, ";
			if (cost.CultureCost > 0)
				str += $"-{cost.CultureCost} Culture, ";
			if (cost.BrandCost > 0)
				str += $"-{cost.BrandCost} Brand, ";
			if (cost.SpreadsheetsCost > 0)
				str += $"-{cost.SpreadsheetsCost} Spreadsheets, ";
			if (cost.RevanueCost > 0)
				str += $"-{cost.RevanueCost} Revenue, ";
			if (cost.PatentsCost > 0)
				str += $"-{cost.PatentsCost} Patents, ";
			if (cost.HornicalCost > 0)
				str += $"-{cost.HornicalCost} Hornical, ";

			if (str.EndsWith(", "))
				str = str.Substring(0, str.Length - 2);
			return str;
		}

		public static string GetEffectsString(this List<Effect> effects)
		{
			string str = "";
			foreach (var effect in effects)
			{
				if (effect.PowerEffect > 0)
					str += $"+{effect.PowerEffect} Power, ";
				if (effect.EgoEffect > 0)
					str += $"+{effect.EgoEffect} Ego, ";
				if (effect.BrandEffect > 0)
					str += $"+{effect.BrandEffect} Brand, ";
				if (effect.HornicalEffect > 0)
					str += $"+{effect.HornicalEffect} Hornical, ";
				if (effect.AmbitionEffect != 0)
					str += $"{effect.AmbitionEffect} Ambition, ";
				if (effect.PrideEffect != 0)
					str += $"{effect.PrideEffect} Pride, ";
			}

			if (str.EndsWith(", "))
				str = str.Substring(0, str.Length - 2);

			return str;
		}

		public static List<string> GetInvalidTooltips(this ActionCost cost, MainGameManager mgm)
		{
			List<string> tooltips = new List<string>();
			if (cost.EgoCost > mgm.Data.Ego)
				tooltips.Add($"Requires {cost.EgoCost} Ego");
			if (cost.MoneyCost > mgm.Data.Funds)
				tooltips.Add($"Requires ${cost.MoneyCost}");
			if (cost.CultureCost > mgm.Data.CorporateCulture)
				tooltips.Add($"Requires {cost.CultureCost} Corporate Culture");
			if (cost.BrandCost > mgm.Data.Brand)
				tooltips.Add($"Requires {cost.BrandCost} Brand");
			if (cost.SpreadsheetsCost > mgm.Data.Spreadsheets)
				tooltips.Add($"Requires {cost.SpreadsheetsCost} Spreadsheets");
			if (cost.RevanueCost > mgm.Data.Revenue)
				tooltips.Add($"Requires {cost.RevanueCost} Revenue");
			if (cost.PatentsCost > mgm.Data.Patents)
				tooltips.Add($"Requires {cost.PatentsCost} Patents");
			if (cost.HornicalCost > mgm.Data.Hornical)
				tooltips.Add($"Requires {cost.HornicalCost} Hornical");

			return tooltips;
		}
	}
}