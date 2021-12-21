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

		public void Setup(Interaction interaction, Npc npc, MainGameManager mgm, NpcUiDisplay npcUiDisplay)
		{
			this.interaction = interaction;
			this._npc = npc;
			Button.onClick.RemoveAllListeners();
			Button.onClick.AddListener(() =>
			{
				npcUiDisplay.InteractionsHandler.gameObject.SetActive(false);

				bool succeeded = interaction.GetInteractionSucceeded();
				var res = interaction.GetInteractionResult(succeeded);
				interaction.Cost.SubtractCost(mgm);
				var displayHandler = new InteractionResultDisplayManager();
				displayHandler.DisplayInteractionResult(mgm, interaction.Completed, res, !succeeded, () =>
				{
					res.Execute(mgm, npc);
					if(succeeded)
						interaction.Completed++;
					mgm.HandleTurnChange();

					npcUiDisplay.UnsetImage();
					npcUiDisplay.UnsetBackground();
					npcUiDisplay.InteractionsHandler.gameObject.SetActive(true);
				}, npc, npcUiDisplay);
			});
			RefreshUiDisplay(mgm, npc);
		}

		public void RefreshUiDisplay(MainGameManager mgm, Npc npc)
		{
			Text.text = $"{CategoryToString(interaction.Category)}: {interaction.Name}";

			if (interaction.CanFail)
				Text.text += $" ({(int)((1f - interaction.ProbabilityOfFailureResult) * 100)}% chance)";

			if (!string.IsNullOrEmpty(interaction.Cost.GetCostString()))
				Text.text += $" {interaction.Cost.GetCostString()}";
			Button.interactable = interaction.InteractionValid(mgm, npc);
			gameObject.SetActive(interaction.InteractionVisible(mgm, npc));
		}

		private string CategoryToString(Interaction.InteractionCategory category)
		{
			switch (category)
			{
				case Interaction.InteractionCategory.OfficePolitics:
					return "OFFICE POLITICS";
				case Interaction.InteractionCategory.Challenge:
					return "CHALLENGE";
				case Interaction.InteractionCategory.Conversation:
					return "CONVERSATION";
				case Interaction.InteractionCategory.Fun:
					return "FUN";
				case Interaction.InteractionCategory.Projects:
					return "PROJECT";
				case Interaction.InteractionCategory.Socialize:
					return "SOCIALIZE";
				case Interaction.InteractionCategory.Surveillance:
					return "SURVEILLANCE";
				case Interaction.InteractionCategory.Train:
					return "TRAIN";
			}

			return "";
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