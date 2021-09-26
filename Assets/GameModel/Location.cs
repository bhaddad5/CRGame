using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.GameModel
{
	[CreateAssetMenu(fileName = "New Interaction", menuName = "Company Man Data/Location", order = 0)]
	[Serializable]
	public class Location : ScriptableObject
	{
		public string Id;
		public string Name;

		public Vector2 UiPosition;

		public bool ClosedOnWeekends;
		public bool Accessible = true;

		public Texture2D Icon;
		public Texture2D BackgroundImage;

		public List<Policy> Policies;
		public List<Mission> Missions;
		public List<Npc> Npcs;

		public bool ShowTrophyCase;
		public bool ShowCar;

		public bool Controlled()
		{
			return Npcs.Count > 0 && Npcs[0].Controlled;
		}

		public bool IsAccessible(MainGameManager mgm)
		{
			var dayOfWeek = mgm.GetDateFromTurnNumber().DayOfWeek;
			return !ClosedOnWeekends || (dayOfWeek != DayOfWeek.Saturday && dayOfWeek != DayOfWeek.Sunday);
		}
	}
}
