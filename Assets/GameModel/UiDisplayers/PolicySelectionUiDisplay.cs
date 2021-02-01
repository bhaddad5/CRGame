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
		private Department dept;

		public void Setup(Policy policy, Department dept, MainGameManager mgm)
		{
			this.policy = policy;
			this.dept = dept;
			Button.onClick.AddListener(() =>
			{
				policy.Active = true;
				mgm.HandleTurnChange();
			});
		}

		public void RefreshUiDisplay(MainGameManager mgm)
		{
			Text.text = $"{policy.Name}";
			Button.interactable = !policy.Active && dept.Controlled();
		}
	}
}