using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Assets.GameModel
{
	[CreateAssetMenu(fileName = "New Interaction", menuName = "Company Man Data/Interaction", order = 2)]
	[Serializable]
	public class Interaction : ScriptableObject
	{
		[HideInInspector]
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
		[Header("Ensure this is set to 0!")]
		public int Completed = 0;

		public bool PreviewEffect = false;

		public InteractionResult Result;

		[Header("")]
		[Header("")]
		[Header("RANDOM FAILURE OPTION")]
		public bool CanFail = false;
		[Header("(Between 0 and 1)")]
		public float ProbabilityOfFailureResult = 0;
		public InteractionResult FailureResult;

		public bool InteractionVisible(MainGameManager mgm, Npc npc)
		{
			if (Completed > 0 && !Repeatable)
				return false;

			return Requirements.RequirementsAreMet(mgm, npc);
		}

		public bool InteractionValid(MainGameManager mgm, Npc contextualNpc = null)
		{
			if (!InteractionVisible(mgm, contextualNpc))
				return false;

			return Cost.CanAffordCost(mgm);
		}

		public bool GetInteractionSucceeded()
		{
			UnityEngine.Random.InitState(System.DateTime.Now.Millisecond);

			return !CanFail || Random.Range(0f, 1f) >= ProbabilityOfFailureResult;
		}

		public InteractionResult GetInteractionResult(bool succeeded)
		{
			return succeeded ? Result : FailureResult;
		}
	}
}