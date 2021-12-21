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

	public void Setup(List<Interaction> interactions, Npc npc, MainGameManager mgm, NpcUiDisplay npcUiDisplay)
	{
		allInteractions = new List<Interaction>(interactions);
		
		foreach (var interaction in allInteractions)
		{
			if (!interaction.InteractionVisible(mgm, npc))
				continue;
			var interactButton = Instantiate(InteractionPrefab);
			interactButton.Setup(interaction, npc, mgm, npcUiDisplay);
			interactButton.transform.SetParent(InteractionsParent);
		}
	}
}
