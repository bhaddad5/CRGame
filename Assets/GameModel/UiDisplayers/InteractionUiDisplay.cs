using System.Collections;
using System.Collections.Generic;
using Assets.GameModel;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InteractionUiDisplay : MonoBehaviour, IUiDisplay
{
	[SerializeField] private Button Button;
	[SerializeField] private TMP_Text Text;

	private Interaction interaction;
	private Fem fem;
	public void Setup(Interaction interaction, Fem fem)
	{
		this.interaction = interaction;
		this.fem = fem;
	}

	public void RefreshUiDisplay(MainGameManager mgm)
	{
		Text.text = $"({interaction.Name}) {interaction.Dialog}";
		Button.interactable = interaction.InteractionValid(mgm, fem);
		Button.onClick.AddListener(() =>
		{
			interaction.ExecuteInteraction(mgm, fem);
			mgm.RefreshAllUi();
		});
	}
}
