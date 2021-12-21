using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Assets.GameModel;
using Assets.GameModel.UiDisplayers;
using UnityEngine;
using UnityEngine.UI;

public class InteractionsDisplayHandler : MonoBehaviour
{
	[SerializeField] private Transform InteractionsParent;

	[SerializeField] private InteractionUiDisplay InteractionPrefab;

	private List<Interaction> allInteractions;

	public void Setup(List<Interaction> interactions, MainGameManager mgm, NpcUiDisplay npcUiDisplay)
	{
		allInteractions = new List<Interaction>(interactions);
		allInteractions.RemoveAll(i => i == null);
		allInteractions.Sort((i1, i2) => ((int)i1.Category).CompareTo((int)i2.Category));
		
		foreach (var interaction in allInteractions)
		{
			if (!interaction.InteractionVisible(mgm))
				continue;
			var interactButton = Instantiate(InteractionPrefab);
			interactButton.Setup(interaction, mgm, npcUiDisplay);
			interactButton.transform.SetParent(InteractionsParent);
		}
	}
}
