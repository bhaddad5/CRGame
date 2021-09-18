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
	public class LocationSelectionUiDisplay : MonoBehaviour, IUiDisplay, ITooltipProvider
	{
		[SerializeField] private Button Button;
		[SerializeField] private Image Icon;
		[SerializeField] private TMP_Text Text;

		private Location dept;
		private Vector2 mainMapSize;

		public void Setup(Location dept, MainMapUiDisplay mainMapUi, MainGameManager mgm)
		{
			this.dept = dept;
			Button.onClick.RemoveAllListeners();
			Button.onClick.AddListener(() =>
			{
				mainMapUi.ShowDepartment(dept, mgm);
			});
			mainMapSize = mainMapUi.GetComponentInChildren<RectTransform>().sizeDelta;
		}

		public void RefreshUiDisplay(MainGameManager mgm)
		{
			Icon.sprite = dept.Icon.ToSprite();
			Text.text = $"{dept.Name}";
			var dayOfWeek = mgm.GetDateFromTurnNumber().DayOfWeek;
			Button.interactable = !dept.ClosedOnWeekends || (dayOfWeek != DayOfWeek.Saturday && dayOfWeek != DayOfWeek.Sunday);
			Button.gameObject.SetActive(dept.Accessible);
			Button.transform.localPosition = ConvertMapPos(dept.UiPosition);
		}

		private Vector3 ConvertMapPos(Vector2 mapPos)
		{
			return new Vector3(mapPos.x, mapPos.y, 0) - new Vector3(mainMapSize.x/2f, mainMapSize.y/2f, 0);
		}

		public string GetTooltip(MainGameManager mgm)
		{
			if (!Button.interactable)
				return "Office locations are closed on weekends";
			return null;
		}
	}
}