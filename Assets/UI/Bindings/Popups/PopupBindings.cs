using System;
using System.Collections;
using System.Collections.Generic;
using Assets.GameModel;
using Assets.GameModel.UiDisplayers;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class PopupBindings : MonoBehaviour
{
	[SerializeField] private TMP_Text Title;
	[SerializeField] private RawImage ImageDisplay;
	[SerializeField] private TMP_Text Text;
	[SerializeField] private RenderTexture VideoTexture;
	[SerializeField] private VideoPlayer VideoPlayer;

	private Action onPopupDone;
	public void Setup(Popup popup, int completionCount, MainGameManager mgm, Action onPopupDone)
	{
		this.onPopupDone = onPopupDone;
		gameObject.SetActive(true);

		Title.text = $"{UiDisplayHelpers.ApplyDynamicValuesToString(popup.Title, mgm)}";
		Title.gameObject.SetActive(!String.IsNullOrEmpty(popup.Title));

		Text.text = $"{UiDisplayHelpers.ApplyDynamicValuesToString(popup.Text, mgm)}";
		Text.gameObject.SetActive(!String.IsNullOrEmpty(popup.Text));

		ImageDisplay.gameObject.SetActive(popup.Texture != null);
		VideoPlayer.gameObject.SetActive(popup.Videos?.Count > 0);

		if (popup.Texture != null)
		{
			ImageDisplay.texture = popup.Texture;
		}
		
		if (popup.Videos?.Count > 0)
		{
			VideoPlayer.GetComponent<RawImage>().texture = VideoTexture;
			UnityEngine.Random.InitState(System.DateTime.Now.Millisecond);
			VideoPlayer.clip = popup.Videos[completionCount % popup.Videos.Count];
		}
	}

	public void ClosePopup()
	{
		GameObject.Destroy(transform.parent.gameObject);
		onPopupDone?.Invoke();
	}
}
