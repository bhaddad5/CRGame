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

		public bool PreviewEffect = false;

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

			int totalProbability = 0;
			foreach (var result in InteractionResults)
			{
				totalProbability += result.Probability;
			}
			UnityEngine.Random.InitState(System.DateTime.Now.Millisecond);
			var randValue = UnityEngine.Random.Range(0, totalProbability);
			Debug.Log(randValue + ", " + totalProbability);
			int checkedProbability = 0;
			foreach (var result in InteractionResults)
			{
				checkedProbability += result.Probability;
				if (randValue < checkedProbability)
					return result;
			}

			throw new Exception($"Random probabiltiy result not found for {Name}, result = {randValue}");
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

		public string GetBenefitsString()
		{
			if(InteractionResults.Count > 0)
				Debug.Log("You should not preview results for interactions with multiple outcomes!!!");

			var res = InteractionResults[0];

			string str = "";
			foreach (var effect in res.Effects)
			{
				if (effect.PowerEffect > 0)
					str += $"+{effect.PowerEffect} Power, ";
				if (effect.EgoEffect > 0)
					str += $"+{effect.EgoEffect} Ego, ";
				if (effect.BrandEffect > 0)
					str += $"+{effect.BrandEffect} Brand, ";
				if (effect.HornicalEffect > 0)
					str += $"+{effect.HornicalEffect} Hornical, ";
				if (effect.AmbitionEffect != 0)
					str += $"{effect.AmbitionEffect} Ambition, ";
				if (effect.PrideEffect != 0)
					str += $"{effect.PrideEffect} Pride, ";
			}

			if (str.EndsWith(", "))
				str = str.Substring(0, str.Length - 2);
			return str;
		}
	}
}