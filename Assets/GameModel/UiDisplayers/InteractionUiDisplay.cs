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

		public void Setup(Interaction interaction, Fem fem, MainGameManager mgm, DialogDisplayHandler displayHandler)
		{
			this.interaction = interaction;
			this.fem = fem;
			Button.onClick.AddListener(() =>
			{
				var res = interaction.ExecuteInteraction(mgm, fem);

				displayHandler.HandleDisplayDialogs(res.Dialogs);
				mgm.RefreshAllUi();
			});
			RefreshUiDisplay(mgm, fem);
		}

		public void RefreshUiDisplay(MainGameManager mgm, Fem fem)
		{
			Text.text = interaction.Name;
			Button.interactable = interaction.InteractionValid(mgm, fem);
			gameObject.SetActive(interaction.InteractionValid(mgm, fem));
		}
	}
}