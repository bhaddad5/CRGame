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
				if (!mgm.Data.GetControlledLocations().Contains(interactionDept))
					return false;
			}

			foreach (var trophy in RequiredTrophies)
			{
				if (!mgm.Data.GetOwnedTrophies().Contains(trophy))
					return false;
			}

			foreach (var interaction in RequiredInteractions)
			{
				if (!interaction.Completed)
					return false;
			}

			foreach (var interaction in RequiredNotCompletedInteractions)
			{
				if (interaction.Completed)
					return false;
			}

			foreach (var policy in RequiredPolicies)
			{
				if (!mgm.Data.GetActivePolicies().Contains(policy))
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

		//TODO: Delete after conversion!
		[HideInInspector]
		public List<string> RequiredInt;
		[HideInInspector]
		public List<string> RequiredPols;
		[HideInInspector]
		public List<string> RequiredDepts;
		[HideInInspector]
		public List<string> RequiredTrps;
		public void ResolveReferences(GameData data, Npc npc)
		{
			RequiredInteractions = new List<Interaction>();
			RequiredNotCompletedInteractions = new List<Interaction>();
			RequiredPolicies = new List<Policy>();
			RequiredDepartmentsControled = new List<Location>();
			RequiredTrophies = new List<Trophy>();

			foreach (var interactionId in RequiredInt)
			{
				var id = interactionId;
				bool inverse = false;
				if (id.StartsWith("!"))
				{
					id = id.Substring(1);
					inverse = true;
				}

				var npcId = npc.Id;

				if (id.Contains("-"))
				{
					var split = id.Split('-');
					id = split[1].Trim();
					npcId = split[0].Trim();
				}

				var interaction = data.GetInteractionById(npcId, interactionId);
				if (interaction != null)
				{
					if (inverse)
						RequiredNotCompletedInteractions.Add(interaction);
					else
						RequiredInteractions.Add(interaction);
				}
			}

			foreach (var policy in RequiredPols)
			{
				var pol = data.GetPolicyFromId(policy);
				if(pol != null)
					RequiredPolicies.Add(pol);
			}

			foreach (var dept in RequiredDepts)
			{
				var department = data.GetLocationById(dept);
				if(department != null)
					RequiredDepartmentsControled.Add(department);
			}

			foreach (var trophy in RequiredTrps)
			{
				var t = data.GetTrophyById(trophy);
				if(t != null)
					RequiredTrophies.Add(t);
			}
		}
		//TODO END
	}
}