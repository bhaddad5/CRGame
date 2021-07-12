﻿using System.Collections;
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
		[SerializeField] private TMP_Text Traits;
		[SerializeField] private Button BackButton;
		[SerializeField] private Image Picture;
		[SerializeField] private Image BackgroundImage;
		[SerializeField] private Transform InfoBox;

		[SerializeField] public InteractionsDisplayHandler InteractionsHandler;
		[SerializeField] private DialogDisplayHandler DialogHandler;

		void Start()
		{
			Picture.GetComponent<RectTransform>().ApplyLayout(_npc.PersonalLayout);
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
			Traits.text = GetTraitsString();
			BackgroundImage.sprite = _npc.BackgroundImage;

			InteractionsHandler.RefreshInteractionVisibilities(_npc, mgm);
		}

		private string overridingImage = null;
		public void SetImage(string image)
		{
			overridingImage = image;
			Picture.sprite = LoadNpcPicture();
		}

		public void UnsetImage()
		{
			overridingImage = null;
		}

		private string GetTraitsString()
		{
			string traitsText = "";
			foreach (var trait in _npc.Traits)
			{
				traitsText += trait.Name + ",";
			}
			if (traitsText.EndsWith(","))
				traitsText = traitsText.Substring(0, traitsText.Length - 1);
			return $"Traits: {traitsText}";
		}

		private Sprite LoadNpcPicture()
		{
			return NpcPicManager.GetNpcPicFromId(_npc.Id, overridingImage ?? _npc.DetermineCurrPictureId());
		}
	}
}