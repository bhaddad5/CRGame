using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.GameModel
{
	[Serializable]
	public struct NpcEffect
	{
		[Header("Defaults to the interaction's NPC, if present")]
		public Npc OptionalNpcReference;
		public float AmbitionEffect;
		public float PrideEffect;
	}

	[Serializable]
	public struct Effect
	{
		public List<NpcEffect> NpcEffects;

		public float EgoEffect;
		public float FundsEffect;
		public float PowerEffect;
		public float PatentsEffect;
		public float CultureEffect;
		public float SpreadsheetsEffect;
		public float BrandEffect;
		public float RevanueEffect;
		public int HornicalEffect;
		public List<Npc> NpcsToControl;
		public List<Npc> NpcsToTrain;
		public List<Npc> NpcsToRemoveFromGame;
		public List<Location> LocationsToControl;
		public List<Trophy> TrophiesClaimedReferences;
		public List<Mission> MissionsToComplete;

		public int Car;
		public int Suits;

		public bool JewleryCuffs;
		public bool JewleryPen;
		public bool JewleryRing;
		public bool JewleryWatch;

		public int Promotion;
		public int Home;

		public void ExecuteEffect(MainGameManager mgm)
		{
			foreach (var effect in NpcEffects)
			{
				effect.OptionalNpcReference.Pride = Mathf.Max(effect.OptionalNpcReference.Pride + effect.PrideEffect, 0);
				effect.OptionalNpcReference.Ambition = Mathf.Max(effect.OptionalNpcReference.Ambition + effect.AmbitionEffect, 0);

			}

			mgm.Data.Ego = Mathf.Max(mgm.Data.Ego + EgoEffect, 0);
			mgm.Data.Funds = Mathf.Max(mgm.Data.Funds + FundsEffect, 0);
			mgm.Data.Power = Mathf.Max(mgm.Data.Power + PowerEffect, 0);
			mgm.Data.Patents = Mathf.Max(mgm.Data.Patents + PatentsEffect, 0);
			mgm.Data.CorporateCulture = Mathf.Max(mgm.Data.CorporateCulture + CultureEffect, 0);
			mgm.Data.Spreadsheets = Mathf.Max(mgm.Data.Spreadsheets + SpreadsheetsEffect, 0);
			mgm.Data.Brand = Mathf.Max(mgm.Data.Brand + BrandEffect, 0);
			mgm.Data.Revenue = Mathf.Max(mgm.Data.Revenue + RevanueEffect, 0);
			mgm.Data.Hornical = Mathf.Max(mgm.Data.Hornical + HornicalEffect, 0);

			foreach (var trophy in TrophiesClaimedReferences)
			{
				trophy.Owned = true;
			}

			foreach (var controlledNpc in NpcsToControl)
			{
				controlledNpc.Controlled = true;
			}

			foreach (var trainedNpc in NpcsToTrain)
			{
				trainedNpc.Trained = true;
			}

			foreach (var removedNpc in NpcsToRemoveFromGame)
			{
				removedNpc.Exists = false;
			}

			foreach (var location in LocationsToControl)
			{
				location.Controlled = true;
			}

			foreach (var mission in MissionsToComplete)
			{
				if (mission.Completed)
				{
					Debug.LogError($"Trying to complete mission: {mission.MissionName} when it is already complete!");
					continue;
				}

				mission.Completed = true;
				mission.Effect.ExecuteEffect(mgm);
			}

			if (Car > 0)
				mgm.Data.Car = Car;
			if (Suits > 0)
				mgm.Data.Suits = Suits;

			if(JewleryCuffs)
				mgm.Data.JewleryCuffs = true;
			if(JewleryPen)
				mgm.Data.JewleryPen = true;
			if(JewleryRing)
				mgm.Data.JewleryRing = true;
			if(JewleryWatch)
				mgm.Data.JewleryWatch = true;

			if (Promotion > 0)
				mgm.Data.Promotion = Promotion;

			if(Home > 0)
				mgm.Data.Home = Home;
		}
	}
}
