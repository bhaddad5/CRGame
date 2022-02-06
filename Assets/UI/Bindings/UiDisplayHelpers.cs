using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Assets.GameModel;
using UnityEngine;

namespace Assets.GameModel.UiDisplayers
{
	public static class UiDisplayHelpers
	{
		public static string ApplyDynamicValuesToString(string str, MainGameManager mgm)
		{
			if (str == null)
				return null;

			var res = str;
			res = res.Replace("{FirstName}", mgm.Data.FirstName);
			res = res.Replace("{LastName}", mgm.Data.LastName);

			return res;
		}

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

		public static string GetEffectsString(this Effect effect)
		{
			string str = "";
			if (effect.PowerEffect > 0)
				str += $"+{effect.PowerEffect} Power, ";
			if (effect.EgoEffect > 0)
				str += $"+{effect.EgoEffect} Ego, ";
			if (effect.BrandEffect > 0)
				str += $"+{effect.BrandEffect} Brand, ";
			if (effect.CultureEffect > 0)
				str += $"+{effect.CultureEffect} Culture, ";
			if (effect.SpreadsheetsEffect > 0)
				str += $"+{effect.SpreadsheetsEffect} Spreadsheets, ";
			if (effect.RevanueEffect > 0)
				str += $"+{effect.RevanueEffect} Revenue, ";
			if (effect.PatentsEffect > 0)
				str += $"+{effect.PatentsEffect} Patents, ";
			if (effect.HornicalEffect > 0)
				str += $"+{effect.HornicalEffect} Hornical";

			if (!String.IsNullOrEmpty(str))
				str = $"Player: {str}";

			if (str.EndsWith(", "))
				str = str.Substring(0, str.Length - 2);

			foreach (var npcEffect in effect.NpcEffects)
			{
				if (!String.IsNullOrEmpty(str))
					str += "\n";

				str += $"{npcEffect.OptionalNpcReference.FirstName} {npcEffect.OptionalNpcReference.LastName}: ";

				if (npcEffect.AmbitionEffect != 0)
					str += $"{npcEffect.AmbitionEffect} Ambition, ";
				if (npcEffect.PrideEffect != 0)
					str += $"{npcEffect.PrideEffect} Pride, ";

				if (str.EndsWith(", "))
					str = str.Substring(0, str.Length - 2);
			}

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

			return TooltipsToString(tooltips);
		}

		public static string GetInvalidTooltip(this ActionRequirements req, MainGameManager mgm)
		{
			List<string> tooltips = new List<string>();

			foreach (var department in req.RequiredDepartmentsControled)
			{
				if(!department.Controlled)
					tooltips.Add($"Control of {department.Name}");
			}

			foreach (var npc in req.RequiredNpcsControled)
			{
				if (!npc.Controlled)
					tooltips.Add($"Control of {npc.FirstName}");
			}

			foreach (var ob in req.RequiredNpcsTrained)
			{
				if (!ob.Trained)
					tooltips.Add($"{ob.FirstName} Trained");
			}

			foreach (var ob in req.RequiredTrophies)
			{
				if (!ob.Owned)
					tooltips.Add($"{ob.Name}");
			}

			foreach (var requiredPolicy in req.RequiredPolicies)
			{
				if (!requiredPolicy.Active)
					tooltips.Add($"{requiredPolicy.Name}");
			}

			foreach (var interaction in req.RequiredInteractions)
			{
				if(interaction.Completed == 0)
					tooltips.Add($"{interaction.Name}");
			}
			
			foreach (var npcReq in req.NpcStatRequirements)
			{
				if (npcReq.Stat == NpcStatRequirement.NpcStat.Ambition && !npcReq.CheckStat(npcReq.OptionalNpcReference.Ambition))
					tooltips.Add($"{npcReq.OptionalNpcReference.FirstName}: {npcReq.Value} or less Ambition");
				if (npcReq.Stat == NpcStatRequirement.NpcStat.Pride && !npcReq.CheckStat(npcReq.OptionalNpcReference.Pride))
					tooltips.Add($"{npcReq.OptionalNpcReference.FirstName}: {npcReq.Value} or less Pride");
			}
			
			if (req.RequiredPower > mgm.Data.Power)
				tooltips.Add($"{req.RequiredPower} or more Power");

			return TooltipsToString(tooltips);
		}

		private static string TooltipsToString(List<string> tooltips)
		{
			string finalTooltip = "";
			foreach (var tt in tooltips)
			{
				finalTooltip += $", {tt}";
			}

			if (finalTooltip.Length > 0)
			{
				finalTooltip = finalTooltip.Substring(2);
				finalTooltip = $"Requires: {finalTooltip}";
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