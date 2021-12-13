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

		[Header("Additional Options:")]
		public Texture2D CustomBackground;
		public NpcLayout CustomBackgroundNpcLayout;

		public void Execute(MainGameManager mgm, Npc contextualNpc = null)
		{
			Effect.ExecuteEffect(mgm, contextualNpc);
		}
	}
}