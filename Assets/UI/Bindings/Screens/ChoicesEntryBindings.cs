using System.Collections;
using System.Collections.Generic;
using Assets.GameModel;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System;

namespace Assets.GameModel.UiDisplayers
{
	public class ChoicesEntryBindings : MonoBehaviour, ITooltipProvider, IPointerEnterHandler
	{
		[SerializeField] private Button Button;
		[SerializeField] private TMP_Text Text;
		[SerializeField] private GameObject NewIndicator;

		private Interaction interaction;

		private MainGameManager mgm;

		private NpcDisplayInfo currDisplayInfo;
		private GameObject choicesScreen;
		private Action choiceCompleted;

		public void Setup(Interaction interaction, NpcDisplayInfo currDisplayInfo, GameObject choicesScreen, MainGameManager mgm, Action choiceCompleted, int index)
		{
			this.currDisplayInfo = currDisplayInfo;
			this.interaction = interaction;
			this.choicesScreen = choicesScreen;
			this.mgm = mgm;
			this.choiceCompleted = choiceCompleted;

			Text.text = $"{index+1}: {interaction.Name}";

			if (interaction.CanFail)
				Text.text += $" ({(int)((1f - interaction.ProbabilityOfFailureResult) * 100)}% chance)";

			NewIndicator.SetActive(interaction.IsNew(mgm));

			if (!string.IsNullOrEmpty(interaction.Cost.GetCostString()))
				Text.text += $" {interaction.Cost.GetCostString()}";
			Button.interactable = interaction.InteractionValid(mgm);
			gameObject.SetActive(interaction.IsVisible(mgm));
		}

		public void ExecuteInteraction()
		{
			GameObject.Destroy(choicesScreen);

			bool succeeded = interaction.GetInteractionSucceeded();
			var res = interaction.GetInteractionResult(succeeded);
			interaction.Cost.SubtractCost(mgm);
			var displayHandler = new InteractionResultDisplayManager();
			displayHandler.DisplayInteractionResult(succeeded ? interaction.Completed : interaction.FailCount, res, !succeeded, currDisplayInfo, mgm, () =>
			{
				res.Execute(mgm);
				if (succeeded)
				{
					interaction.Completed++;
					interaction.TurnCompletedOn = mgm.Data.TurnNumber;
				}
				else
				{
					interaction.FailCount++;
				}
				choiceCompleted();
			});
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

		public void OnPointerEnter(PointerEventData eventData)
		{
			if (interaction.InteractionValid(mgm))
			{
				interaction.New = false;
				NewIndicator.SetActive(false);
			}
		}
	}
}