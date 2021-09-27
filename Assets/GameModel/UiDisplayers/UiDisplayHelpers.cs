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

		public static string GetInvalidTooltip(this ActionCost cost, MainGameManager mgm)
		{
			List<string> tooltips = new List<string>();
			if (cost.EgoCost > mgm.Data.Ego)
				tooltips.Add($"{cost.EgoCost} Ego");
			if (cost.MoneyCost > mgm.Data.Funds)
				tooltips.Add($"${cost.MoneyCost}");
			if (cost.CultureCost > mgm.Data.CorporateCulture)
				tooltips.Add($"{cost.CultureCost} Corporate Culture");
			if (cost.BrandCost > mgm.Data.Brand)
				tooltips.Add($"{cost.BrandCost} Brand");
			if (cost.SpreadsheetsCost > mgm.Data.Spreadsheets)
				tooltips.Add($"{cost.SpreadsheetsCost} Spreadsheets");
			if (cost.RevanueCost > mgm.Data.Revenue)
				tooltips.Add($"{cost.RevanueCost} Revenue");
			if (cost.PatentsCost > mgm.Data.Patents)
				tooltips.Add($"{cost.PatentsCost} Patents");
			if (cost.HornicalCost > mgm.Data.Hornical)
				tooltips.Add($"{cost.HornicalCost} Hornical");

			string finalTooltip = "";
			foreach (var tt in tooltips)
			{
				finalTooltip += $", {tt}";
			}

			if (finalTooltip.Length > 0)
			{
				finalTooltip = finalTooltip.Substring(2);
				finalTooltip = $"Requires {finalTooltip}";
			}

			return finalTooltip;
		}

		public static string GetInvalidTooltip(this ActionRequirements req, MainGameManager mgm, Npc npc)
		{
			List<string> tooltips = new List<string>();
			if (req.RequiredAmbition >= 0 && req.RequiredAmbition < npc.Ambition)
				tooltips.Add($"{req.RequiredAmbition} or less Ambition");
			if (req.RequiredPride >= 0 && req.RequiredPride < npc.Pride)
				tooltips.Add($"{req.RequiredPride} or less Pride");
			if (req.RequiredPower > mgm.Data.Power)
				tooltips.Add($"{req.RequiredPower} or more Power");

			foreach (var requiredPolicy in req.RequiredPolicies)
			{
				tooltips.Add($"{requiredPolicy.Name}");
			}

			string finalTooltip = "";
			foreach (var tt in tooltips)
			{
				finalTooltip += $", {tt}";
			}

			if (finalTooltip.Length > 0)
			{
				finalTooltip = finalTooltip.Substring(2);
				finalTooltip = $"Requires {finalTooltip}";
			}

			return finalTooltip;
		}

		public static Sprite ToSprite(this Texture2D tex)
		{
			if (tex == null)
				return null;
			return Sprite.Create(tex, new Rect(Vector2.zero, new Vector2(tex.width, tex.height)), Vector2.zero);
		}
	}
}