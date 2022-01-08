using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.GameModel
{
	[Serializable]
	public struct NpcStatRequirement
	{
		public enum NpcStat
		{
			Ambition,
			Pride,
		}

		public enum Comparison
		{
			LessThanOrEqualTo = 0,
			EqualTo = 1,
			GreaterOrEqualTo = 2,
		}

		[Header("Defaults to the parent NPC, if present")]
		public Npc OptionalNpcReference;

		public NpcStat Stat;
		public Comparison Is;
		public float Value;
		
		public bool CheckStat(float val)
		{
			if (Is == Comparison.LessThanOrEqualTo)
				return val <= Value;
			else if (Is == Comparison.GreaterOrEqualTo)
				return val >= Value;
			else
				return val.Equals(Value);
		}
	}
	

	[Serializable]
	public struct ActionRequirements
	{
		public float RequiredPower;
		public int RequiredPromotionLevel;
		public int RequiredTurnNumber;
		
		public List<NpcStatRequirement> NpcStatRequirements;
		
		public List<Interaction> RequiredInteractions;
		public List<Interaction> RequiredNotCompletedInteractions;
		public List<Policy> RequiredPolicies;
		public List<Location> RequiredDepartmentsControled;
		public List<Npc> RequiredNpcsControled;
		public List<Npc> RequiredNpcsTrained;
		public List<Npc> RequiredNpcsNotControled;
		public List<Trophy> RequiredTrophies;

		public bool VisRequirementsAreMet()
		{
			foreach (var interaction in RequiredInteractions)
			{
				if (interaction.Completed == 0)
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

			foreach (var interaction in RequiredNotCompletedInteractions)
			{
				if (interaction.Completed > 0)
					return false;
			}

			foreach (var notControlledNpc in RequiredNpcsNotControled)
			{
				if (notControlledNpc.Controlled)
					return false;
			}

			//This is a weird hack.  Basically we don't wanna display the interaction if it is based on pride and you don't control them yet
			foreach (var req in NpcStatRequirements)
			{
				if (req.Stat == NpcStatRequirement.NpcStat.Pride && !req.CheckStat(req.OptionalNpcReference.Pride) && !req.OptionalNpcReference.Controlled)
					return false;
			}

			return true;
		}

		public bool RequirementsAreMet(MainGameManager mgm)
		{
			if (!VisRequirementsAreMet())
				return false;

			foreach (var req in NpcStatRequirements)
			{
				if (req.Stat == NpcStatRequirement.NpcStat.Ambition && !req.CheckStat(req.OptionalNpcReference.Ambition))
					return false;
				if (req.Stat == NpcStatRequirement.NpcStat.Pride && !req.CheckStat(req.OptionalNpcReference.Pride))
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