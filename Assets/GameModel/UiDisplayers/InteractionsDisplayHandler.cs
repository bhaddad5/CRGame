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

	public void Setup(List<Interaction> interactions, Fem fem, MainGameManager mgm, DialogDisplayHandler dialogDisplay)
	{
		allInteractions = new List<Interaction>(interactions);
		
		InteractionsParent.gameObject.SetActive(false);
		CategoriesParent.gameObject.SetActive(true);

		BackToCategories.onClick.AddListener(() =>
		{
			InteractionsParent.gameObject.SetActive(false);
			CategoriesParent.gameObject.SetActive(true);
		});

		SetupButton(Interaction.InteractionCategory.OfficePolitics, OfficePoliticsParent, fem, mgm, dialogDisplay);
		SetupButton(Interaction.InteractionCategory.Conversation, ConversationParent, fem, mgm, dialogDisplay);
		SetupButton(Interaction.InteractionCategory.Challenge, ChallengeParent, fem, mgm, dialogDisplay);
		SetupButton(Interaction.InteractionCategory.Socialize, SocializeParent, fem, mgm, dialogDisplay);
		SetupButton(Interaction.InteractionCategory.Projects, ProjectsParent, fem, mgm, dialogDisplay);
		SetupButton(Interaction.InteractionCategory.Train, TrainParent, fem, mgm, dialogDisplay);
		SetupButton(Interaction.InteractionCategory.Fun, FunParent, fem, mgm, dialogDisplay);
		SetupButton(Interaction.InteractionCategory.Surveillance, SurveillanceParent, fem, mgm, dialogDisplay);

		RefreshInteractionVisibilities(fem, mgm);
	}

	private void SetupButton(Interaction.InteractionCategory cat, Button butt, Fem fem, MainGameManager mgm, DialogDisplayHandler dialogDisplay)
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
				interactButton.Setup(interaction, fem, mgm, dialogDisplay);
				interactButton.transform.SetParent(InteractionsParent);
			}

			InteractionsParent.gameObject.SetActive(true);
			CategoriesParent.gameObject.SetActive(false);
		});
	}

	public void RefreshInteractionVisibilities(Fem fem, MainGameManager mgm)
	{
		foreach (var display in InteractionsParent.GetComponentsInChildren<InteractionUiDisplay>(true))
			display.RefreshUiDisplay(mgm, fem);

		RefreshCategory(Interaction.InteractionCategory.OfficePolitics, OfficePoliticsParent, fem, mgm);
		RefreshCategory(Interaction.InteractionCategory.Conversation, ConversationParent, fem, mgm);
		RefreshCategory(Interaction.InteractionCategory.Challenge, ChallengeParent, fem, mgm);
		RefreshCategory(Interaction.InteractionCategory.Socialize, SocializeParent, fem, mgm);
		RefreshCategory(Interaction.InteractionCategory.Projects, ProjectsParent, fem, mgm);
		RefreshCategory(Interaction.InteractionCategory.Train, TrainParent, fem, mgm);
		RefreshCategory(Interaction.InteractionCategory.Fun, FunParent, fem, mgm);
		RefreshCategory(Interaction.InteractionCategory.Surveillance, SurveillanceParent, fem, mgm);
	}

	private void RefreshCategory(Interaction.InteractionCategory category, Button categoryParent, Fem fem, MainGameManager mgm)
	{
		categoryParent.gameObject.SetActive(allInteractions.Any(i => i.Category == category && i.InteractionValid(mgm, fem)));
	}
}
