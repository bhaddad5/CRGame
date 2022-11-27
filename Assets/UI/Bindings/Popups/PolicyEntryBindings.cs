using System.Collections.Generic;
using Assets.UI_System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Assets.GameModel.UiDisplayers
{
	public class PolicyEntryBindings : MonoBehaviour, ITooltipProvider, IPointerEnterHandler
	{
		[SerializeField] private TMP_Text Text;
		[SerializeField] private Image Image;
		[SerializeField] private TMP_Text Cost;
		[SerializeField] private TMP_Text Description;
		[SerializeField] private TMP_Text RewardsText;
		[SerializeField] private Transform ActiveIndicator;
		[SerializeField] private Button ActivatePolicyButton;
		[SerializeField] private GameObject NewIndicator;

		private Policy policy;
		private MainGameManager mgm;

		public void Setup(Policy policy, MainGameManager mgm)
		{
			this.policy = policy;
			this.mgm = mgm;
			NewIndicator.SetActive(policy.IsNew(mgm));
			
			RefreshUiDisplay(mgm);
		}

		public void ActivatePolicy()
		{
			policy.New = false;

			AudioHandler.Instance.PlayOverridingMusicTrack(mgm.PolicyAudioClip);
			policy.Active = true;
			policy.Cost.SubtractCost(mgm);
			policy.Effect.ExecuteEffect(mgm);
			RefreshUiDisplay(mgm);
			mgm.HandleTurnChange();
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

		public void OnPointerEnter(PointerEventData eventData)
		{
			if (policy.IsNew(mgm))
			{
				policy.New = false;
				NewIndicator.SetActive(false);
			}
		}
	}
}