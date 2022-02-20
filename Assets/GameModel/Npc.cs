using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = System.Random;

namespace Assets.GameModel
{
	[Serializable]
	public struct NpcLayout
	{
		public float xPos;
		public float yPos;
		public float width;

		public void ApplyToRectTransform(RectTransform rt)
		{
			var ratio = 2;
			rt.anchorMin = new Vector2(xPos, yPos);
			rt.anchorMax = new Vector2(xPos, yPos);
			rt.sizeDelta = new Vector2(width, width * ratio);
			rt.anchoredPosition = Vector2.zero;
		}
	}

	[Serializable]
	public struct NpcData
	{
		public float Ambition;
		public float Pride;
		public bool Controlled;
		public bool Exists;
		public bool Trained;
	}

	[CreateAssetMenu(fileName = "New Interaction", menuName = "Company Man Data/NPC", order = 1)]
	[Serializable]
	public class Npc : ScriptableObject
	{
		[HideInInspector]
		public string Id;

		public bool IsControllable;
		

		public float StartingAmbition;
		public float StartingPride;

		public string FirstName;
		public string LastName;
		public int Age;
		public string Education;
		[TextArea(15, 20)]
		public string Bio;

		public Interaction RequiredVisibilityInteractionReference;

		[Header("Screen Position in Department/Location")]
		public NpcLayout LocationLayout = new NpcLayout() { width = 200, xPos = .5f, yPos = .5f };

		[Header("Screen Position when talking to her")]
		public NpcLayout PersonalLayout = new NpcLayout() { width = 200, xPos = .5f, yPos = .5f };

		public Texture2D BackgroundImage;
		
		[HideInInspector]
		public float Ambition;
		[HideInInspector]
		public float Pride;
		[HideInInspector]
		public bool Controlled;
		[HideInInspector]
		public bool Trained;
		[HideInInspector]
		public bool Exists = true;

		public List<Interaction> Interactions = new List<Interaction>();
		public List<Trophy> Trophies = new List<Trophy>();

		public List<Texture2D> IndependentImages = new List<Texture2D>();
		public List<Texture2D> ControlledImages = new List<Texture2D>();
		public List<Texture2D> TrainedImages = new List<Texture2D>();
		private MainGameManager mgm;

		public void Setup(MainGameManager mgm)
		{
			this.mgm = mgm;

			Controlled = false;
			Trained = false;
			Exists = true;
			Ambition = StartingAmbition;
			Pride = StartingPride;

			foreach (var ob in Interactions)
				ob.Setup();
			foreach (var ob in Trophies)
				ob.Setup();
		}

		public bool IsVisible(MainGameManager mgm)
		{
			if (!Exists)
				return false;

			return (RequiredVisibilityInteractionReference?.Completed ?? 1) > 0;
		}

		public bool HasNewInteractions(MainGameManager mgm)
		{
			return Interactions.Any(i => i.IsNew(mgm));
		}

		public Texture2D GetCurrentPicture()
		{
			Random r = new Random(mgm.Data.TurnNumber);
			if (Trained)
				return TrainedImages[r.Next(0, TrainedImages.Count)];
			else if (Controlled)
				return ControlledImages[r.Next(0, ControlledImages.Count)];
			else
				return IndependentImages[r.Next(0, IndependentImages.Count)];
		}

		public override string ToString()
		{
			return $"{FirstName} {LastName}";
		}
	}
}
