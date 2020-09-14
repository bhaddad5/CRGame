using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.GameModel
{
	public class Interaction
	{
		public string Id;
		public string Name;
		public string Dialog;

		public int TurnCost = 1;
		public float EgoCost = 0;
		public float MoneyCost = 0;

		public List<string> RequiredPolicies = new List<string>();
		public bool RequiredControl = false;
		public float RequiredAmbition = -1;
		public float RequiredPride = -1;

		public float AmbitionEffect = 0;
		public float PrideEffect = 0;
		public float EgoEffect = 0;
		public bool ControlEffect = false;

		public bool InteractionValid(MainGameManager mgm, Fem fem)
		{
			if (RequiredControl && !fem.Controlled)
				return false;
			if (RequiredAmbition >= 0 && RequiredAmbition < fem.Ambition)
				return false;
			if (RequiredPride >= 0 && RequiredPride < fem.Pride)
				return false;
			foreach (var policy in RequiredPolicies)
			{
				if (!mgm.ActivePolicies.Contains(policy))
					return false;
			}
			return TurnCost <= mgm.RemainingTurnActions && EgoCost <= mgm.Ego && MoneyCost <= mgm.Funds;
		}

		public void ExecuteInteraction(MainGameManager mgm, Fem fem)
		{
			mgm.RemainingTurnActions -= TurnCost;
			mgm.Ego -= EgoCost;
			mgm.Funds -= MoneyCost;

			fem.Pride = Mathf.Max(fem.Pride + PrideEffect, 0);
			fem.Ambition = Mathf.Max(fem.Ambition + AmbitionEffect, 0);
			mgm.Ego = Mathf.Max(mgm.Ego + EgoEffect, 0);
			fem.Controlled = fem.Controlled || ControlEffect;
		}
	}
}
