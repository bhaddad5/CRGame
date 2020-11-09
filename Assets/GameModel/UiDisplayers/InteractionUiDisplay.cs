using System.Collections;
using System.Collections.Generic;
using Assets.GameModel;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.GameModel.UiDisplayers
{
	public class InteractionUiDisplay : MonoBehaviour, IUiDisplay
	{
		[SerializeField] private Button Button;
		[SerializeField] private TMP_Text Text;

		[SerializeField] private DialogUiDisplay DialogPrefab;

		private Interaction interaction;
		private Fem fem;

		public void Setup(Interaction interaction, Fem fem, MainGameManager mgm, Transform dialogParent)
		{
			this.interaction = interaction;
			this.fem = fem;
			Button.onClick.AddListener(() =>
			{
				var res = interaction.ExecuteInteraction(mgm, fem);
				
				//clear existing dialog
				for (int i = 0; i < dialogParent.transform.childCount; i++)
					GameObject.Destroy(dialogParent.GetChild(i).gameObject);

				foreach (var dialogEntry in res.Dialogs)
				{
					var dialog = Instantiate(DialogPrefab);
					dialog.Setup(dialogEntry);
					dialog.transform.SetParent(dialogParent);
				}

				mgm.RefreshAllUi();
			});
		}

		public void RefreshUiDisplay(MainGameManager mgm, Fem fem)
		{
			Text.text = interaction.Name;
			Button.interactable = interaction.InteractionValid(mgm, fem);
			gameObject.SetActive(interaction.InteractionValid(mgm, fem));
		}
	}
}