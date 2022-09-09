using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.GameModel
{
	[Serializable]
	public class GameData : ScriptableObject
	{
		public string FirstName = "";
		public string LastName = "";
		public float StartingEgo = 0;
		public float StartingFunds = 0;

		[HideInInspector] public int TurnNumber = 0;
		[HideInInspector] public float Ego = 0;
		[HideInInspector] public float Funds = 0;
		[HideInInspector] public float Power = 0;
		[HideInInspector] public float Patents = 0;
		[HideInInspector] public float CorporateCulture = 0;
		[HideInInspector] public float Spreadsheets = 0;
		[HideInInspector] public float Brand = 0;
		[HideInInspector] public float Revenue = 0;

		[HideInInspector] public Dictionary<InventoryItem, int> Inventory = new Dictionary<InventoryItem, int>();

		//LEGACY
		[HideInInspector] public int Hornical = 0;
		//END LEGACY

		[HideInInspector] public int Promotion = 0;
		[HideInInspector] public int Home = 0;

		//Status Symbols
		[HideInInspector] public int Car;
		[HideInInspector] public int Suits;
		[HideInInspector] public bool JewleryCuffs;
		[HideInInspector] public bool JewleryPen;
		[HideInInspector] public bool JewleryRing;
		[HideInInspector] public bool JewleryWatch;

		public List<Promotion> PlayerPromotionLevels = new List<Promotion>();
		public List<Home> PlayerHomeLevels = new List<Home>();

		public List<InventoryItem> InventoryItemOptions = new List<InventoryItem>();
		public List<Region> Regions = new List<Region>();

		public List<Interaction> StartOfTurnInteractions = new List<Interaction>();

		public void Setup(MainGameManager mgm)
		{
			TurnNumber = 0;
			Ego = StartingEgo;
			Funds = StartingFunds;

			Power = 0;
			Patents = 0;
			CorporateCulture = 0;
			Spreadsheets = 0;
			Brand = 0;
			Revenue = 0;
			
			Inventory = new Dictionary<InventoryItem, int>();

			Promotion = 0;
			Home = 0;

			Car = 0;
			Suits = 0;
			JewleryCuffs = false;
			JewleryPen = false;
			JewleryRing = false;
			JewleryWatch = false;

			foreach (var ob in Regions)
				ob.Setup(mgm);

			foreach (var ob in StartOfTurnInteractions)
				ob.Setup();

			//LEGACY
			Hornical = 0;
			//END LEGACY
		}
		
		public List<Trophy> GetOwnedTrophies()
		{
			List<Trophy> res = new List<Trophy>();
			foreach (var region in Regions)
			{
				foreach (var department in region.Locations)
				{
					foreach (var npc in department.Npcs)
					{
						foreach (var trophy in npc.Trophies)
						{
							if (trophy.Owned)
								res.Add(trophy);
						}
					}
				}
			}

			return res;
		}

		public int GetInventoryItemCount(InventoryItem item)
		{
			if (!Inventory.ContainsKey(item))
				return 0;
			return Inventory[item];
		}

		public void AddItemToInventory(InventoryItem item)
		{
			if (!Inventory.ContainsKey(item))
				Inventory[item] = 0;
			Inventory[item] = Inventory[item] + 1;
		}

		public void RemoveItemFromInventory(InventoryItem item)
		{
			if (!Inventory.ContainsKey(item) || Inventory[item] == 0)
				throw new Exception("Trying to remove an inventory item you don't possess!!!");

			Inventory[item] = Inventory[item] - 1;
		}
	}
}