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
			foreach (var policyId in RequiredPolicies)
			{
				if (!mgm.Data.GetActivePolicyIds().Contains(policyId))
					return false;
			}
			return TurnCost <= mgm.Data.Actions && EgoCost <= mgm.Data.Ego && MoneyCost <= mgm.Data.Funds;
		}

		public void ExecuteInteraction(MainGameManager mgm, Fem fem)
		{
			mgm.Data.Actions -= TurnCost;
			mgm.Data.Ego -= EgoCost;
			mgm.Data.Funds -= MoneyCost;

			var chosenEffect = Effects[UnityEngine.Random.Range(0, Effects.Count - 1)];
			chosenEffect.ExecuteEffect(mgm, fem);
		}
	}
}
