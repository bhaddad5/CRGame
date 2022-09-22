using System;
using System.Collections;
using System.Collections.Generic;
using Assets.GameModel;
using Assets.GameModel.UiDisplayers;
using Assets.UI_System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = System.Random;

public class DialogScreenBindings : MonoBehaviour
{
	[SerializeField] private NpcVisualDisplay NpcDisplay;

	[SerializeField] private TMP_Text SpeakerName;
	[SerializeField] private GameObject SpeakerNameBox;
	[SerializeField] private TMP_Text DialogText;
	[SerializeField] private Image NextDialogImage;

	private DialogEntry dialog;
	private Coroutine runningCoroutine = null;
	private Action dialogsComplete = null;
	private MainGameManager mgm;

	public void Setup(DialogEntry dialog, NpcDisplayInfo currDisplayInfo, MainGameManager mgm, Action dialogsComplete)
	{
		this.mgm = mgm;
		this.dialogsComplete = dialogsComplete;
		this.dialog = dialog;

		if (dialog.OptionalNpcReference != null)
		{
			currDisplayInfo.Picture = dialog.OptionalNpcReference.GetCurrentPicture();
			if (dialog.CustomNpcImageOptions != null && dialog.CustomNpcImageOptions.Count > 0)
				currDisplayInfo.Picture = dialog.CustomNpcImageOptions[UnityEngine.Random.Range(0, dialog.CustomNpcImageOptions.Count)];
		}

		if (dialog.CustomBackground != null)
		{
			currDisplayInfo.Background = dialog.CustomBackground;
			currDisplayInfo.Layout = dialog.CustomBackgroundNpcLayout;
		}

		NpcDisplay.DisplayNpcInfo(currDisplayInfo);

		runningCoroutine = StartCoroutine(TypeOutDialog());
	}
	
	public void SkipOrClose()
	{
		if (runningCoroutine != null)
		{
			StopCoroutine(runningCoroutine);
			runningCoroutine = null;
			DialogText.text = textToShow;
			NextDialogImage.enabled = true;
		}
		else
		{
			dialogsComplete?.Invoke();
			GameObject.Destroy(gameObject);
		}
	}

	private string textToShow = "";
	private IEnumerator TypeOutDialog()
	{
		SpeakerName.text = "";
		if (dialog.CurrSpeaker == DialogEntry.Speaker.Player)
			SpeakerName.text = $"{mgm.Data.FirstName}";
		else if (dialog.CurrSpeaker == DialogEntry.Speaker.Npc)
			SpeakerName.text = dialog.OptionalNpcReference.FirstName;

		SpeakerNameBox.SetActive(dialog.CurrSpeaker != DialogEntry.Speaker.Narrator);

		NextDialogImage.enabled = false;
		textToShow = UiDisplayHelpers.ApplyDynamicValuesToString(dialog.Text, mgm);
		DialogText.text = "";
		
		foreach (var c in textToShow)
		{
			DialogText.text += c;
			yield return new WaitForSeconds(.01f);
		}

		NextDialogImage.enabled = true;
		runningCoroutine = null;
	}
}
