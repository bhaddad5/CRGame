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

				displayHandler.HandleDisplayDialogs(res.Dialogs, res.OptionalPopup, () =>
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
			if (interaction.Requirements.RequiredAmbition >= 0 && interaction.Requirements.RequiredAmbition < fem.Ambition)
				tooltip += $"\nRequires {interaction.Requirements.RequiredAmbition} or less Ambition";
			if (interaction.Requirements.RequiredPride >= 0 && interaction.Requirements.RequiredPride < fem.Pride)
				tooltip += $"\nRequires {interaction.Requirements.RequiredPride} or less Pride";
			if(interaction.Cost.EgoCost > mgm.Data.Ego)
				tooltip += $"\nRequires {interaction.Cost.EgoCost} Ego";
			if (interaction.Cost.MoneyCost > mgm.Data.Funds)
				tooltip += $"\nRequires ${interaction.Cost.MoneyCost}";
			if (interaction.Cost.CultureCost > mgm.Data.CorporateCulture)
				tooltip += $"\nRequires {interaction.Cost.CultureCost} Corporate Culture";
			if (interaction.Cost.BrandCost > mgm.Data.Brand)
				tooltip += $"\nRequires {interaction.Cost.BrandCost} Brand";
			if (interaction.Cost.SpreadsheetsCost > mgm.Data.Spreadsheets)
				tooltip += $"\nRequires {interaction.Cost.SpreadsheetsCost} Spreadsheets";
			if (interaction.Cost.RevanueCost > mgm.Data.Revenue)
				tooltip += $"\nRequires {interaction.Cost.RevanueCost} Revenue";
			if (interaction.Cost.PatentsCost > mgm.Data.Patents)
				tooltip += $"\nRequires {interaction.Cost.PatentsCost} Patents";
			if (interaction.Cost.HornicalCost > mgm.Data.Hornical)
				tooltip += $"\nRequires {interaction.Cost.HornicalCost} Hornical";

			if (tooltip.Length > 0)
				return tooltip.Substring(1);
			return "Interaction invalid";
		}
	}
}