using System.Collections;
using System.Collections.Generic;
using Assets.GameModel;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.GameModel.UiDisplayers
{
	public class InteractionUiDisplay : MonoBehaviour, IUiDisplay
	{
		[SerializeField] private Button Button;
		[SerializeField] private TMP_Text Text;

		private Interaction interaction;
		private Fem fem;

		public void Setup(Interaction interaction, Fem fem, MainGameManager mgm)
		{
			this.interaction = interaction;
			this.fem = fem;
			Button.onClick.AddListener(() =>
			{
				interaction.ExecuteInteraction(mgm, fem);
				mgm.RefreshAllUi();
			});
		}

		public void RefreshUiDisplay(MainGameManager mgm)
		{
			Text.text = interaction.Name;
			Button.interactable = interaction.InteractionValid(mgm, fem);
		}
	}
}