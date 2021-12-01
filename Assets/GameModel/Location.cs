using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.GameModel
{
	[CreateAssetMenu(fileName = "New Interaction", menuName = "Company Man Data/Location", order = 0)]
	[Serializable]
	public class Location : ScriptableObject
	{
		[HideInInspector]
		public string Id;

		public string Name;
		public Texture2D Icon;
		public Texture2D BackgroundImage;

		public Vector2 UiPosition;

		public List<Interaction> VisibilityInteractions;
		public List<Interaction> VisibilityNotCompletedInteractions;
		public bool ClosedOnWeekends;

		public Interaction ControlInteractionReference;

		public List<Policy> Policies;
		public List<Mission> Missions;
		public List<Npc> Npcs;

		public bool ShowTrophyCase;
		public bool ShowCar;


		public bool IsVisible(MainGameManager mgm)
		{
			foreach (var interaction in VisibilityInteractions)
			{
				if (interaction != null && !interaction.Completed)
					return false;
			}
			foreach (var interaction in VisibilityNotCompletedInteractions)
			{
				if (interaction != null && interaction.Completed)
					return false;
			}
			return true;
		}

		public bool Controlled()
		{
			return ControlInteractionReference?.Completed ?? false;
		}

		public bool IsAccessible(MainGameManager mgm)
		{
			var dayOfWeek = mgm.GetDateFromTurnNumber().DayOfWeek;
			return !ClosedOnWeekends || (dayOfWeek != DayOfWeek.Saturday && dayOfWeek != DayOfWeek.Sunday);
		}
	}
}
