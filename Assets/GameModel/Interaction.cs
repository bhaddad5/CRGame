﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace Assets.GameModel
{
	public class Interaction
	{
		public string Id;
		public string Name;

		public enum InteractionCategory
		{
			OfficePolitics,
			Conversation,
			Challenge,
			Socialize,
			Projects,
			Train,
			Fun,
			Surveillance,
		}

		public InteractionCategory Category;

		public ActionRequirements Requirements;
		public ActionCost Cost;

		public bool Repeatable = false;
		public bool Completed = false;

		public List<InteractionResult> InteractionResults;

		public bool InteractionVisible(MainGameManager mgm, Fem fem)
		{
			if (Completed && !Repeatable)
				return false;
			if (fem.Controlled && InteractionResults.Any(res => res.Effects.Any(eff => eff.ControlEffect)))
				return false;

			return Requirements.RequirementsAreMet(mgm, fem);
		}

		public bool InteractionValid(MainGameManager mgm, Fem fem)
		{
			if (!InteractionVisible(mgm, fem))
				return false;

			return Cost.CanAffordCost(mgm);
		}

		public InteractionResult GetInteractionResult(MainGameManager mgm, Fem fem)
		{
			Cost.SubtractCost(mgm);

			Completed = true;

			var chosenResult = InteractionResults[UnityEngine.Random.Range(0, InteractionResults.Count)];
			return chosenResult;
		}
	}
}