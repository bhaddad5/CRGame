﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.GameModel
{
	public class Department
	{
		public string Id;
		public string Name;

		public Vector2 UiPosition;

		public bool Accessible = true;

		public List<Policy> Policies;
		public List<Fem> Fems;

		public bool Controlled()
		{
			return Fems.All(f => f.Controlled);
		}

		public void ActivatePolicy(MainGameManager mgm, string policyId)
		{
			mgm.ActivePolicies.Add(policyId);
			Policies.First(pol => pol.Id == policyId).Active = true;
		}
	}
}
