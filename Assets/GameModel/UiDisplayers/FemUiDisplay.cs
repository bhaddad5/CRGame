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
		[SerializeField] private Transform DialogOptions;
		[SerializeField] private Button BackButton;
		[SerializeField] private Image Picture;

		[SerializeField] private InteractionUiDisplay InteractionButtonPrefab;
		[SerializeField] private DialogDisplayHandler DialogHandler;

		private Fem fem;
		public void Setup(Fem fem, MainGameManager mgm, DepartmentUiDisplay duid)
		{
			this.fem = fem;

			foreach (Interaction interaction in fem.Interactions)
			{
				var interact = Instantiate(InteractionButtonPrefab);
				interact.Setup(interaction, fem, mgm, DialogHandler);
				interact.transform.SetParent(DialogOptions);
			}

			DialogHandler.Setup(fem);

			StartCoroutine(RefreshLayout(DialogOptions.gameObject));

			BackButton.onClick.AddListener(() => duid.CloseCurrentFem());
		}

		//From: https://answers.unity.com/questions/1276433/get-layoutgroup-and-contentsizefitter-to-update-th.html
		IEnumerator RefreshLayout(GameObject layout)
		{
			yield return new WaitForFixedUpdate();
			VerticalLayoutGroup vlg = layout.GetComponent<VerticalLayoutGroup>();
			vlg.enabled = false;
			vlg.enabled = true;
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

			foreach (var button in DialogOptions.GetComponentsInChildren<InteractionUiDisplay>(true))
				button.RefreshUiDisplay(mgm, fem);
			
			StartCoroutine(RefreshLayout(DialogOptions.gameObject));
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
			return FemPicManager.GetFemPicFromId(fem.Id, fem.DetermineCurrPictureId());
		}
	}
}