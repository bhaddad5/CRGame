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
	public class FemUiDisplay : MonoBehaviour, IUiDisplay
	{
		[SerializeField] private TMP_Text Name;
		[SerializeField] private TMP_Text Age;
		[SerializeField] private TMP_Text Ambition;
		[SerializeField] private TMP_Text Pride;
		[SerializeField] private TMP_Text Traits;
		[SerializeField] private Button BackButton;
		[SerializeField] private Image Picture;
		[SerializeField] private Image BackgroundImage;

		[SerializeField] public InteractionsDisplayHandler InteractionsHandler;
		[SerializeField] private DialogDisplayHandler DialogHandler;

		void Start()
		{
			Picture.GetComponent<RectTransform>().anchorMin = new Vector2(fem.PersonalLayout.X, fem.PersonalLayout.Y);
			Picture.GetComponent<RectTransform>().anchorMax = new Vector2(fem.PersonalLayout.X, fem.PersonalLayout.Y);
			Picture.GetComponent<RectTransform>().sizeDelta = new Vector2(fem.PersonalLayout.Width, fem.PersonalLayout.Width * 2f);
			Picture.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
		}

		private Fem fem;
		public void Setup(Fem fem, MainGameManager mgm, LocationUiDisplay duid)
		{
			this.fem = fem;

			InteractionsHandler.Setup(fem.Interactions, fem, mgm, DialogHandler);

			DialogHandler.Setup(fem, this, mgm);
			
			BackButton.onClick.AddListener(() => duid.CloseCurrentFem());
		}

		public void RefreshUiDisplay(MainGameManager mgm)
		{
			Name.text = $"{fem.FirstName} {fem.LastName}";
			Age.text = $"{fem.Age} years old";
			Ambition.text = $"Ambition: {fem.Ambition}";
			Pride.text = $"Pride: {fem.Pride}";
			Picture.sprite = LoadFemPicture();
			Picture.preserveAspect = true;
			Traits.text = GetTraitsString();
			BackgroundImage.sprite = fem.BackgroundImage;

			InteractionsHandler.RefreshInteractionVisibilities(fem, mgm);
		}

		private string overridingImage = null;
		public void SetImage(string image)
		{
			overridingImage = image;
			Picture.sprite = LoadFemPicture();
		}

		public void UnsetImage()
		{
			overridingImage = null;
		}

		private string GetTraitsString()
		{
			string traitsText = "";
			foreach (var trait in fem.Traits)
			{
				traitsText += trait.Name + ",";
			}
			if (traitsText.EndsWith(","))
				traitsText = traitsText.Substring(0, traitsText.Length - 1);
			return $"Traits: {traitsText}";
		}

		private Sprite LoadFemPicture()
		{
			return FemPicManager.GetFemPicFromId(fem.Id, overridingImage ?? fem.DetermineCurrPictureId());
		}
	}
}