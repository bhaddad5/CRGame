using System;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

namespace Assets.GameModel
{
	[CreateAssetMenu(fileName = "New Interaction", menuName = "Company Man Data/NPC", order = 1)]
	[Serializable]
	public class Npc : ScriptableObject
	{
		public string Id;

		public bool IsControllable;
		[HideInInspector]
		public bool Controlled;

		public float Ambition;
		public float Pride;

		public string FirstName;
		public string LastName;
		public int Age;

		public Interaction RequiredVisibilityInteractionReference;

		[Header("Screen Position in Department/Location")]
		public float LocationLayoutXPos = 0.5f;
		public float LocationLayoutYPos = 0.5f;
		public float LocationLayoutWidth = 200f;

		[Header("Screen Position when talking to her")]
		public float PersonalLayoutXPos = 0.5f;
		public float PersonalLayoutYPos = 0.5f;
		public float PersonalLayoutWidth = 200f;

		public Texture2D BackgroundImage;

		public List<Interaction> Interactions = new List<Interaction>();
		public List<Trophy> Trophies = new List<Trophy>();

		public List<Texture2D> IndependentImages = new List<Texture2D>();
		public List<Texture2D> ControlledImages = new List<Texture2D>();
		public List<Texture2D> TrainedImages = new List<Texture2D>();

		public bool IsVisible(MainGameManager mgm)
		{
			return RequiredVisibilityInteractionReference?.Completed ?? true;
		}

		public Texture2D GetCurrentPicture()
		{
			if (Controlled && Pride < 1)
				return TrainedImages[UnityEngine.Random.Range(0, TrainedImages.Count)];
			else if (Controlled)
				return ControlledImages[UnityEngine.Random.Range(0, ControlledImages.Count)];
			else
				return IndependentImages[UnityEngine.Random.Range(0, IndependentImages.Count)];
		}

		public void ApplyLocationLayout(RectTransform transform)
		{
			var ratio = 2;
			transform.anchorMin = new Vector2(LocationLayoutXPos, LocationLayoutYPos);
			transform.anchorMax = new Vector2(LocationLayoutXPos, LocationLayoutYPos);
			transform.sizeDelta = new Vector2(LocationLayoutWidth, LocationLayoutWidth * ratio);
			transform.anchoredPosition = Vector2.zero;
		}

		public void ApplyPersonalLayout(RectTransform transform)
		{
			var ratio = 2;
			transform.anchorMin = new Vector2(PersonalLayoutXPos, PersonalLayoutYPos);
			transform.anchorMax = new Vector2(PersonalLayoutXPos, PersonalLayoutYPos);
			transform.sizeDelta = new Vector2(PersonalLayoutWidth, PersonalLayoutWidth * ratio);
			transform.anchoredPosition = Vector2.zero;
		}

		public override string ToString()
		{
			return $"{FirstName} {LastName}";
		}
	}
}
