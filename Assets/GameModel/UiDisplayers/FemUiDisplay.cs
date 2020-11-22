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

		[SerializeField] private InteractionsDisplayHandler InteractionsHandler;
		[SerializeField] private DialogDisplayHandler DialogHandler;

		private Fem fem;
		public void Setup(Fem fem, MainGameManager mgm, DepartmentUiDisplay duid)
		{
			this.fem = fem;

			InteractionsHandler.Setup(fem.Interactions, fem, mgm, DialogHandler);

			DialogHandler.Setup(fem, this);
			
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