using System;
using System.Collections;
using System.Collections.Generic;
using Assets.GameModel;
using Assets.GameModel.UiDisplayers;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogDisplayHandler : MonoBehaviour
{
	[SerializeField] private TMP_Text SpeakerName;
	[SerializeField] private TMP_Text DialogText;
	[SerializeField] private Button DialogAreaButton;
	[SerializeField] private Image NextDialogImage;

	private Fem fem;
	private FemUiDisplay femUiDisplay;
	private MainGameManager mgm;

	public void Setup(Fem fem, FemUiDisplay femUiDisplay, MainGameManager mgm)
	{
		this.mgm = mgm;
		this.fem = fem;
		this.femUiDisplay = femUiDisplay;
		DialogAreaButton.onClick.AddListener(() =>
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
				HandleNextDialog();
			}
		});
		gameObject.SetActive(false);
	}

	private void HandleNextDialog()
	{
		if (currDialogsToShow.Count > 0)
		{
			runningCoroutine = StartCoroutine(ShowDialog());
		}
		else if (popupToShow != null)
		{
			gameObject.SetActive(false);
			mgm.ShowPopup(popupToShow, () => HandleNextDialog());
			popupToShow = null;
		}
		else
		{
			femUiDisplay.UnsetImage();
			gameObject.SetActive(false);
			dialogsComplete?.Invoke();
			femUiDisplay.InteractionsHandler.gameObject.SetActive(true);
		}
	}

	private Popup popupToShow = null;
	private List<DialogEntry> currDialogsToShow = new List<DialogEntry>();
	private Coroutine runningCoroutine = null;
	private Action dialogsComplete = null;
	public void HandleDisplayDialogs(List<DialogEntry> dialogs, Popup popupToShow, Action dialogsComplete)
	{
		femUiDisplay.InteractionsHandler.gameObject.SetActive(false);

		this.dialogsComplete = dialogsComplete;
		this.popupToShow = popupToShow;
		currDialogsToShow = new List<DialogEntry>(dialogs);
		gameObject.SetActive(true);
		if (runningCoroutine != null)
			StopCoroutine(runningCoroutine);

		HandleNextDialog();
	}

	private string textToShow = "";
	private IEnumerator ShowDialog()
	{
		var dialog = currDialogsToShow[0];
		currDialogsToShow.RemoveAt(0);

		SpeakerName.text = "";
		if (dialog.CurrSpeaker == DialogEntry.Speaker.Player)
			SpeakerName.text = "Player";
		else if (dialog.CurrSpeaker == DialogEntry.Speaker.Fem)
			SpeakerName.text = fem.FirstName;
		else if (dialog.CurrSpeaker == DialogEntry.Speaker.CustomFemId)
			SpeakerName.text = mgm.Data.GetFemById(dialog.CustomSpeakerId).FirstName;
		NextDialogImage.enabled = false;
		textToShow = dialog.Text;
		DialogText.text = "";
		if (dialog.NpcImage != "")
			femUiDisplay.SetImage(dialog.NpcImage);

		foreach (var c in dialog.Text)
		{
			DialogText.text += c;
			yield return new WaitForSeconds(.02f);
		}

		NextDialogImage.enabled = true;
		runningCoroutine = null;
	}
}
