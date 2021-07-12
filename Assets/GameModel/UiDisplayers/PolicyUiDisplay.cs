using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using Assets.GameModel;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.GameModel.UiDisplayers
{
	public class PolicyUiDisplay : MonoBehaviour, IUiDisplay
	{
		[SerializeField] private TMP_Text Text;
		[SerializeField] private Image Image;
		[SerializeField] private TMP_Text Description;
		[SerializeField] private TMP_Text RewardsText;
		[SerializeField] private Transform ActiveIndicator;
		[SerializeField] private Button ActivatePolicyButton;

		private Policy policy;
		private Location loc;

		public void Setup(Policy policy, Location loc, MainGameManager mgm)
		{
			this.policy = policy;
			this.loc = loc;

			ActivatePolicyButton.onClick.AddListener(() => { policy.Active = true; });
			RefreshUiDisplay(mgm);
		}

		public void RefreshUiDisplay(MainGameManager mgm)
		{
			Text.text = $"{policy.Name}";
			Description.text = $"{policy.Description}";
			ActivatePolicyButton.interactable = !policy.Active && loc.Controlled();
			Image.sprite = policy.Image;
			ActivatePolicyButton.gameObject.SetActive(!policy.Active);
			ActiveIndicator.gameObject.SetActive(policy.Active);
			RewardsText.text = $"Rewards: {policy.Effects.GetEffectsString()}";
		}
	}
}