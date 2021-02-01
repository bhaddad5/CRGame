using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.GameModel
{
	public class ActionRequirements
	{
		public List<string> RequiredInteractions = new List<string>();
		public List<string> RequiredPolicies = new List<string>();
		public List<string> RequiredDepartmentsControled = new List<string>();
		public List<string> RequiredTrophies = new List<string>();
		public float RequiredPower = 0;

		//If there is a fem
		public bool RequiredControl = false;
		public float RequiredAmbition = -1;
		public float RequiredPride = -1;

		public bool RequirementsAreMet(MainGameManager mgm, Fem fem)
		{
			if (RequiredControl && !fem.Controlled)
			{
				return false;
			}

			foreach (var interactionDept in RequiredDepartmentsControled)
			{
				if (!mgm.Data.GetControlledDepartmentIds().Contains(interactionDept))
					return false;
			}

			foreach (var trophy in RequiredTrophies)
			{
				if (!mgm.Data.GetOwnedTrophyIds().Contains(trophy))
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

			if (RequiredAmbition >= 0 && RequiredAmbition < fem.Ambition)
			{
				return false;
			}

			if (RequiredPride >= 0 && RequiredPride < fem.Pride)
			{
				return false;
			}

			if (mgm.Data.Power < RequiredPower)
			{
				return false;
			}

			return true;
		}

		public List<string> GetInvalidTooltips(MainGameManager mgm, Fem fem)
		{
			List<string> tooltips = new List<string>();
			if (RequiredAmbition >= 0 && RequiredAmbition < fem.Ambition)
				tooltips.Add($"Requires {RequiredAmbition} or less Ambition");
			if (RequiredPride >= 0 && RequiredPride < fem.Pride)
				tooltips.Add($"Requires {RequiredPride} or less Pride");
			if (RequiredPower > mgm.Data.Power)
				tooltips.Add($"Requires {RequiredPower} or more Power");

			return tooltips;
		}
	}
}