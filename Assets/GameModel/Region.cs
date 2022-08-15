using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.GameModel
{
	[Serializable]
	public class Region : ScriptableObject
	{
		[HideInInspector]
		public string Id;

		public string Name;

		public Texture2D Icon;
		public Texture2D MapImage;

		public Vector2 UiPosition;

		public List<Location> QuickAccessLocations = new List<Location>();
		public List<Location> Locations = new List<Location>();

		public bool IsVisible(MainGameManager mgm)
		{
			return true;
		}
	}
}