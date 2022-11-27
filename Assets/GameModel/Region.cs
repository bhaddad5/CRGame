using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.GameModel
{
	[Serializable]
	public struct RegionResources
	{
		public bool HasAnyResources;
		public bool HasPower;
		public bool HasSpreadsheets;
		public bool HasBrand;
		public bool HasRevanue;
		public bool HasPatents;
		public bool HasCulture;
	}

	[Serializable]
	public class Region : ScriptableObject
	{
		[HideInInspector]
		public string Id;

		public string Name;

		public Texture2D Icon;
		public Texture2D MapImage;

		public Vector2 UiPosition;

		public RegionResources RegionResources;

		public List<Interaction> VisibilityInteractions;
		public List<Interaction> VisibilityNotCompletedInteractions;

		public List<Location> QuickAccessLocations = new List<Location>();
		public List<Location> Locations = new List<Location>();

		public AudioClip BackgroundAmbience = null;
		public List<AudioClip> WeekMusicTracks = new List<AudioClip>();

		public List<AudioClip> WeekendMusicTracks = new List<AudioClip>();

		public void Setup(MainGameManager mgm)
		{
			foreach (var ob in Locations)
				ob.Setup(mgm);
		}

		public List<AudioClip> GetCurrMusicTracks(MainGameManager mgm)
		{
			return mgm.IsWeekend() ? WeekendMusicTracks : WeekMusicTracks;
		}

		public bool IsVisible(MainGameManager mgm)
		{
			if (mgm.DebugAll)
				return true;

			foreach (var interaction in VisibilityInteractions)
			{
				if (interaction != null && interaction.Completed == 0)
					return false;
			}
			foreach (var interaction in VisibilityNotCompletedInteractions)
			{
				if (interaction != null && interaction.Completed > 0)
					return false;
			}
			return true;
		}

		public bool HasNewInteractions(MainGameManager mgm)
		{
			return Locations.Any(l => l.IsVisible(mgm) && (l.HasNewInteractions(mgm) || l.HasNewPolicies(mgm)) && l.IsAccessible(mgm));
		}
	}
}