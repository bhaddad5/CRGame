using System.Collections;
using System.Collections.Generic;
using Assets.GameModel;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.GameModel.UiDisplayers
{
	public class InteractionUiDisplay : MonoBehaviour, IUiDisplay, ITooltipProvider
	{
		[SerializeField] private Button Button;
		[SerializeField] private TMP_Text Text;

		private Interaction interaction;
		private Fem fem;

		public void Setup(Interaction interaction, Fem fem, MainGameManager mgm, DialogDisplayHandler displayHandler)
		{
			this.interaction = interaction;
			this.fem = fem;
			Button.onClick.AddListener(() =>
			{
				var res = interaction.GetInteractionResult(mgm, fem);

				displayHandler.HandleDisplayDialogs(res.Dialogs, () =>
				{
					res.Execute(mgm, fem);
					mgm.HandleTurnChange();
				});
			});
			RefreshUiDisplay(mgm, fem);
		}

		public void RefreshUiDisplay(MainGameManager mgm, Fem fem)
		{
			Text.text = interaction.Name;
			Button.interactable = interaction.InteractionValid(mgm, fem);
			gameObject.SetActive(interaction.InteractionVisible(mgm, fem));
		}

		public string GetTooltip(MainGameManager mgm)
		{
			if (interaction.InteractionValid(mgm, fem))
			{
				return null;
			}

			string tooltip = "";
			if (interaction.RequiredAmbition >= 0 && interaction.RequiredAmbition < fem.Ambition)
				tooltip += $"\nRequires {interaction.RequiredAmbition} or less Ambition";
			if (interaction.RequiredPride >= 0 && interaction.RequiredPride < fem.Pride)
				tooltip += $"\nRequires {interaction.RequiredPride} or less Pride";
			if(interaction.EgoCost > mgm.Data.Ego)
				tooltip += $"\nRequires {interaction.EgoCost} Ego";
			if (interaction.MoneyCost > mgm.Data.Funds)
				tooltip += $"\nRequires ${interaction.MoneyCost}";
			if (interaction.CultureCost > mgm.Data.CorporateCulture)
				tooltip += $"\nRequires {interaction.CultureCost} Corporate Culture";
			if (interaction.BrandCost > mgm.Data.Brand)
				tooltip += $"\nRequires {interaction.BrandCost} Brand";
			if (interaction.SpreadsheetsCost > mgm.Data.Spreadsheets)
				tooltip += $"\nRequires {interaction.SpreadsheetsCost} Spreadsheets";
			if (interaction.RevanueCost > mgm.Data.Revenue)
				tooltip += $"\nRequires {interaction.RevanueCost} Revenue";
			if (interaction.PatentsCost > mgm.Data.Patents)
				tooltip += $"\nRequires {interaction.PatentsCost} Patents";
			if (interaction.HornicalCost > mgm.Data.Hornical)
				tooltip += $"\nRequires {interaction.HornicalCost} Hornical";

			if (tooltip.Length > 0)
				return tooltip.Substring(1);
			return "Interaction invalid";
		}
	}
}