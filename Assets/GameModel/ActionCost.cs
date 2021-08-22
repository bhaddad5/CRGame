using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.GameModel
{
	[Serializable]
	public struct ActionCost
	{
		public float EgoCost;
		public float MoneyCost;
		public float SpreadsheetsCost;
		public float CultureCost;
		public float PatentsCost;
		public float BrandCost;
		public float RevanueCost;
		public int HornicalCost;

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