﻿using System.Collections;
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

		public void Setup(Interaction interaction, MainGameManager mgm, NpcUiDisplay npcUiDisplay)
		{
			this.interaction = interaction;
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
					res.Execute(mgm);
					if(succeeded)
						interaction.Completed++;
					mgm.HandleTurnChange();

					npcUiDisplay.UnsetImage();
					npcUiDisplay.UnsetBackground();
					npcUiDisplay.InteractionsHandler.gameObject.SetActive(true);
				}, npcUiDisplay);
			});
			RefreshUiDisplay(mgm);
		}

		public void RefreshUiDisplay(MainGameManager mgm)
		{
			Text.text = $"{CategoryToString(interaction.Category)}: {interaction.Name}";

			if (interaction.CanFail)
				Text.text += $" ({(int)((1f - interaction.ProbabilityOfFailureResult) * 100)}% chance)";

			if (!string.IsNullOrEmpty(interaction.Cost.GetCostString()))
				Text.text += $" {interaction.Cost.GetCostString()}";
			Button.interactable = interaction.InteractionValid(mgm);
			gameObject.SetActive(interaction.InteractionVisible(mgm));
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
			if (interaction.InteractionValid(mgm))
				return null;
			
			var reqTooltip = interaction.Requirements.GetInvalidTooltip(mgm);
			var costTooltip = interaction.Cost.GetInvalidTooltip(mgm);

			var tooltip = $"{reqTooltip}\n{costTooltip}";

			if (tooltip.StartsWith("\n"))
				tooltip = tooltip.Substring(1);
			if (tooltip.EndsWith("\n"))
				tooltip = tooltip.Substring(0, tooltip.Length - 1);

			return tooltip;
		}
	}
}