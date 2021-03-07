using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.GameModel
{
	public class Fem
	{
		public class OfficeLayout
		{
			public float X = 0.5f;
			public float Y = 0.5f;
			public float Width = 200f;
		}

		public string Id;

		public bool Controlled;

		public float Ambition;
		public float Pride;

		public string FirstName;
		public string LastName;
		public int Age;

		public string RequiredVisibilityInteraction;

		public OfficeLayout Layout = new OfficeLayout();
		public OfficeLayout PersonalLayout = new OfficeLayout();

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
			var femId = split[0].Trim();

			return mgm.Data.GetCompletedInteractionIds(femId).Contains(id);
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
