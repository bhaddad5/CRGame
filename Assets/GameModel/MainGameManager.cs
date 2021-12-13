using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Assets.GameModel.UiDisplayers;
using Assets.GameModel.Save;
using UnityEngine;

namespace Assets.GameModel
{
	public class MainGameManager : MonoBehaviour
	{
		public static MainGameManager Manager = null;

		[SerializeField]
		private GameData DefaultGameData;
		[HideInInspector]
		public GameData Data;

		[SerializeField] private HudUiDisplay HudUiDisplayPrefab;
		[SerializeField] private MainMapUiDisplay MainMapUiDisplayPrefab;

		private HudUiDisplay hudUiDisplay;
		private MainMapUiDisplay mainMapUiDisplay;

		void Start()
		{
			Manager = this;

			InitializeGame(null);
		}

		private SaveGameState startingData;

		public void InitializeGame(string saveDataPath)
		{
			if (hudUiDisplay != null)
			{
				GameObject.Destroy(hudUiDisplay.gameObject);
			}
			if (mainMapUiDisplay != null)
			{
				mainMapUiDisplay.CloseCurrentDepartment(false);
				GameObject.Destroy(mainMapUiDisplay.gameObject);
			}

			hudUiDisplay = Instantiate(HudUiDisplayPrefab);
			mainMapUiDisplay = Instantiate(MainMapUiDisplayPrefab);

			startingData = SaveGameState.FromData(DefaultGameData);
			Data = DefaultGameData;

			if (saveDataPath != null)
				SaveLoadHandler.LoadAndApplyToGameData(saveDataPath, Data);

			hudUiDisplay.Setup(this, mainMapUiDisplay);
			mainMapUiDisplay.Setup(this, Data.Locations);
			RefreshAllUi();
		}

		void OnApplicationQuit()
		{
			startingData.ApplyToData(DefaultGameData);
			Debug.Log("Data reset on quit");
		}

		private void RefreshAllUi()
		{
			hudUiDisplay.RefreshUiDisplay(this);
			mainMapUiDisplay.RefreshUiDisplay(this);
		}

		public void HandleTurnChange()
		{
			foreach (var endOfTurnInteraction in Data.EndOfTurnInteractions)
			{
				if(endOfTurnInteraction.InteractionValid(this))
					endOfTurnInteraction.GetInteractionResult().Execute(this);
			}


			Data.TurnNumber++;
			
			var dateTime = GetDateFromTurnNumber();
			if (Data.TurnNumber % 2 == 0 && 
			    (dateTime.Day == 15 || dateTime.Day == DateTime.DaysInMonth(dateTime.Year, dateTime.Month)))
			{
				HandleBiMonthlyChange();
			}

			mainMapUiDisplay.CloseCurrentDepartment(true);
			RefreshAllUi();
		}

		private void HandleBiMonthlyChange()
		{
			Data.Funds += Data.PlayerPromotionLevels[Data.Promotion].Salary;
		}

		public DateTime GetDateFromTurnNumber()
		{
			var currentDate = new DateTime(2030, 7, 1);
			currentDate += new TimeSpan(Data.TurnNumber/2, 0, 0, 0);

			return currentDate;
		}
		
		public void ShowPopup(Popup popup, int completionCount, Action onPopupDone)
		{
			hudUiDisplay.ShowPopup(popup, completionCount, onPopupDone);
		}

		public void SetTrophyCaseVisibility(bool vis)
		{
			hudUiDisplay.SetTrophyCaseVisibility(vis);
		}

		public void SetStatusSymbolsVisibility(bool vis)
		{
			hudUiDisplay.SetStatusSymbolsVisibility(vis);
		}
	}
}