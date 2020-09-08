using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.GameModel
{
	public class Department
	{
		public string Id;
		public string Name;

		public List<Policy> Policies;
		public List<Fem> Fems;

		public bool Controlled()
		{
			return Fems.All(f => f.Controlled);
		}

		public bool ActivatePolicy(MainGameManager mgm, string policyId)
		{
			mgm.ActivePolicies.Add(policyId);
			Policies.First(pol => pol.Id == policyId).Active = true;
		}
	}
}
