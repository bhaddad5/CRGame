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

	public void Setup(Fem fem, FemUiDisplay femUiDisplay)
	{
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
				if(currDialogsToShow.Count == 0)
					gameObject.SetActive(false);
				else
					runningCoroutine = StartCoroutine(ShowDialog());
			}
		});
		gameObject.SetActive(false);
	}

	private List<DialogEntry> currDialogsToShow = new List<DialogEntry>();
	private Coroutine runningCoroutine = null;
	public void HandleDisplayDialogs(List<DialogEntry> dialogs)
	{
		gameObject.SetActive(true);
		currDialogsToShow = new List<DialogEntry>(dialogs);
		if(runningCoroutine != null)
			StopCoroutine(runningCoroutine);
		runningCoroutine = StartCoroutine(ShowDialog());
	}

	private string textToShow = "";
	private IEnumerator ShowDialog()
	{
		var dialog = currDialogsToShow[0];
		currDialogsToShow.RemoveAt(0);

		SpeakerName.text = dialog.IsPlayer ? "Player" : fem.FirstName;
		DialogText.text = "";
		NextDialogImage.enabled = false;
		textToShow = dialog.Text;
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
