using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = System.Random;

namespace Assets.GameModel
{
		

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
		
		public ActionRequirements VisRequirements;

		[Header("Screen Position in Department/Location")]
		public NpcLayout LocationLayout = new NpcLayout() { width = 200, xPos = .5f, yPos = .5f };

		[Header("Screen Position when talking to her")]
		public NpcLayout PersonalLayout = new NpcLayout() { width = 200, xPos = .5f, yPos = .5f };

		public Texture2D BackgroundImage;
		public AudioClip OptionalDialogClip = null;

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

		public List<ImageSet> StartingImageSets = new List<ImageSet>();
		public List<ImageSet> AllImageSets = new List<ImageSet>();

		[HideInInspector]
		public List<ImageSet> CurrentImageSets = new List<ImageSet>();

		public List<Texture2D> IndependentImages = new List<Texture2D>();
		public List<Texture2D> ControlledImages = new List<Texture2D>();
		public List<Texture2D> TrainedImages = new List<Texture2D>();

		[Header("LEGACY DATA UPGRADE, DO NOT TOUCH:")]
		public ImageSet Legacy_IndependentImages;
		public ImageSet Legacy_ControlledImages;
		public ImageSet Legacy_TrainedImages;

		[HideInInspector]
		public List<string> RemovedImages = new List<string>();

		private MainGameManager mgm;

		public void Setup(MainGameManager mgm)
		{
			this.mgm = mgm;

			Controlled = false;
			Trained = false;
			Exists = true;
			RemovedImages.Clear();
			Ambition = StartingAmbition;
			Pride = StartingPride;

			CurrentImageSets.Clear();
			foreach (var imageSet in StartingImageSets)
				CurrentImageSets.Add(imageSet);

			foreach (var ob in Interactions)
			{
				ob.Setup();
			}

			foreach (var ob in Trophies)
			{
				ob.Setup();
			}
		}

		public bool IsVisible(MainGameManager mgm)
		{
			if (!Exists)
				return false;

			if (mgm.DebugAll)
				return true;

			//return VisRequirements.VisRequirementsAreMet();
			//We should never see disabled npcs
			return VisRequirements.RequirementsAreMet(mgm);
		}

		public bool IsAccessible(MainGameManager mgm)
		{
			if (mgm.DebugAll)
				return true;

			return VisRequirements.RequirementsAreMet(mgm);
		}

		public bool HasNewInteractions(MainGameManager mgm)
		{
			return Interactions.Any(i => i.IsNew(mgm) && !i.SubInteraction);
		}

		public bool CanRemoveCurrentImage()
		{
			int totalAvailableImages = 0;
			foreach (var imageSet in CurrentImageSets)
				totalAvailableImages += imageSet.Images.Count(img => !RemovedImages.Contains(img.name));

			return totalAvailableImages > 1;
		}

		public Texture2D GetCurrentPicture()
		{
			Random r = new Random((int)(mgm.Data.TurnNumber/2));

			//TODO: What if this image set has nothing in it, but others do?

			var resImages = new List<Texture2D>();
			foreach (var imageSet in CurrentImageSets)
			{
				resImages = resImages.Concat(imageSet.Images.Where(img => !RemovedImages.Contains(img.name))).ToList();
			}

			resImages = resImages.ToList();

			//Just default if no image available
			if (resImages.Count == 0)
				return AllImageSets.First().Images[0];

			return resImages[r.Next(0, resImages.Count)];
		}

		public override string ToString()
		{
			return $"{FirstName} {LastName}";
		}
	}
}
