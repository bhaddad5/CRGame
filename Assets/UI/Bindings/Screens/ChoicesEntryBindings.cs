using System.Collections;
using System.Collections.Generic;
using Assets.GameModel;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Assets.GameModel.UiDisplayers
{
	public class ChoicesEntryBindings : MonoBehaviour, ITooltipProvider
	{
		[SerializeField] private Button Button;
		[SerializeField] private TMP_Text Text;

		private Interaction interaction;

		private MainGameManager mgm;

		private NpcDisplayInfo currDisplayInfo;

		public void Setup(Interaction interaction, NpcDisplayInfo currDisplayInfo, MainGameManager mgm)
		{
			this.currDisplayInfo = currDisplayInfo;
			this.interaction = interaction;
			this.mgm = mgm;

			Text.text = $"{interaction.Name}";

			if (interaction.CanFail)
				Text.text += $" ({(int)((1f - interaction.ProbabilityOfFailureResult) * 100)}% chance)";

			if (!string.IsNullOrEmpty(interaction.Cost.GetCostString()))
				Text.text += $" {interaction.Cost.GetCostString()}";
			Button.interactable = interaction.InteractionValid(mgm);
			gameObject.SetActive(interaction.InteractionVisible(mgm));
		}

		public void ExecuteInteraction()
		{
			GameObject.Destroy(gameObject);

			bool succeeded = interaction.GetInteractionSucceeded();
			var res = interaction.GetInteractionResult(succeeded);
			interaction.Cost.SubtractCost(mgm);
			var displayHandler = new InteractionResultDisplayManager();
			displayHandler.DisplayInteractionResult(interaction.Completed, res, !succeeded, currDisplayInfo, mgm, () =>
			{
				res.Execute(mgm);
				if (succeeded)
					interaction.Completed++;
				mgm.HandleTurnChange();
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
	}
}