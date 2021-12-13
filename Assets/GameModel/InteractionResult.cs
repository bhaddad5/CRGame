﻿using System;
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
		public int Probability;

		[Header("Additional Options:")]
		public Texture2D CustomBackground;
		public NpcLayout CustomBackgroundNpcLayout;

		public void Execute(MainGameManager mgm, Npc npc)
		{
			Effect.ExecuteEffect(mgm, npc);
		}
	}
}