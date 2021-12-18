using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.GameModel
{
	[Serializable]
	public class GameData : ScriptableObject
	{
		public string PlayerName = "";
		public int TurnNumber = 0;
		public float Ego = 10;
		public float Funds = 0;
		public float Power = 0;
		public float Patents = 0;
		public float CorporateCulture = 0;
		public float Spreadsheets = 0;
		public float Brand = 0;
		public float Revenue = 0;
		public int Hornical = 0;

		public int Promotion = 0;

		//Status Symbols
		public int Car;
		public int Suits;
		public bool JewleryCuffs;
		public bool JewleryPen;
		public bool JewleryRing;
		public bool JewleryWatch;

		public List<Promotion> PlayerPromotionLevels = new List<Promotion>();

		public Location MyOffice;
		public List<Location> Locations = new List<Location>();

		public List<Interaction> StartOfTurnInteractions = new List<Interaction>();
		
		public List<Trophy> GetOwnedTrophies()
		{
			List<Trophy> res = new List<Trophy>();
			foreach (var department in Locations)
			{
				if (department == null)
					continue;
				foreach (var npc in department.Npcs)
				{
					if (npc == null)
						continue;
					foreach (var trophy in npc.Trophies)
					{
						if (trophy == null)
							continue;
						if (trophy.Owned)
							res.Add(trophy);
					}
				}
			}

			return res;
		}
	}
}