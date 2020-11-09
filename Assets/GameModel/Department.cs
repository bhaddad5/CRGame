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
			return Fems[0].Controlled;
		}

		public void HandleEndTurn(MainGameManager mgm)
		{
			foreach (var fem in Fems)
			{
				fem.HandleEndTurn(mgm, this);
			}
		}
	}
}
