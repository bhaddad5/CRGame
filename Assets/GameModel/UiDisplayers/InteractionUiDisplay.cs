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
			Button.onClick.AddListener(() =>
			{
				var res = interaction.GetInteractionResult(mgm, npc);
				var missions = interaction.GetRelevantMissions(mgm, npc);

				displayHandler.HandleDisplayDialogs(res.Dialogs, res.OptionalPopup, missions,() =>
				{
					res.Execute(mgm, npc);
					interaction.ExecuteMissionIfRelevant(mgm, npc);
					mgm.HandleTurnChange();
				});
			});
			RefreshUiDisplay(mgm, npc);
		}

		public void RefreshUiDisplay(MainGameManager mgm, Npc npc)
		{
			Text.text = $"{interaction.Name}";
			if (!string.IsNullOrEmpty(interaction.Cost.GetCostString()))
				Text.text += $" ({interaction.Cost.GetCostString()})";
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

			foreach (var tt in interaction.Requirements.GetInvalidTooltips(mgm, _npc))
				tooltip += $"\n{tt}";
			foreach (var tt in interaction.Cost.GetInvalidTooltips(mgm))
				tooltip += $"\n{tt}";

			if (tooltip.Length > 0)
				return tooltip.Substring(1);
			return "Interaction invalid";
		}
	}
}