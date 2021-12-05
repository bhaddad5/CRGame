using System;
using System.Collections;
using System.Collections.Generic;
using Assets.GameModel;
using Assets.GameModel.UiDisplayers;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class PopupUiDisplay : MonoBehaviour, IUiDisplay
{
	[SerializeField] private TMP_Text Title;
	[SerializeField] private RawImage ImageDisplay;
	[SerializeField] private TMP_Text Text;
	[SerializeField] private Button Done;
	[SerializeField] private RenderTexture VideoTexture;
	[SerializeField] private VideoPlayer VideoPlayer;

	private Popup popup;
	private MainGameManager mgm;
	private Action onPopupDone;
	public void Show(Popup popup, int completionCount, MainGameManager mgm, Action onPopupDone)
	{
		this.popup = popup;
		this.mgm = mgm;
		this.onPopupDone = onPopupDone;
		gameObject.SetActive(true);

		Title.text = $"{popup.Title}";
		Text.text = $"{popup.Text}";
		if (popup.Texture != null)
		{
			ImageDisplay.texture = popup.Texture;
		}
		else if (popup.Videos.Count > 0)
		{
			ImageDisplay.texture = VideoTexture;
			UnityEngine.Random.InitState(System.DateTime.Now.Millisecond);
			VideoPlayer.clip = popup.Videos[completionCount % popup.Videos.Count];
		}
	}

	public void ClosePopup()
	{
		gameObject.SetActive(false);
		onPopupDone?.Invoke();
	}
}
