using System.Collections;
using System.Collections.Generic;
using Assets.GameModel;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.GameModel.UiDisplayers
{
	public class NpcInteractionEntryBindings : MonoBehaviour, ITooltipProvider
	{
		[SerializeField] private Button Button;
		[SerializeField] private TMP_Text Text;

		private Interaction interaction;

		private NpcScreenBindings npcUiDisplay;
		private MainGameManager mgm;

		public void Setup(Interaction interaction, MainGameManager mgm, NpcScreenBindings npcUiDisplay)
		{
			this.interaction = interaction;
			this.npcUiDisplay = npcUiDisplay;
			this.mgm = mgm;

			Text.text = $"{CategoryToString(interaction.Category)}: {interaction.Name}";

			if (interaction.CanFail)
				Text.text += $" ({(int)((1f - interaction.ProbabilityOfFailureResult) * 100)}% chance)";

			if (!string.IsNullOrEmpty(interaction.Cost.GetCostString()))
				Text.text += $" {interaction.Cost.GetCostString()}";
			Button.interactable = interaction.InteractionValid(mgm);
			gameObject.SetActive(interaction.InteractionVisible(mgm));

		}

		public void ExecuteInteraction()
		{
			npcUiDisplay.InteractionsParent.parent.parent.gameObject.SetActive(false);

			bool succeeded = interaction.GetInteractionSucceeded();
			var res = interaction.GetInteractionResult(succeeded);
			interaction.Cost.SubtractCost(mgm);
			var displayHandler = new InteractionResultDisplayManager();
			displayHandler.DisplayInteractionResult(mgm, interaction.Completed, res, !succeeded, () =>
			{
				res.Execute(mgm);
				if (succeeded)
					interaction.Completed++;
				mgm.HandleTurnChange();

				npcUiDisplay.UnsetImage();
				npcUiDisplay.UnsetBackground();
				npcUiDisplay.InteractionsParent.parent.parent.gameObject.SetActive(true);
			}, npcUiDisplay);
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

		public string GetTooltip()
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