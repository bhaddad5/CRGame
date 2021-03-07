using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.GameModel
{
	public class ActionCost
	{
		public int TurnCost = 1;
		public float EgoCost = 0;
		public float MoneyCost = 0;
		public float SpreadsheetsCost = 0;
		public float CultureCost = 0;
		public float PatentsCost = 0;
		public float BrandCost = 0;
		public float RevanueCost = 0;
		public int HornicalCost = 0;

		public bool CanAffordCost(MainGameManager mgm)
		{
			return EgoCost <= mgm.Data.Ego &&
			       MoneyCost <= mgm.Data.Funds &&
			       CultureCost <= mgm.Data.CorporateCulture &&
			       BrandCost <= mgm.Data.Brand &&
			       SpreadsheetsCost <= mgm.Data.Spreadsheets &&
			       RevanueCost <= mgm.Data.Revenue &&
			       PatentsCost <= mgm.Data.Patents &&
			       HornicalCost <= mgm.Data.Hornical;
		}

		public void SubtractCost(MainGameManager mgm)
		{
			mgm.Data.Ego -= EgoCost;
			mgm.Data.Funds -= MoneyCost;
			mgm.Data.Hornical -= HornicalCost;
			mgm.Data.CorporateCulture -= CultureCost;
			mgm.Data.Brand -= BrandCost;
			mgm.Data.Spreadsheets -= SpreadsheetsCost;
			mgm.Data.Revenue -= RevanueCost;
			mgm.Data.Patents -= PatentsCost;
		}

		public List<string> GetInvalidTooltips(MainGameManager mgm)
		{
			List<string> tooltips = new List<string>();
			if (EgoCost > mgm.Data.Ego)
				tooltips.Add($"Requires {EgoCost} Ego");
			if (MoneyCost > mgm.Data.Funds)
				tooltips.Add($"Requires ${MoneyCost}");
			if (CultureCost > mgm.Data.CorporateCulture)
				tooltips.Add($"Requires {CultureCost} Corporate Culture");
			if (BrandCost > mgm.Data.Brand)
				tooltips.Add($"Requires {BrandCost} Brand");
			if (SpreadsheetsCost > mgm.Data.Spreadsheets)
				tooltips.Add($"Requires {SpreadsheetsCost} Spreadsheets");
			if (RevanueCost > mgm.Data.Revenue)
				tooltips.Add($"Requires {RevanueCost} Revenue");
			if (PatentsCost > mgm.Data.Patents)
				tooltips.Add($"Requires {PatentsCost} Patents");
			if (HornicalCost > mgm.Data.Hornical)
				tooltips.Add($"Requires {HornicalCost} Hornical");

			return tooltips;
		}

		public string GetCostString()
		{
			string str = "";
			if (EgoCost > 0)
				str += $"{EgoCost} Ego, ";
			if (MoneyCost > 0)
				str += $"${MoneyCost}, ";
			if (CultureCost > 0)
				str += $"{CultureCost} Culture, ";
			if (BrandCost > 0)
				str += $"{BrandCost} Brand, ";
			if (SpreadsheetsCost > 0)
				str += $"{SpreadsheetsCost} Spreadsheets, ";
			if (RevanueCost > 0)
				str += $"{RevanueCost} Revenue, ";
			if (PatentsCost > 0)
				str += $"{PatentsCost} Patents, ";
			if (HornicalCost > 0)
				str += $"{HornicalCost} Hornical, ";

			if (str.EndsWith(", "))
				str = str.Substring(0, str.Length - 2);
			return str;
		}
	}
}