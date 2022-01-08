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

		public Effect Effect;

		public void Execute(MainGameManager mgm)
		{
			Effect.ExecuteEffect(mgm);
		}
	}
}