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
	public class NpcUiDisplay : MonoBehaviour, IUiDisplay
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

		[SerializeField] public InteractionsDisplayHandler InteractionsHandler;
		[SerializeField] private DialogDisplayHandler DialogHandler;

		void Start()
		{
			_npc.ApplyPersonalLayout(Picture.GetComponent<RectTransform>());
		}

		private Npc _npc;
		public void Setup(Npc npc, MainGameManager mgm, LocationUiDisplay duid)
		{
			this._npc = npc;

			if(!npc.IsControllable)
				InfoBox.gameObject.SetActive(false);

			InteractionsHandler.Setup(npc.Interactions, npc, mgm, DialogHandler);

			DialogHandler.Setup(npc, this, mgm);
			
			BackButton.onClick.RemoveAllListeners();
			BackButton.onClick.AddListener(() => duid.CloseCurrentNpc());
		}

		public void RefreshUiDisplay(MainGameManager mgm)
		{
			Name.text = $"{_npc.FirstName} {_npc.LastName}";
			Age.text = $"{_npc.Age} years old";
			Ambition.text = $"Ambition: {_npc.Ambition}";
			Pride.text = $"Pride: {_npc.Pride}";
			Picture.sprite = LoadNpcPicture();
			Picture.preserveAspect = true;
			Bio.text = $"Notes: {_npc.Bio}";
			Education.text = $"Education: {_npc.Education}";
			BackgroundImage.sprite = _npc.BackgroundImage.ToSprite();

			InteractionsHandler.RefreshInteractionVisibilities(_npc, mgm);
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
	}
}