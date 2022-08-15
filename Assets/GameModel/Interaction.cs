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
		public bool New = true;

		public void Setup()
		{
			New = true;
			Completed = 0;
		}

		public bool InteractionVisible(MainGameManager mgm)
		{
			if (Completed > 0 && !Repeatable)
				return false;

			return Requirements.VisRequirementsAreMet();
		}

		public bool InteractionValid(MainGameManager mgm)
		{
			if (!InteractionVisible(mgm))
				return false;

			if (!Requirements.RequirementsAreMet(mgm))
				return false;

			return Cost.CanAffordCost(mgm);
		}

		public bool IsNew(MainGameManager mgm)
		{
			return AlertNew && New && InteractionValid(mgm);
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