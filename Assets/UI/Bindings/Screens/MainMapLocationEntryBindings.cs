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
	public class MainMapLocationEntryBindings : MonoBehaviour, ITooltipProvider
	{
		[SerializeField] private Button Button;
		[SerializeField] private Image Icon;
		[SerializeField] private TMP_Text Text;
		[SerializeField] private GameObject NewIndicator;

		private Location loc;
		private Vector2 mainMapSize;

		public void Setup(Location dept, MainMapScreenBindings mainMapUi, MainGameManager mgm)
		{
			this.loc = dept;
			Button.onClick.RemoveAllListeners();
			Button.onClick.AddListener(() =>
			{
				mainMapUi.ShowDepartment(dept, mgm);
			});
			mainMapSize = mainMapUi.GetComponentInChildren<RectTransform>().sizeDelta;
		}

		public void RefreshUiDisplay(MainGameManager mgm)
		{
			Icon.sprite = loc.Icon.ToSprite();
			Text.text = $"{loc.Name}";
			var dayOfWeek = mgm.GetDateFromTurnNumber().DayOfWeek;
			Button.interactable = !loc.ClosedOnWeekends || (dayOfWeek != DayOfWeek.Saturday && dayOfWeek != DayOfWeek.Sunday);
			Button.gameObject.SetActive(loc.IsVisible(mgm));
			Button.transform.localPosition = ConvertMapPos(loc.UiPosition);
			NewIndicator.SetActive(loc.HasNewInteractions(mgm));
		}

		private Vector3 ConvertMapPos(Vector2 mapPos)
		{
			return new Vector3(mapPos.x, mapPos.y, 0) - new Vector3(mainMapSize.x/2f, mainMapSize.y/2f, 0);
		}

		public string GetTooltip()
		{
			if (!Button.interactable)
				return "Office locations are closed on weekends";
			return null;
		}
	}
}