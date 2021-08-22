using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.GameModel
{
	public class ActionCost : ScriptableObject
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
	}
}