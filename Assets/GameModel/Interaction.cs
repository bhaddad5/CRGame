using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Assets.GameModel
{
	[Serializable]
	public class Interaction : ScriptableObject
	{
		[HideInInspector]
		public string Id;

		public string Name;

		public bool SubInteraction;
		public bool AlertNew = true;

		public enum InteractionCategory
		{
			Conversation = 1,
			OfficePolitics = 0,
			Socialize = 3,
			Challenge = 2,
			Projects = 4,
			Train = 5,
			Fun = 6,
			Surveillance = 7,
		}

		public InteractionCategory Category;

		public ActionRequirements Requirements;
		public ActionCost Cost;

		public bool Repeatable = false;
		public int Cooldown = 0;
		
		public bool PreviewEffect = false;

		public InteractionResult Result;

		[Header("")]
		[Header("")]
		[Header("RANDOM FAILURE OPTION")]
		public bool CanFail = false;
		[Header("(Between 0 and 1)")]
		public float ProbabilityOfFailureResult = 0;
		public InteractionResult FailureResult;

		[HideInInspector]
		public int Completed = 0;
		[HideInInspector]
		public int TurnCompletedOn = -1;
		[HideInInspector]
		public bool New = true;

		public void Setup()
		{
			New = true;
			Completed = 0;
			TurnCompletedOn = -1;
		}

		public bool IsVisible(MainGameManager mgm)
		{
			if (Completed > 0 && !Repeatable)
				return false;

			if (mgm.DebugAll)
				return true;

			return Requirements.VisRequirementsAreMet();
		}

		public bool InteractionValid(MainGameManager mgm)
		{
			if (Completed > 0 && !Repeatable)
				return false;

			if (mgm.DebugAll)
				return true;

			if (!IsVisible(mgm))
				return false;

			if (!Requirements.RequirementsAreMet(mgm))
				return false;

			if (Completed > 0 && Repeatable && mgm.Data.TurnNumber <= TurnCompletedOn + Cooldown)
				return false;

			return Cost.CanAffordCost(mgm);
		}

		public bool IsNew(MainGameManager mgm)
		{
			return AlertNew && New && InteractionValid(mgm);
		}

		public bool GetInteractionSucceeded()
		{
			if (!CanFail)
				return true;

			UnityEngine.Random.InitState((int)Time.time);

			var val = Random.Range(0f, 1f);

			bool succeeded = val >= ProbabilityOfFailureResult;

			var succStr = succeeded ? "Succeeded" : "Failed";
			Debug.Log($"{Name}: {succStr}!  Rolled a {val} and needed to be greater than {ProbabilityOfFailureResult}.");

			return succeeded;
		}

		public InteractionResult GetInteractionResult(bool succeeded)
		{
			return succeeded ? Result : FailureResult;
		}
	}
}