using System.Collections;
using System.Collections.Generic;
using System.IO;
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
		[SerializeField] private Transform DialogOptions;
		[SerializeField] private Button BackButton;
		[SerializeField] private Image Picture;

		[SerializeField] private InteractionUiDisplay InteractionButtonPrefab;

		private Fem fem;
		public void Setup(Fem fem, MainGameManager mgm, DepartmentUiDisplay duid)
		{
			this.fem = fem;

			foreach (Interaction interaction in fem.Interactions)
			{
				var interact = Instantiate(InteractionButtonPrefab);
				interact.Setup(interaction, fem, mgm);
				interact.transform.SetParent(DialogOptions);
			}

			BackButton.onClick.AddListener(() => duid.CloseCurrentFem());
		}

		public void RefreshUiDisplay(MainGameManager mgm)
		{
			Name.text = fem.Name + (fem.Controlled ? "(Controlled)" : "");
			Age.text = $"{fem.Age} years old";
			Ambition.text = $"Ambition: {fem.Ambition}";
			Pride.text = $"Pride: {fem.Pride}";
			Picture.sprite = LoadFemPicture();
			Traits.text = GetTraitsString();

			foreach (var button in DialogOptions.GetComponentsInChildren<InteractionUiDisplay>(true))
				button.RefreshUiDisplay(mgm);
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
			return Resources.Load<Sprite>(Path.Combine("FemPics", fem.Id, DetermineFemPictureId()));
		}

		private string DetermineFemPictureId()
		{
			if (fem.Controlled)
				return "owned";
			else if (fem.Ambition > 70)
				return "angry";
			else if (fem.Ambition > 30)
				return "neutral";
			else
				return "submissive";
		}
	}
}