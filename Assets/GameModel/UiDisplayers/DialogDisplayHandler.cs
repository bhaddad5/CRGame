using System;
using System.Collections;
using System.Collections.Generic;
using Assets.GameModel;
using Assets.GameModel.UiDisplayers;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = System.Random;

public class DialogDisplayHandler : MonoBehaviour
{
	[SerializeField] private TMP_Text SpeakerName;
	[SerializeField] private TMP_Text DialogText;
	[SerializeField] private Button DialogAreaButton;
	[SerializeField] private Image NextDialogImage;

	private DialogEntry dialog;
	private Coroutine runningCoroutine = null;
	private Action dialogsComplete = null;
	private Npc _npc;
	private NpcUiDisplay npcDisplay;

	public void ShowDialog(DialogEntry dialog, Action dialogsComplete, Npc npc = null, NpcUiDisplay npcDisplay = null)
	{
		this._npc = npc;
		this.dialogsComplete = dialogsComplete;
		this.npcDisplay = npcDisplay;

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
			SpeakerName.text = "Player";
		else if (dialog.CurrSpeaker == DialogEntry.Speaker.Npc)
			SpeakerName.text = _npc.FirstName;
		else if (dialog.CurrSpeaker == DialogEntry.Speaker.CustomNpcId)
			SpeakerName.text = dialog.CustomSpeakerReference.FirstName;
		NextDialogImage.enabled = false;
		textToShow = dialog.Text;
		DialogText.text = "";
		
		if (npcDisplay != null && dialog.CustomNpcImageOptions != null && dialog.CustomNpcImageOptions.Count > 0)
			npcDisplay.SetImage(dialog.CustomNpcImageOptions[UnityEngine.Random.Range(0, dialog.CustomNpcImageOptions.Count)]);

		foreach (var c in dialog.Text)
		{
			DialogText.text += c;
			yield return new WaitForSeconds(.02f);
		}

		NextDialogImage.enabled = true;
		runningCoroutine = null;
	}
}
