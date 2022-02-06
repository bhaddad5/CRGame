using System;
using System.Collections;
using System.Collections.Generic;
using Assets.GameModel;
using Assets.GameModel.UiDisplayers;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = System.Random;

public class DialogScreenBindings : MonoBehaviour
{
	[SerializeField] private Image NpcImage;
	[SerializeField] private Image NpcBackground;
	[SerializeField] private GameObject BlackBackground;

	[SerializeField] private TMP_Text SpeakerName;
	[SerializeField] private GameObject SpeakerNameBox;
	[SerializeField] private TMP_Text DialogText;
	[SerializeField] private Image NextDialogImage;

	private DialogEntry dialog;
	private Coroutine runningCoroutine = null;
	private Action dialogsComplete = null;
	private MainGameManager mgm;

	public void Setup(DialogEntry dialog, MainGameManager mgm, Action dialogsComplete)
	{
		this.mgm = mgm;
		this.dialogsComplete = dialogsComplete;
		this.dialog = dialog;

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

		if (dialog.OptionalNpcReference != null)
		{
			NpcImage.sprite = dialog.OptionalNpcReference.GetCurrentPicture().ToSprite();
			if (dialog.CustomNpcImageOptions != null && dialog.CustomNpcImageOptions.Count > 0)
				NpcImage.sprite = dialog.CustomNpcImageOptions[UnityEngine.Random.Range(0, dialog.CustomNpcImageOptions.Count)].ToSprite();
		}
		else
		{
			NpcImage.gameObject.SetActive(false);
		}

		if (dialog.CustomBackground != null)
		{
			NpcBackground.sprite = dialog.CustomBackground.ToSprite();
			dialog.CustomBackgroundNpcLayout.ApplyToRectTransform(NpcImage.GetComponent<RectTransform>());
		}
		else
		{
			NpcBackground.gameObject.SetActive(false);
			BlackBackground.gameObject.SetActive(false);
		}

		foreach (var c in textToShow)
		{
			DialogText.text += c;
			yield return new WaitForSeconds(.02f);
		}

		NextDialogImage.enabled = true;
		runningCoroutine = null;
	}
}
