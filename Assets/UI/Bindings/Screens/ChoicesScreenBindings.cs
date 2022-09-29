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

public class ChoicesScreenBindings : MonoBehaviour
{
	[SerializeField] private NpcVisualDisplay NpcDisplay;
	[SerializeField] private ChoicesEntryBindings ChoicePrefab;
	[SerializeField] private Transform ChoicesParent;

	private MainGameManager mgm;

	public void Setup(List<Interaction> choices, NpcDisplayInfo currDisplayInfo, MainGameManager mgm, Action dialogsComplete)
	{
		this.mgm = mgm;

		NpcDisplay.DisplayNpcInfo(currDisplayInfo);

		for (int i = 0; i < choices.Count; i++)
		{
			Instantiate(ChoicePrefab, ChoicesParent).Setup(choices[i], currDisplayInfo, gameObject, mgm, dialogsComplete, i);
		}
	}
}
