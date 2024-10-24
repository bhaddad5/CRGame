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

		public List<Interaction> Choices;

		public Effect Effect;

		public Region GoToRegion;

		public void Execute(MainGameManager mgm)
		{
			Effect.ExecuteEffect(mgm);

			if (GoToRegion != null)
			{
				mgm.OpenRegion(GoToRegion);
			}
		}
	}
}