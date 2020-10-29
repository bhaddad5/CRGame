using System.Collections;
using System.Collections.Generic;
using Assets.GameModel;
using UnityEngine;

namespace Assets.GameModel
{
	public class InteractionResult
	{
		public List<DialogEntry> Dialogs;
		public List<Effect> Effects;

		public void Execute(MainGameManager mgm, Fem fem)
		{
			foreach (var effect in Effects)
			{
				effect.ExecuteEffect(mgm, fem);
			}
		}
	}
}