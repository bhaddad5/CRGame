using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.GameModel.UiDisplayers
{
	public class PolicyEntryBindings : MonoBehaviour, ITooltipProvider
	{
		[SerializeField] private TMP_Text Text;
		[SerializeField] private Image Image;
		[SerializeField] private TMP_Text Cost;
		[SerializeField] private TMP_Text Description;
		[SerializeField] private TMP_Text RewardsText;
		[SerializeField] private Transform ActiveIndicator;
		[SerializeField] private Button ActivatePolicyButton;

		private Policy policy;
		private MainGameManager mgm;

		public void Setup(Policy policy, MainGameManager mgm)
		{
			this.policy = policy;
			this.mgm = mgm;
			
			RefreshUiDisplay(mgm);
		}

		public void ActivatePolicy()
		{
			policy.Active = true;
			policy.Cost.SubtractCost(mgm);
			policy.Effect.ExecuteEffect(mgm);
			RefreshUiDisplay(mgm);
		}

		public void RefreshUiDisplay(MainGameManager mgm)
		{
			Text.text = $"{policy.Name}";
			Description.text = $"{policy.Description}";
			Cost.text = $"Cost: {policy.Cost.GetCostString()}";
			ActivatePolicyButton.interactable = !policy.Active && policy.Requirements.RequirementsAreMet(mgm) && policy.Cost.CanAffordCost(mgm);
			Image.sprite = policy.Image.ToSprite();
			ActivatePolicyButton.gameObject.SetActive(!policy.Active);
			ActiveIndicator.gameObject.SetActive(policy.Active);
			RewardsText.text = $"Rewards: {policy.Effect.GetEffectsString()}";
		}

		public string GetTooltip()
		{
			if (!policy.Requirements.RequirementsAreMet(mgm))
				return policy.Requirements.GetInvalidTooltip(mgm);
			if (!policy.Cost.CanAffordCost(mgm))
				return policy.Cost.GetInvalidTooltip(mgm);
			return null;
		}
	}
}