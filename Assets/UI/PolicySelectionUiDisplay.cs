using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.GameModel.UiDisplayers
{
	public class PolicySelectionUiDisplay : MonoBehaviour, IUiDisplay
	{
		[SerializeField] private Button Button;
		[SerializeField] private TMP_Text Text;

		private Policy policy;
		private Location dept;

		public void Setup(Policy policy, Location dept, LocationUiDisplay uiDisplay)
		{
			this.policy = policy;
			this.dept = dept;
			Button.onClick.AddListener(() =>
			{
				uiDisplay.OpenPolicy(policy);
			});
		}

		public void RefreshUiDisplay(MainGameManager mgm)
		{
			Text.text = $"{policy.Name}";
			if (policy.Active)
				Text.text += $" (Active)";
		}
	}
}