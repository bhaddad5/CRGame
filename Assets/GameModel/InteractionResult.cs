using System;
using System.Collections;
using System.Collections.Generic;
using Assets.GameModel;
using UnityEngine;

namespace Assets.GameModel
{
	[Serializable]
	public struct InteractionResult
	{
		public List<DialogEntry> Dialogs;
		public List<Popup> OptionalPopups;
		public List<Effect> Effects;
		public int Probability;

		public void Execute(MainGameManager mgm, Npc npc)
		{
			foreach (var effect in Effects)
			{
				effect.ExecuteEffect(mgm, npc);
			}
		}
	}
}