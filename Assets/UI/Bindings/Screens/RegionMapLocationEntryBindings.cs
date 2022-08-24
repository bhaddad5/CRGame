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

		private Location loc;
		private Vector2 mainMapSize;
		private bool isQuickAccess;

		public void Setup(Location dept, RegionMapScreenBindings mainMapUi, MainGameManager mgm, bool isQuickAccess = false)
		{
			this.isQuickAccess = isQuickAccess;
			this.loc = dept;
			Button.onClick.RemoveAllListeners();
			Button.onClick.AddListener(() =>
			{
				mainMapUi.ShowLocation(dept, mgm);
			});
			mainMapSize = mainMapUi.GetComponentInChildren<RectTransform>().sizeDelta;
		}

		public void RefreshUiDisplay(MainGameManager mgm)
		{
			Icon.sprite = loc.Icon.ToSprite();
			Text.text = $"{loc.Name}";
			Button.interactable = loc.IsAccessible(mgm);
			Button.gameObject.SetActive(loc.IsVisible(mgm));
			if(!isQuickAccess)
				Button.transform.localPosition = ConvertMapPos(loc.UiPosition);
			NewIndicator.SetActive(loc.HasNewInteractions(mgm) && loc.IsAccessible(mgm));

			if (loc.IsAccessible(mgm))
				TextBackground.color = Button.colors.normalColor;
			else
				TextBackground.color = Button.colors.disabledColor;
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