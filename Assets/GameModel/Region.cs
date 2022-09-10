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

		public List<Location> QuickAccessLocations = new List<Location>();
		public List<Location> Locations = new List<Location>();

		public AudioClip BackgroundAmbience = null;
		public string BackgroundAmbienceTmp
		{
			get
			{
				if (BackgroundAmbience)
					return BackgroundAmbience?.name;
				else
					return null;
			}
		}

		public List<AudioClip> WeekMusicTracks = new List<AudioClip>();

		public List<string> WeekMusicTracksTmp
		{
			get
			{
				var res = new List<string>();
				foreach (var track in WeekMusicTracks)
				{
					res.Add(track.name);
				}
				return res;
			}
		}

		public List<AudioClip> WeekendMusicTracks = new List<AudioClip>();
		public List<string> WeekendMusicTracksTmp
		{
			get
			{
				var res = new List<string>();
				foreach (var track in WeekendMusicTracks)
				{
					res.Add(track.name);
				}
				return res;
			}
		}

		public void Setup(MainGameManager mgm)
		{
			foreach (var ob in Locations)
				ob.Setup(mgm);
		}

		public List<string> GetCurrMusicTracks(MainGameManager mgm)
		{
			return mgm.IsWeekend() ? WeekendMusicTracksTmp : WeekMusicTracksTmp;
		}

		public bool IsVisible(MainGameManager mgm)
		{
			return true;
		}

		public bool HasNewInteractions(MainGameManager mgm)
		{
			return Locations.Any(l => l.IsVisible(mgm) && l.HasNewInteractions(mgm) && l.IsAccessible(mgm));
		}
	}
}