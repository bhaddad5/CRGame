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
		[HideInInspector] public int Hornical = 0;

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

		public Location MyOffice;
		public Location MyHome;

		public List<Location> Locations = new List<Location>();
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
			Hornical = 0;

			Promotion = 0;
			Home = 0;

			Car = 0;
			Suits = 0;
			JewleryCuffs = false;
			JewleryPen = false;
			JewleryRing = false;
			JewleryWatch = false;

			foreach (var ob in Locations)
				ob.Setup(mgm);

			foreach (var ob in StartOfTurnInteractions)
				ob.Setup();
		}
		
		public List<Trophy> GetOwnedTrophies()
		{
			List<Trophy> res = new List<Trophy>();
			foreach (var department in Locations)
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

			return res;
		}
	}
}