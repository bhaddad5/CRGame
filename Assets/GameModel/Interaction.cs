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

		public int TurnCost = 1;
		public float EgoCost = 0;
		public float MoneyCost = 0;
		public float SpreadsheetsCost = 0;
		public float CultureCost = 0;
		public float PatentsCost = 0;
		public float BrandCost = 0;
		public float RevanueCost = 0;

		public List<string> RequiredInteractions = new List<string>();
		public List<string> RequiredPolicies = new List<string>();
		public bool RequiredControl = false;
		public List<string> RequiredDepartmentsControled = new List<string>();
		public float RequiredAmbition = -1;
		public float RequiredPride = -1;

		public bool Repeatable = false;
		public bool Completed = false;

		public List<InteractionResult> InteractionResults;

		public bool InteractionVisible(MainGameManager mgm, Fem fem)
		{
			if (Completed && !Repeatable)
				return false;
			if (RequiredControl && !fem.Controlled)
				return false;
			foreach (var interactionDept in RequiredDepartmentsControled)
			{
				if (!mgm.Data.GetControlledDepartmentIds().Contains(interactionDept))
					return false;
			}
			foreach (var interactionId in RequiredInteractions)
			{
				var id = interactionId;
				bool inverse = false;
				if (id.StartsWith("!"))
				{
					id = id.Substring(1);
					inverse = true;
				}
				if (!inverse && !mgm.Data.GetCompletedInteractionIds(fem.Id).Contains(id))
					return false;
				else if (inverse && mgm.Data.GetCompletedInteractionIds(fem.Id).Contains(id))
					return false;
			}
			foreach (var policyId in RequiredPolicies)
			{
				if (!mgm.Data.GetActivePolicyIds().Contains(policyId))
					return false;
			}
			if (InteractionResults.Any(res => res.Effects.Any(eff => eff.ControlEffect)) && fem.Controlled)
				return false;

			return true;
		}

		public bool InteractionValid(MainGameManager mgm, Fem fem)
		{
			if (!InteractionVisible(mgm, fem))
				return false;
			if (RequiredAmbition >= 0 && RequiredAmbition < fem.Ambition)
				return false;
			if (RequiredPride >= 0 && RequiredPride < fem.Pride)
				return false;
			return EgoCost <= mgm.Data.Ego && 
			       MoneyCost <= mgm.Data.Funds && 
			       CultureCost <= mgm.Data.CorporateCulture && 
			       BrandCost <= mgm.Data.Brand &&
			       SpreadsheetsCost <= mgm.Data.Spreadsheets && 
			       RevanueCost <= mgm.Data.Revenue && 
			       PatentsCost <= mgm.Data.Patents;
		}

		public InteractionResult GetInteractionResult(MainGameManager mgm, Fem fem)
		{
			mgm.Data.Ego -= EgoCost;
			mgm.Data.Funds -= MoneyCost;

			Completed = true;

			var chosenResult = InteractionResults[UnityEngine.Random.Range(0, InteractionResults.Count)];
			return chosenResult;
		}
	}
}
