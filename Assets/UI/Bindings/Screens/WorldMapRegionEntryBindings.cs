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
	public class WorldMapRegionEntryBindings : MonoBehaviour
	{
		[SerializeField] private Button Button;
		[SerializeField] private Image Icon;
		[SerializeField] private TMP_Text Text;
		[SerializeField] private GameObject NewIndicator;
		[SerializeField] private Image TextBackground;

		private Region region;
		private Vector2 mainMapSize;

		public void Setup(Region region, WorldMapScreenBindings mainMapUi, MainGameManager mgm)
		{
			this.region = region;
			Button.onClick.RemoveAllListeners();
			Button.onClick.AddListener(() =>
			{
				mainMapUi.ShowRegion(region, mgm);
			});
			mainMapSize = mainMapUi.GetComponentInChildren<RectTransform>().sizeDelta;
		}

		public void RefreshUiDisplay(MainGameManager mgm)
		{
			Icon.sprite = region.Icon.ToSprite();
			Text.text = $"{region.Name}";
			Button.gameObject.SetActive(region.IsVisible(mgm));
			Button.transform.localPosition = ConvertMapPos(region.UiPosition);
			NewIndicator.SetActive(region.HasNewInteractions(mgm));
		}

		private Vector3 ConvertMapPos(Vector2 mapPos)
		{
			return new Vector3(mapPos.x, mapPos.y, 0) - new Vector3(mainMapSize.x / 2f, mainMapSize.y / 2f, 0);
		}
	}
}