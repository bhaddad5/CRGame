using System;
using System.Collections;
using System.Collections.Generic;
using Assets.GameModel;
using Assets.GameModel.UiDisplayers;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Assets.GameModel.UiDisplayers
{
	public class DepartmentSelectionUiDisplay : MonoBehaviour, IUiDisplay, ITooltipProvider
	{
		[SerializeField] private Button Button;
		[SerializeField] private Image Icon;
		[SerializeField] private TMP_Text Text;

		private Department dept;

		public void Setup(Department dept, MainMapUiDisplay mainMapUi, MainGameManager mgm)
		{
			this.dept = dept;
			Button.onClick.AddListener(() =>
			{
				mainMapUi.ShowDepartment(dept, mgm);
			});
			Button.transform.position = new Vector3(dept.UiPosition.x, dept.UiPosition.y, 0);
		}

		public void RefreshUiDisplay(MainGameManager mgm)
		{
			Icon.sprite = dept.Icon;
			Text.text = $"{dept.Name}";
			var dayOfWeek = mgm.GetDateFromTurnNumber().DayOfWeek;
			Button.interactable = !dept.ClosedOnWeekends || (dayOfWeek != DayOfWeek.Saturday && dayOfWeek != DayOfWeek.Sunday);
			Button.gameObject.SetActive(dept.Accessible);
		}

		public string GetTooltip(MainGameManager mgm)
		{
			if (!Button.interactable)
				return "Office locations are closed on weekends";
			return null;
		}
	}
}