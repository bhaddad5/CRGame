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

	private GameObject popupParent;
	public void ViewTrophyPopup()
	{
		var popup = new Popup()
		{
			Title = $"{trophy.Name}",
			Textures = new List<Texture2D>() { trophy.Image },
			Text = trophy.Description,
		};

		popupParent = GameObject.Instantiate(UiPrefabReferences.Instance.PopupOverlayParent);
		GameObject.Instantiate(UiPrefabReferences.Instance.GetPrefabByName("Popup Display"), popupParent.transform).GetComponent<PopupBindings>().Setup(popup, 0, mgm, null);
	}

	private void OnDestroy()
	{
		if (popupParent != null)
		{
			GameObject.Destroy(popupParent);
		}
	}
}
