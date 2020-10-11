using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Assets.GameModel.UiDisplayers;
using UnityEngine;

namespace Assets.GameModel
{
	public class MainGameManager : MonoBehaviour
	{
		public int TurnNumber = 0;

		private const int maxTurnActions = 4;
		public int RemainingTurnActions = 4;
		public float Ego = 10;
		public float Funds = 0;
		public float Power = 0;
		public float CorporateCulture = 0;
		public List<string> ActivePolicies;

		private List<Department> Locations = new List<Department>();

		[SerializeField] private HudUiDisplay HudUiDisplay;
		[SerializeField] private MainMapUiDisplay MainMapUiDisplay;
		private List<IUiDisplay> RootLevelUiDisplays = new List<IUiDisplay>();

		private void HandleEndTurn()
		{
			TurnNumber++;
			Ego += 10;
			RemainingTurnActions = maxTurnActions;
			foreach (Department department in Locations)
			{
				department.HandleEndTurn(this);
			}
		}

		public void RefreshAllUi()
		{
			foreach (var uiDisplay in RootLevelUiDisplays)
			{
				uiDisplay.RefreshUiDisplay(this);
			}
		}

		public void EndTurn()
		{
			HandleEndTurn();
			RefreshAllUi();
		}

		void Start()
		{
			var xmlResolver = new XmlResolver();
			xmlResolver.LoadXmlData();

			Locations = TempContent.GenerateContent();

			RootLevelUiDisplays.Add(HudUiDisplay);
			RootLevelUiDisplays.Add(MainMapUiDisplay);
			HudUiDisplay.Setup(this);
			MainMapUiDisplay.Setup(this, Locations);
			RefreshAllUi();
		}
	}
}