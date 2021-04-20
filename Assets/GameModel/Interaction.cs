using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

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

		public bool InteractionVisible(MainGameManager mgm, Npc npc)
		{
			if (Completed && !Repeatable)
				return false;
			if (npc.Controlled && InteractionResults.Any(res => res.Effects.Any(eff => eff.ControlEffect)))
				return false;

			return Requirements.RequirementsAreMet(mgm, npc);
		}

		public bool InteractionValid(MainGameManager mgm, Npc npc)
		{
			if (!InteractionVisible(mgm, npc))
				return false;

			return Cost.CanAffordCost(mgm);
		}

		public InteractionResult GetInteractionResult(MainGameManager mgm, Npc npc)
		{
			Cost.SubtractCost(mgm);

			Completed = true;
			var randValue = UnityEngine.Random.Range(0, InteractionResults.Count);
			var chosenResult = InteractionResults[randValue];
			return chosenResult;
		}

		public List<Mission> GetRelevantMissions(MainGameManager mgm, Npc npc)
		{
			return mgm.Data.GetMissionsForInteraction(npc.Id, Id);
		}

		public void ExecuteMissionIfRelevant(MainGameManager mgm, Npc npc)
		{
			var missions = GetRelevantMissions(mgm, npc);
			foreach (var mission in missions)
			{
				foreach (var effect in mission.Rewards)
				{
					effect.ExecuteEffect(mgm, npc);
				}
			}
		}
	}
}