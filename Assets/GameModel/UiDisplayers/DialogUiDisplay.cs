using System.Collections;
using System.Collections.Generic;
using Assets.GameModel;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogUiDisplay : MonoBehaviour
{
	[SerializeField] private TMP_Text Text;

	private DialogEntry dialog;

	public void Setup(DialogEntry dialog)
	{
		this.dialog = dialog;
	}

	public void RefreshUiDisplay(MainGameManager mgm)
	{
		Text.text = dialog.Text;

		GetComponent<VerticalLayoutGroup>().childAlignment = dialog.IsPlayer ? TextAnchor.UpperLeft : TextAnchor.UpperRight;

		StartCoroutine(RefreshLayout(gameObject));
	}

	//From: https://answers.unity.com/questions/1276433/get-layoutgroup-and-contentsizefitter-to-update-th.html
	IEnumerator RefreshLayout(GameObject layout)
	{
		yield return new WaitForFixedUpdate();
		VerticalLayoutGroup vlg = layout.GetComponent<VerticalLayoutGroup>();
		vlg.enabled = false;
		vlg.enabled = true;
	}
}
