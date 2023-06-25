using System.Collections;
using System.Collections.Generic;
using Assets.GameModel;
using Assets.GameModel.UiDisplayers;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TrophyEntryBindings : MonoBehaviour
{
	[SerializeField] private Image TrophyImage;
	[SerializeField] private TMP_Text TrophyName;

	private Trophy trophy;
	private MainGameManager mgm;
	public void Setup(Trophy trophy, MainGameManager mgm)
	{
		this.mgm = mgm;
		this.trophy = trophy;

		TrophyImage.sprite = trophy.Image.ToSprite();
		TrophyName.text = trophy.Name;
	}

	public void ViewTrophyPopup()
	{
		var popup = new Popup()
		{
			Title = $"{trophy.Name}",
			Textures = new List<Texture2D>() { trophy.Image },
			Text = trophy.Description,
		};

		var popupParent = GameObject.Instantiate(UiPrefabReferences.Instance.PopupOverlayParent, transform);
		GameObject.Instantiate(UiPrefabReferences.Instance.GetPrefabByName("Popup Display"), popupParent.transform).GetComponent<PopupBindings>().Setup(popup, 0, mgm, null);
	}
}
