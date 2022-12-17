using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.GameModel
{
	[Serializable]
	public class Location : ScriptableObject
	{
		[HideInInspector]
		public string Id;

		public string Name;
		[TextArea(15, 20)]
		public string Description;

		public Texture2D Icon;
		public Texture2D BackgroundImage;
		public AudioClip OptionalBackgroundAudio = null;

		public enum LocationType
		{
			Misc,
			Office,
			Store,
			Home,
			Fun
		}

		public LocationType locationType;

		public Vector2 UiPosition;

		public ActionRequirements VisRequirements;
		
		public List<Policy> Policies = new List<Policy>();
		public List<Mission> Missions = new List<Mission>();
		public List<Npc> Npcs = new List<Npc>();

		public bool ShowTrophyCase;
		public bool ShowCar;
		public bool ShowMyOfficeCustomBackground;
		public bool ShowMyHome;

		[HideInInspector]
		public bool Controlled;

		public void Setup(MainGameManager mgm)
		{
			Controlled = false;

			foreach (var ob in Npcs)
				ob.Setup(mgm);
			foreach (var ob in Missions)
				ob.Setup();
			foreach (var ob in Policies)
				ob.Setup();
		}
		
		public bool IsVisible(MainGameManager mgm)
		{
			if (mgm.DebugAll)
				return true;

			if (!Npcs.Any(npc => npc?.IsVisible(mgm) ?? false) && (Policies?.Count ?? 0) == 0 && (Missions?.Count ?? 0) == 0)
				return false;

			return VisRequirements.VisRequirementsAreMet();
		}

		public bool HasNewInteractions(MainGameManager mgm)
		{
			return Npcs.Any(n => n.IsVisible(mgm) && n.HasNewInteractions(mgm));
		}

		public bool HasNewPolicies(MainGameManager mgm)
		{
			return Policies.Any(p => p.IsNew(mgm));
		}

		public bool IsAccessible(MainGameManager mgm)
		{
			return VisRequirements.RequirementsAreMet(mgm);
		}
	}
}
