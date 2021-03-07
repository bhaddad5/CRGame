using System.Collections;
using System.Collections.Generic;
using Assets.GameModel;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.GameModel.UiDisplayers
{
	public class InteractionUiDisplay : MonoBehaviour, IUiDisplay, ITooltipProvider
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
				var res = interaction.GetInteractionResult(mgm, fem);

				displayHandler.HandleDisplayDialogs(res.Dialogs, res.OptionalPopup, () =>
				{
					res.Execute(mgm, fem);
					interaction.ExecuteMissionIfRelevant(mgm, fem);
					mgm.HandleTurnChange();
				});
			});
			RefreshUiDisplay(mgm, fem);
		}

		public void RefreshUiDisplay(MainGameManager mgm, Fem fem)
		{
			Text.text = $"{interaction.Name}";
			if (!string.IsNullOrEmpty(interaction.Cost.GetCostString()))
				Text.text += $" - {interaction.Cost.GetCostString()}";
			Button.interactable = interaction.InteractionValid(mgm, fem);
			gameObject.SetActive(interaction.InteractionVisible(mgm, fem));
		}

		public string GetTooltip(MainGameManager mgm)
		{
			if (interaction.InteractionValid(mgm, fem))
			{
				return null;
			}

			string tooltip = "";

			foreach (var tt in interaction.Requirements.GetInvalidTooltips(mgm, fem))
				tooltip += $"\n{tt}";
			foreach (var tt in interaction.Cost.GetInvalidTooltips(mgm))
				tooltip += $"\n{tt}";

			if (tooltip.Length > 0)
				return tooltip.Substring(1);
			return "Interaction invalid";
		}
	}
}