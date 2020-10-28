using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Assets.GameModel.UiDisplayers;
using Assets.GameModel.XmlParsers;
using UnityEngine;

namespace Assets.GameModel
{
	public class MainGameManager : MonoBehaviour
	{
		public GameData Data;

		private const int maxTurnActions = 4;
		
		[SerializeField] private HudUiDisplay HudUiDisplay;
		[SerializeField] private MainMapUiDisplay MainMapUiDisplay;
		private List<IUiDisplay> RootLevelUiDisplays = new List<IUiDisplay>();

		private void HandleEndTurn()
		{
			Data.TurnNumber++;
			Data.Ego += 10;
			Data.Actions = maxTurnActions;
			foreach (Department department in Data.Departments)
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

			Data = new XmlResolver().LoadXmlData();//TempContent.GenerateContent();

			RootLevelUiDisplays.Add(HudUiDisplay);
			RootLevelUiDisplays.Add(MainMapUiDisplay);
			HudUiDisplay.Setup(this);
			MainMapUiDisplay.Setup(this, Data.Departments);
			RefreshAllUi();
		}
	}
}