using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.GameModel
{
	public class LocationLayout
	{
		public float X = 0.5f;
		public float Y = 0.5f;
		public float Width = 200f;
	}

	public class Npc
	{
		public string Id;

		public bool Controlled;

		public float Ambition;
		public float Pride;

		public string FirstName;
		public string LastName;
		public int Age;

		public string RequiredVisibilityInteraction;

		public LocationLayout Layout = new LocationLayout();
		public LocationLayout PersonalLayout = new LocationLayout();

		public Sprite BackgroundImage;

		public List<Interaction> Interactions = new List<Interaction>();
		public List<Trait> Traits = new List<Trait>();
		public List<Trophy> Trophies = new List<Trophy>();

		public bool IsVisible(MainGameManager mgm)
		{
			if (String.IsNullOrEmpty(RequiredVisibilityInteraction))
				return true;
			var interaction = RequiredVisibilityInteraction;
			var split = interaction.Split('-');
			var id = split[1].Trim();
			var npcId = split[0].Trim();

			return mgm.Data.GetCompletedInteractionIds(npcId).Contains(id);
		}

		public string DetermineCurrPictureId()
		{
			if (Controlled && Pride < 1)
				return "trained";
			else if (Controlled)
				return "controlled";
			else
				return "independent";
		}
	}
}
