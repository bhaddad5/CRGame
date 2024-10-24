﻿using System;
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

		public List<InventoryItem> Items;

		public bool IsFree()
		{
			return EgoCost == 0 && MoneyCost == 0 && SpreadsheetsCost == 0 && CultureCost == 0 && PatentsCost == 0 &&
			       BrandCost == 0 && RevanueCost == 0 && Items.Count == 0;
		}

		public bool CanAffordCost(MainGameManager mgm)
		{
			foreach (var item in Items)
			{
				var numRequired = Items.Count(i => i == item);
				var numInInventory = mgm.Data.GetInventoryItemCount(item);

				if (numRequired > numInInventory)
					return false;
			}

			return EgoCost <= mgm.Data.Ego &&
			       MoneyCost <= mgm.Data.Funds &&
			       CultureCost <= mgm.Data.CorporateCulture &&
			       BrandCost <= mgm.Data.Brand &&
			       SpreadsheetsCost <= mgm.Data.Spreadsheets &&
			       RevanueCost <= mgm.Data.Revenue &&
			       PatentsCost <= mgm.Data.Patents;
		}

		public void SubtractCost(MainGameManager mgm)
		{
			mgm.Data.Ego -= EgoCost;
			mgm.Data.Funds -= MoneyCost;
			mgm.Data.CorporateCulture -= CultureCost;
			mgm.Data.Brand -= BrandCost;
			mgm.Data.Spreadsheets -= SpreadsheetsCost;
			mgm.Data.Revenue -= RevanueCost;
			mgm.Data.Patents -= PatentsCost;

			foreach (var item in Items)
			{
				mgm.Data.RemoveItemFromInventory(item);
			}
		}
	}
}