using System;
using System.Collections.Generic;
using System.Linq;

namespace Assets.GameModel
{
	public class Interaction
	{
		public string Id;
		public string Name;

		public int TurnCost = 1;
		public float EgoCost = 0;
		public float MoneyCost = 0;

		public List<string> RequiredPolicies = new List<string>();
		public bool RequiredControl = false;
		public float RequiredAmbition = -1;
		public float RequiredPride = -1;

		public List<InteractionResult> InteractionResults;

		public bool InteractionValid(MainGameManager mgm, Fem fem)
		{
			if (RequiredControl && !fem.Controlled)
				return false;
			if (RequiredAmbition >= 0 && RequiredAmbition < fem.Ambition)
				return false;
			if (RequiredPride >= 0 && RequiredPride < fem.Pride)
				return false;
			if (InteractionResults.Any(res => res.Effects.Any(eff => eff.ControlEffect)) && fem.Controlled)
				return false;
			foreach (var policyId in RequiredPolicies)
			{
				if (!mgm.Data.GetActivePolicyIds().Contains(policyId))
					return false;
			}
			return TurnCost <= mgm.Data.Actions && EgoCost <= mgm.Data.Ego && MoneyCost <= mgm.Data.Funds;
		}

		public InteractionResult ExecuteInteraction(MainGameManager mgm, Fem fem)
		{
			mgm.Data.Actions -= TurnCost;
			mgm.Data.Ego -= EgoCost;
			mgm.Data.Funds -= MoneyCost;

			var chosenResult = InteractionResults[UnityEngine.Random.Range(0, InteractionResults.Count)];
			chosenResult.Execute(mgm, fem);
			return chosenResult;
		}
	}
}
