using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Random = System.Random;

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

		public List<Effect> Effects;

		public bool InteractionValid(MainGameManager mgm, Fem fem)
		{
			if (RequiredControl && !fem.Controlled)
				return false;
			if (RequiredAmbition >= 0 && RequiredAmbition < fem.Ambition)
				return false;
			if (RequiredPride >= 0 && RequiredPride < fem.Pride)
				return false;
			if (Effects.Any(eff => eff.ControlEffect) && fem.Controlled)
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

			var chosenEffect = Effects[UnityEngine.Random.Range(0, Effects.Count - 1)];
			chosenEffect.ExecuteEffect(mgm, fem);
		}
	}
}
