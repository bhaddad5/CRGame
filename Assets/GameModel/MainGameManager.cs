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
		}

		private SaveGameState startingData;

		private bool initialized = false;
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

			NullCleanupLogic.CleanUpAnnoyingNulls(DefaultGameData);

			startingData = SaveGameState.FromData(DefaultGameData);
			Data = DefaultGameData;

			if (saveDataPath != null)
				SaveLoadHandler.LoadAndApplyToGameData(saveDataPath, Data);

			hudUiDisplay.Setup(this, mainMapUiDisplay);
			mainMapUiDisplay.Setup(this, Data.Locations);
			RefreshAllUi();

			TryRunStartOfTurnInteractions();
			initialized = true;
		}

		void OnApplicationQuit()
		{
			if (initialized)
			{
				startingData.ApplyToData(DefaultGameData);
				Debug.Log("Data reset on quit");
			}
		}

		private void RefreshAllUi()
		{
			hudUiDisplay.RefreshUiDisplay(this);
			mainMapUiDisplay.RefreshUiDisplay(this);
		}

		public void HandleTurnChange()
		{
			Data.TurnNumber++;
			
			var dateTime = GetDateFromTurnNumber();
			if (Data.TurnNumber % 2 == 0 && 
			    (dateTime.Day == 15 || dateTime.Day == DateTime.DaysInMonth(dateTime.Year, dateTime.Month)))
			{
				HandleBiMonthlyChange();
			}

			mainMapUiDisplay.CloseCurrentDepartment(true);

			RefreshAllUi();

			TryRunStartOfTurnInteractions();
		}

		private void TryRunStartOfTurnInteractions()
		{
			foreach (var startOfTurnInteraction in Data.StartOfTurnInteractions)
			{
				if (startOfTurnInteraction.InteractionValid(this))
				{
					bool succeeded = startOfTurnInteraction.GetInteractionSucceeded();
					var res = startOfTurnInteraction.GetInteractionResult(succeeded);
					var displayHandler = new InteractionResultDisplayManager();
					displayHandler.DisplayInteractionResult(this, startOfTurnInteraction.Completed, res, !succeeded, () =>
					{
						res.Execute(this);
						if(succeeded)
							startOfTurnInteraction.Completed++;
						RefreshAllUi();
					});
				}
			}
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

		public void ShowDialog(DialogEntry dialog, Action onDialogsDone, Npc contextualNpc = null, NpcUiDisplay contextualNpcDisplay = null)
		{
			hudUiDisplay.ShowDialog(dialog, onDialogsDone, contextualNpc, contextualNpcDisplay);
		}
	}
}