using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.GameModel
{
	[Serializable]
	public struct NpcRequirement
	{
		[Header("Defaults to the parent NPC, if present")]
		public Npc OptionalNpcReference;

		public bool RequiresAmbitionAtOrBelowValue;
		public float RequiredAmbition;
		public bool RequiresPrideAtOrBelowValue;
		public float RequiredPride;
	}

	[Serializable]
	public struct ActionRequirements
	{
		public float RequiredPower;
		public int RequiredPromotionLevel;
		public int RequiredTurnNumber;

		public List<NpcRequirement> NpcRequirements;

		public List<Interaction> RequiredInteractions;
		public List<Interaction> RequiredNotCompletedInteractions;
		public List<Policy> RequiredPolicies;
		public List<Location> RequiredDepartmentsControled;
		public List<Npc> RequiredNpcsControled;
		public List<Npc> RequiredNpcsTrained;
		public List<Npc> RequiredNpcsNotControled;
		public List<Trophy> RequiredTrophies;
		
		public bool RequirementsAreMet(MainGameManager mgm)
		{
			foreach (var npcRequirement in NpcRequirements)
			{
				if (npcRequirement.OptionalNpcReference == null)
				{
					Debug.LogError($"Trying to test ambition/pride on a null npc!");
					continue;
				}

				if (npcRequirement.RequiresAmbitionAtOrBelowValue && npcRequirement.RequiredAmbition < npcRequirement.OptionalNpcReference.Ambition)
					return false;

				if (npcRequirement.RequiresPrideAtOrBelowValue && npcRequirement.RequiredPride < npcRequirement.OptionalNpcReference.Pride)
					return false;
			}
			
			foreach (var controlledNpc in RequiredNpcsControled)
			{
				if (!controlledNpc.Controlled)
					return false;
			}

			foreach (var trainedNpc in RequiredNpcsTrained)
			{
				if (!trainedNpc.Trained)
					return false;
			}

			foreach (var notControlledNpc in RequiredNpcsNotControled)
			{
				if (notControlledNpc.Controlled)
					return false;
			}

			foreach (var interactionDept in RequiredDepartmentsControled)
			{
				if (!interactionDept.Controlled)
					return false;
			}

			foreach (var trophy in RequiredTrophies)
			{
				if (!mgm.Data.GetOwnedTrophies().Contains(trophy))
					return false;
			}

			foreach (var interaction in RequiredInteractions)
			{
				if (interaction.Completed == 0)
					return false;
			}

			foreach (var interaction in RequiredNotCompletedInteractions)
			{
				if (interaction.Completed > 0)
					return false;
			}

			foreach (var policy in RequiredPolicies)
			{
				if (!policy.Active)
					return false;
			}

			if (mgm.Data.Power < RequiredPower)
				return false;

			if (mgm.Data.Promotion < RequiredPromotionLevel)
				return false;

			if (mgm.Data.TurnNumber < RequiredTurnNumber)
				return false;

			return true;
		}
	}
}