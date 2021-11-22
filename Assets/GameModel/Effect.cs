using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.GameModel
{
	[Serializable]
	public struct Effect
	{
		public Npc ContextualNpcReference;
		public float AmbitionEffect;
		public float PrideEffect;
		public float EgoEffect;
		public float FundsEffect;
		public float PowerEffect;
		public float PatentsEffect;
		public float CultureEffect;
		public float SpreadsheetsEffect;
		public float BrandEffect;
		public float RevanueEffect;
		public int HornicalEffect;
		public bool ControlEffect;
		public bool RemoveNpcFromGame;
		public List<Trophy> TrophiesClaimedReferences;

		public Location ContextualLocationReference;
		public Texture2D UpdateLocationBackground;
		public bool ShouldUpdateLocationMapPos;
		public Vector2 UpdateLocationMapPosition;

		public int Car;
		public int Suits;

		public bool JewleryCuffs;
		public bool JewleryPen;
		public bool JewleryRing;
		public bool JewleryWatch;

		public void ExecuteEffect(MainGameManager mgm, Npc npc)
		{
			npc = ContextualNpcReference ?? npc;

			if (npc != null)
			{
				if (RemoveNpcFromGame)
				{
					var loc = mgm.Data.FindNpcLocation(npc);
					loc.Npcs.Remove(npc);
					mgm.Data.DeadNpcPool.Npcs.Add(npc);
				}

				mgm.Data.Ego = Mathf.Max(mgm.Data.Ego + EgoEffect, 0);
				mgm.Data.Funds = Mathf.Max(mgm.Data.Funds + FundsEffect, 0);

				npc.Pride = Mathf.Max(npc.Pride + PrideEffect, 0);
				npc.Ambition = Mathf.Max(npc.Ambition + AmbitionEffect, 0);
				npc.Controlled = npc.Controlled || ControlEffect;
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
			}

			if (ContextualLocationReference != null)
			{
				if (UpdateLocationBackground != null)
					ContextualLocationReference.BackgroundImage = UpdateLocationBackground;
				if (UpdateLocationMapPosition.x >= 0 && UpdateLocationMapPosition.y >= 0)
					ContextualLocationReference.UiPosition = UpdateLocationMapPosition;
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
		}
	}
}
