using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.GameModel
{
	[Serializable]
	public struct ActionRequirements
	{
		public float RequiredPower;
		//If there is a npc
		public bool RequiredControl;
		public bool RequiresAmbitionAtOrBelowValue;
		public float RequiredAmbition;
		public bool RequiresPrideAtOrBelowValue;
		public float RequiredPride;

		public List<Interaction> RequiredInteractions;
		public List<Interaction> RequiredNotCompletedInteractions;
		public List<Policy> RequiredPolicies;
		public List<Location> RequiredDepartmentsControled;
		public List<Trophy> RequiredTrophies;
		
		public bool RequirementsAreMet(MainGameManager mgm, Npc npc)
		{
			if (RequiredControl && !npc.Controlled)
			{
				return false;
			}

			foreach (var interactionDept in RequiredDepartmentsControled)
			{
				if (interactionDept == null)
					continue;
				if (!interactionDept.Controlled())
					return false;
			}

			foreach (var trophy in RequiredTrophies)
			{
				if (!mgm.Data.GetOwnedTrophies().Contains(trophy))
					return false;
			}

			foreach (var interaction in RequiredInteractions)
			{
				if(interaction == null)
					continue;

				if (interaction.Completed == 0)
					return false;
			}

			foreach (var interaction in RequiredNotCompletedInteractions)
			{
				if (interaction == null)
					continue;

				if (interaction.Completed > 0)
					return false;
			}

			foreach (var policy in RequiredPolicies)
			{
				if(policy == null)
					continue;

				if (!policy.Active)
					return false;
			}

			if (RequiresAmbitionAtOrBelowValue && RequiredAmbition < npc.Ambition)
			{
				return false;
			}

			if (RequiresPrideAtOrBelowValue && RequiredPride < npc.Pride)
			{
				return false;
			}

			if (mgm.Data.Power < RequiredPower)
			{
				return false;
			}

			return true;
		}
	}
}