using System.Collections;
using System.Collections.Generic;
using Assets.GameModel;
using UnityEngine;

namespace Assets.GameModel
{
	public class InteractionResult
	{
		public List<DialogEntry> Dialogs;
		public Popup OptionalPopup;
		public List<Effect> Effects;

		public void Execute(MainGameManager mgm, Npc npc)
		{
			foreach (var effect in Effects)
			{
				effect.ExecuteEffect(mgm, npc);
			}
		}
	}
}