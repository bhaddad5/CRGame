﻿using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.GameModel
{
	public class Location
	{
		public string Id;
		public string Name;

		public Vector2 UiPosition;

		public bool ClosedOnWeekends;
		public bool Accessible = true;

		public Sprite Icon;
		public Sprite BackgroundImage;

		public List<Policy> Policies;
		public List<Mission> Missions;
		public List<Npc> Npcs;

		public bool Controlled()
		{
			return Npcs[0].Controlled;
		}

		public bool IsAccessible(MainGameManager mgm)
		{
			var dayOfWeek = mgm.GetDateFromTurnNumber().DayOfWeek;
			return !ClosedOnWeekends || (dayOfWeek != DayOfWeek.Saturday && dayOfWeek != DayOfWeek.Sunday);
		}
	}
}
