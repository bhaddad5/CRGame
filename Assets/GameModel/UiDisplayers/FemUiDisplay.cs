using System.Collections;
using System.Collections.Generic;
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
		[SerializeField] private Transform DialogOptions;
		[SerializeField] private Button BackButton;

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

			foreach (var button in DialogOptions.GetComponentsInChildren<InteractionUiDisplay>(true))
				button.RefreshUiDisplay(mgm);
		}
	}
}