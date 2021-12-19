using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Assets.GameModel;
using Assets.GameModel.UiDisplayers;
using UnityEngine;
using UnityEngine.UI;

public class InteractionsDisplayHandler : MonoBehaviour
{
	[SerializeField] private Transform CategoriesParent;

	[SerializeField] private Button OfficePoliticsParent;
	[SerializeField] private Button ConversationParent;
	[SerializeField] private Button ChallengeParent;
	[SerializeField] private Button SocializeParent;
	[SerializeField] private Button ProjectsParent;
	[SerializeField] private Button TrainParent;
	[SerializeField] private Button FunParent;
	[SerializeField] private Button SurveillanceParent;

	[SerializeField] private Button BackToCategories;
	[SerializeField] private Transform InteractionsParent;

	[SerializeField] private InteractionUiDisplay InteractionPrefab;

	private List<Interaction> allInteractions;

	public void Setup(List<Interaction> interactions, Npc npc, MainGameManager mgm, NpcUiDisplay npcUiDisplay)
	{
		allInteractions = new List<Interaction>(interactions);
		
		InteractionsParent.gameObject.SetActive(false);
		CategoriesParent.gameObject.SetActive(true);

		BackToCategories.onClick.AddListener(() =>
		{
			InteractionsParent.gameObject.SetActive(false);
			CategoriesParent.gameObject.SetActive(true);
		});

		SetupButton(Interaction.InteractionCategory.OfficePolitics, OfficePoliticsParent, npc, mgm, npcUiDisplay);
		SetupButton(Interaction.InteractionCategory.Conversation, ConversationParent, npc, mgm, npcUiDisplay);
		SetupButton(Interaction.InteractionCategory.Challenge, ChallengeParent, npc, mgm, npcUiDisplay);
		SetupButton(Interaction.InteractionCategory.Socialize, SocializeParent, npc, mgm, npcUiDisplay);
		SetupButton(Interaction.InteractionCategory.Projects, ProjectsParent, npc, mgm, npcUiDisplay);
		SetupButton(Interaction.InteractionCategory.Train, TrainParent, npc, mgm, npcUiDisplay);
		SetupButton(Interaction.InteractionCategory.Fun, FunParent, npc, mgm, npcUiDisplay);
		SetupButton(Interaction.InteractionCategory.Surveillance, SurveillanceParent, npc, mgm, npcUiDisplay);

		RefreshInteractionVisibilities(npc, mgm);
	}

	private void SetupButton(Interaction.InteractionCategory cat, Button butt, Npc npc, MainGameManager mgm, NpcUiDisplay npcUiDisplay)
	{
		butt.onClick.AddListener(() =>
		{
			for (int i = 1; i < InteractionsParent.childCount; i++)
			{
				GameObject.Destroy(InteractionsParent.GetChild(i).gameObject);
			}

			foreach (var interaction in allInteractions.Where(i => i.Category == cat))
			{
				var interactButton = Instantiate(InteractionPrefab);
				interactButton.Setup(interaction, npc, mgm, npcUiDisplay);
				interactButton.transform.SetParent(InteractionsParent);
			}

			InteractionsParent.gameObject.SetActive(true);
			CategoriesParent.gameObject.SetActive(false);
		});
	}

	public void RefreshInteractionVisibilities(Npc npc, MainGameManager mgm)
	{
		foreach (var display in InteractionsParent.GetComponentsInChildren<InteractionUiDisplay>(true))
			display.RefreshUiDisplay(mgm, npc);

		RefreshCategory(Interaction.InteractionCategory.OfficePolitics, OfficePoliticsParent, npc, mgm);
		RefreshCategory(Interaction.InteractionCategory.Conversation, ConversationParent, npc, mgm);
		RefreshCategory(Interaction.InteractionCategory.Challenge, ChallengeParent, npc, mgm);
		RefreshCategory(Interaction.InteractionCategory.Socialize, SocializeParent, npc, mgm);
		RefreshCategory(Interaction.InteractionCategory.Projects, ProjectsParent, npc, mgm);
		RefreshCategory(Interaction.InteractionCategory.Train, TrainParent, npc, mgm);
		RefreshCategory(Interaction.InteractionCategory.Fun, FunParent, npc, mgm);
		RefreshCategory(Interaction.InteractionCategory.Surveillance, SurveillanceParent, npc, mgm);
	}

	private void RefreshCategory(Interaction.InteractionCategory category, Button categoryParent, Npc npc, MainGameManager mgm)
	{
		categoryParent.gameObject.SetActive(allInteractions.Any(i => i.Category == category && i.InteractionVisible(mgm, npc)));

		categoryParent.GetComponent<TooltipProviderBasic>().Tooltip = categoryParent.interactable ? null : "No valid interactions";
	}
}
