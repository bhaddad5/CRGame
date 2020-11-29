using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.GameModel
{
	public class Department
	{
		public string Id;
		public string Name;

		public Vector2 UiPosition;

		public bool ClosedOnWeekends;
		public bool Accessible = true;

		public List<Policy> Policies;
		public List<Fem> Fems;

		public bool Controlled()
		{
			return Fems[0].Controlled;
		}
	}
}
