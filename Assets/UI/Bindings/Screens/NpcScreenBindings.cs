using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Assets.GameModel;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.GameModel.UiDisplayers
{
	public class NpcScreenBindings : MonoBehaviour
	{
		[SerializeField] private TMP_Text Name;
		[SerializeField] private TMP_Text Age;
		[SerializeField] private TMP_Text Ambition;
		[SerializeField] private TMP_Text Pride;
		[SerializeField] private TMP_Text Education;
		[SerializeField] private TMP_Text Bio;
		[SerializeField] private Button BackButton;
		[SerializeField] private Image Picture;
		[SerializeField] private Image BackgroundImage;
		[SerializeField] private Transform InfoBox;

		//TODO: MAKE PRIVATE AGAIN!
		[SerializeField] public Transform InteractionsParent;

		[SerializeField] private NpcInteractionEntryBindings InteractionEntryPrefab;

		void Start()
		{
			_npc.ApplyPersonalLayout(Picture.GetComponent<RectTransform>());
		}

		private Npc _npc;
		public void Setup(Npc npc, MainGameManager mgm, LocationScreenBindings duid)
		{
			this._npc = npc;

			if(!npc.IsControllable)
				InfoBox.gameObject.SetActive(false);

			var allInteractions = new List<Interaction>(npc.Interactions);
			allInteractions.RemoveAll(i => i == null);
			allInteractions.Sort((i1, i2) => ((int)i1.Category).CompareTo((int)i2.Category));

			foreach (var interaction in allInteractions)
			{
				if (!interaction.InteractionVisible(mgm))
					continue;
				var interactButton = Instantiate(InteractionEntryPrefab);
				interactButton.Setup(interaction, mgm, this);
				interactButton.transform.SetParent(InteractionsParent);
			}

			BackButton.onClick.RemoveAllListeners();
			BackButton.onClick.AddListener(() => duid.CloseCurrentNpc());
		}

		public void RefreshUiDisplay(MainGameManager mgm)
		{
			Name.text = $"{_npc.FirstName} {_npc.LastName}";

			if (_npc.Trained)
				Name.text += " (Trained)";
			else if (_npc.Controlled)
				Name.text += " (Controlled)";

			Age.text = $"{_npc.Age} years old";
			Ambition.text = $"Ambition: {_npc.Ambition}";
			Pride.text = $"Pride: {_npc.Pride}";
			Picture.sprite = LoadNpcPicture();
			Picture.preserveAspect = true;
			Bio.text = $"Notes: {_npc.Bio}";
			Education.text = $"Education: {_npc.Education}";
			BackgroundImage.sprite = _npc.BackgroundImage.ToSprite();
		}

		private Texture2D overridingImage = null;
		public void SetImage(Texture2D image)
		{
			overridingImage = image;
			Picture.sprite = LoadNpcPicture();
		}

		public void UnsetImage()
		{
			overridingImage = null;
		}

		public void SetBackground(Texture2D background)
		{
			BackgroundImage.sprite = background.ToSprite();
		}

		public void UnsetBackground()
		{
			BackgroundImage.sprite = _npc.BackgroundImage.ToSprite();
		}
		
		private Sprite LoadNpcPicture()
		{
			return (overridingImage ?? _npc.GetCurrentPicture()).ToSprite();
		}

		public void SetCustomLayout(NpcLayout layout)
		{
			layout.ApplyToRectTransform(Picture.GetComponent<RectTransform>());
		}
	}
}