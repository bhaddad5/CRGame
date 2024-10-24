﻿using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.GameModel.UiDisplayers
{
	public class RegionMapLocationEntryBindings : MonoBehaviour, ITooltipProvider
	{
		[SerializeField] private Button Button;
		[SerializeField] private Image Icon;
		[SerializeField] private TMP_Text Text;
		[SerializeField] private GameObject NewIndicator;
		[SerializeField] private Image TextBackground;
		[SerializeField] private Image ControlledImage;

		private Location loc;
		private Vector2 mainMapSize;
		private bool isQuickAccess;
		private MainGameManager mgm;

		public bool IsNew => (loc.HasNewInteractions(mgm) || loc.HasNewPolicies(mgm)) && loc.IsAccessible(mgm);
		public bool HasNpcs => loc.Npcs.Any(npc => npc.IsVisible(mgm));

		public void Setup(Location dept, RegionMapScreenBindings mainMapUi, Vector2 mainMapSize, MainGameManager mgm, bool isQuickAccess = false)
		{
			this.isQuickAccess = isQuickAccess;
			this.loc = dept;
			this.mainMapSize = mainMapSize;
			this.mgm = mgm;
			Button.onClick.RemoveAllListeners();
			Button.onClick.AddListener(() =>
			{
				mainMapUi.ShowLocation(dept, mgm);
			});
		}

		public void RefreshUiDisplay(MainGameManager mgm)
		{
			Icon.sprite = loc.Icon.ToSprite();
			Text.text = $"{loc.Name}";
			Button.interactable = loc.IsAccessible(mgm);
			Button.gameObject.SetActive(loc.IsVisible(mgm));
			if(!isQuickAccess)
				Button.transform.localPosition = ConvertMapPos(loc.UiPosition);
			NewIndicator.SetActive(IsNew);

			if (loc.IsAccessible(mgm))
				TextBackground.color = Button.colors.normalColor;
			else
				TextBackground.color = Button.colors.disabledColor;

			if(ControlledImage != null)
				ControlledImage.gameObject.SetActive(loc.Controlled);
		}

		private Vector3 ConvertMapPos(Vector2 mapPos)
		{
			return new Vector3(mapPos.x, mapPos.y, 0) - new Vector3(mainMapSize.x/2f, mainMapSize.y/2f, 0);
		}

		public string GetTooltip()
		{
			if (!Button.interactable)
				return "Location currently closed";
			return null;
		}
	}
}