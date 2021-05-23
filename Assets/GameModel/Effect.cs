using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.GameModel
{
	public class Effect
	{
		public string ContextualNpcId = "";
		public float AmbitionEffect = 0;
		public float PrideEffect = 0;
		public float EgoEffect = 0;
		public float FundsEffect = 0;
		public float PowerEffect = 0;
		public float PatentsEffect = 0;
		public float CultureEffect = 0;
		public float SpreadsheetsEffect = 0;
		public float BrandEffect = 0;
		public float RevanueEffect = 0;
		public int HornicalEffect = 0;
		public bool ControlEffect = false;
		public bool RemoveNpcFromGame = false;
		public List<string> TraitsAdded;
		public List<string> TraitsRemoved;
		public List<string> TrophiesClaimed;

		public string ContextualLocationId;
		public Sprite UpdateLocationBackground;
		public Vector2 UpdateLocationMapPosition;

		public PlayerStatusSymbols UpdateStatusSymbols;

		public void ExecuteEffect(MainGameManager mgm, Npc npc)
		{
			if (ContextualNpcId != "")
				npc = mgm.Data.GetNpcById(ContextualNpcId);

			if (npc != null)
			{
				if (RemoveNpcFromGame)
				{
					var loc = mgm.Data.FindNpcLocation(npc);
					loc.Npcs.Remove(npc);
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

				foreach (var trophyId in TrophiesClaimed)
				{
					var trophy = npc.Trophies.FirstOrDefault(t => t.Id == trophyId);
					if (trophy == null)
					{
						Debug.LogError($"Cannot find trophy with id {trophyId} in npc {npc.Id} to claim!");
						continue;
					}
					trophy.Owned = true;
				}
			}

			Location contextualLocation = mgm.Data.GetLocationById(ContextualLocationId);
			if (contextualLocation != null)
			{
				if (UpdateLocationBackground != null)
					contextualLocation.BackgroundImage = UpdateLocationBackground;
				if (UpdateLocationMapPosition.x >= 0 && UpdateLocationMapPosition.y >= 0)
					contextualLocation.UiPosition = UpdateLocationMapPosition;
			}

			if (UpdateStatusSymbols != null)
			{
				if(UpdateStatusSymbols.CarLayout != null)
					mgm.Data.StatusSymbols.CarLayout = UpdateStatusSymbols.CarLayout;
				if (!String.IsNullOrEmpty(UpdateStatusSymbols.CarName))
					mgm.Data.StatusSymbols.CarName = UpdateStatusSymbols.CarName;
				if (UpdateStatusSymbols.CarImage != null)
					mgm.Data.StatusSymbols.CarImage = UpdateStatusSymbols.CarImage;

				if (UpdateStatusSymbols.SuitsLocation != null)
					mgm.Data.StatusSymbols.SuitsLocation = UpdateStatusSymbols.SuitsLocation;
				if (!String.IsNullOrEmpty(UpdateStatusSymbols.SuitsName))
					mgm.Data.StatusSymbols.SuitsName = UpdateStatusSymbols.SuitsName;
				if (UpdateStatusSymbols.SuitsImage != null)
					mgm.Data.StatusSymbols.SuitsImage = UpdateStatusSymbols.SuitsImage;
			}

			
		}
	}
}
