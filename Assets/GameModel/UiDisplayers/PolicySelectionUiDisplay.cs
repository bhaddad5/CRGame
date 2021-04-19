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
		[SerializeField] private Image Image;

		private Policy policy;
		private Location dept;

		public void Setup(Policy policy, Location dept, MainGameManager mgm)
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
			Image.sprite = policy.Image;
		}
	}
}