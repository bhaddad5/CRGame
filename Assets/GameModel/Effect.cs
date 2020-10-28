using System.Collections.Generic;
using UnityEngine;

namespace Assets.GameModel
{
	public class Effect
	{
		public float AmbitionEffect = 0;
		public float PrideEffect = 0;
		public float EgoEffect = 0;
		public float FundsEffect = 0;
		public bool ControlEffect = false;
		public List<string> TraitsAdded;
		public List<string> TraitsRemoved;

		public void ExecuteEffect(MainGameManager mgm, Fem fem)
		{
			mgm.Data.Ego = Mathf.Max(mgm.Data.Ego + EgoEffect, 0);
			mgm.Data.Funds = Mathf.Max(mgm.Data.Funds + FundsEffect, 0);

			fem.Pride = Mathf.Max(fem.Pride + PrideEffect, 0);
			fem.Ambition = Mathf.Max(fem.Ambition + AmbitionEffect, 0);
			fem.Controlled = fem.Controlled || ControlEffect;
		}
	}
}
