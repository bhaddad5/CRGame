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

	private Npc _npc;
	private NpcUiDisplay _npcUiDisplay;
	private MainGameManager mgm;

	public void Setup(Npc npc, NpcUiDisplay npcUiDisplay, MainGameManager mgm)
	{
		this.mgm = mgm;
		this._npc = npc;
		this._npcUiDisplay = npcUiDisplay;
		DialogAreaButton.onClick.RemoveAllListeners();
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
		else if (currPopupsToShow.Count > 0)
		{
			gameObject.SetActive(false);
			var popup = currPopupsToShow[0];
			currPopupsToShow.RemoveAt(0);
			mgm.ShowPopup(popup, completedCount, () => HandleNextDialog());
		}
		else if (currMissionsToShow.Count > 0)
		{
			var missionPopup = new Popup()
			{
				Title = $"Mission Complete: {currMissionsToShow[0].MissionName}",
				Texture = currMissionsToShow[0].MissionImage,
				Text = currMissionsToShow[0].MissionDescription,
			};
			gameObject.SetActive(false);
			currMissionsToShow.RemoveAt(0);
			mgm.ShowPopup(missionPopup, completedCount, () => HandleNextDialog());
		}
		else
		{
			_npcUiDisplay.UnsetImage();
			_npcUiDisplay.UnsetBackground();
			gameObject.SetActive(false);
			dialogsComplete?.Invoke();
			_npcUiDisplay.InteractionsHandler.gameObject.SetActive(true);
		}
	}

	private List<DialogEntry> currDialogsToShow = new List<DialogEntry>();
	private List<Popup> currPopupsToShow = new List<Popup>();
	private List<Mission> currMissionsToShow = new List<Mission>();
	private Coroutine runningCoroutine = null;
	private Action dialogsComplete = null;
	private int completedCount;
	private InteractionResult res;
	public void HandleDisplayDialogs(int completedCount, InteractionResult res, List<DialogEntry> dialogs, List<Popup> popupsToShow, List<Mission> missionsToShow, Action dialogsComplete)
	{
		this.completedCount = completedCount;
		this.res = res;
		this.dialogsComplete = dialogsComplete;

		_npcUiDisplay.InteractionsHandler.gameObject.SetActive(false);

		if (res.CustomBackground != null)
		{
			_npcUiDisplay.SetBackground(res.CustomBackground);
			_npcUiDisplay.SetCustomLayout(res.CustomBackgroundNpcLayout);
		}

		currPopupsToShow = new List<Popup>(popupsToShow);
		currMissionsToShow = new List<Mission>(missionsToShow);
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
		else if (dialog.CurrSpeaker == DialogEntry.Speaker.Npc)
			SpeakerName.text = _npc.FirstName;
		else if (dialog.CurrSpeaker == DialogEntry.Speaker.CustomNpcId)
			SpeakerName.text = dialog.CustomSpeakerReference.FirstName;
		NextDialogImage.enabled = false;
		textToShow = dialog.Text;
		DialogText.text = "";
		
		if (dialog.CustomNpcImageOptions != null && dialog.CustomNpcImageOptions.Count > 0)
			_npcUiDisplay.SetImage(dialog.CustomNpcImageOptions[UnityEngine.Random.Range(0, dialog.CustomNpcImageOptions.Count)]);

		foreach (var c in dialog.Text)
		{
			DialogText.text += c;
			yield return new WaitForSeconds(.02f);
		}

		NextDialogImage.enabled = true;
		runningCoroutine = null;
	}
}
