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
		private Npc _npc;

		public void Setup(Interaction interaction, Npc npc, MainGameManager mgm, DialogDisplayHandler displayHandler)
		{
			this.interaction = interaction;
			this._npc = npc;
			Button.onClick.RemoveAllListeners();
			Button.onClick.AddListener(() =>
			{
				var res = interaction.GetInteractionResult(mgm);

				displayHandler.HandleDisplayDialogs(interaction.Completed, res, res.Dialogs, res.OptionalPopups,() =>
				{
					interaction.Cost.SubtractCost(mgm);
					res.Execute(mgm, npc);
					interaction.Completed++;
					mgm.HandleTurnChange();
				});
			});
			RefreshUiDisplay(mgm, npc);
		}

		public void RefreshUiDisplay(MainGameManager mgm, Npc npc)
		{
			Text.text = $"{interaction.Name}";
			if (!string.IsNullOrEmpty(interaction.Cost.GetCostString()))
				Text.text += $" {interaction.Cost.GetCostString()}";
			if (!string.IsNullOrEmpty(interaction.Cost.GetCostString()) && interaction.PreviewEffect && !string.IsNullOrEmpty(interaction.GetDefaultResult().Effects.GetEffectsString()))
				Text.text += ",";
			if (interaction.PreviewEffect)
				Text.text += $" {interaction.GetDefaultResult().Effects.GetEffectsString()}";
			Button.interactable = interaction.InteractionValid(mgm, npc);
			gameObject.SetActive(interaction.InteractionVisible(mgm, npc));
		}

		public string GetTooltip(MainGameManager mgm)
		{
			if (interaction.InteractionValid(mgm, _npc))
			{
				return null;
			}

			string tooltip = "";

			tooltip += $"{interaction.Requirements.GetInvalidTooltip(mgm, _npc)}";
			if (tooltip.Length > 0)
				tooltip += $"\n"; 
			tooltip += $"\n{interaction.Cost.GetInvalidTooltip(mgm)}";
			
			return "Interaction invalid";
		}
	}
}