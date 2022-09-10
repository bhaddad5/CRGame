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
		public int MajorVersion;
		public int MinorVersion;
		public int Patch;
		public string VersionName;

		[SerializeField] private GameData DefaultGameData;
		[HideInInspector] public GameData Data;

		[SerializeField] private AudioClip missionAudioClip;
		public string MissionAudioClip => missionAudioClip.name;
		[SerializeField] private AudioClip trophyAudioClip;
		public string TrophyAudioClip => trophyAudioClip.name;
		[SerializeField] private AudioClip policyAudioClip;
		public string PolicyAudioClip => policyAudioClip.name;

		[SerializeField] private AudioClip resourceTickAudio;
		public string ResourceTickAudio => resourceTickAudio.name;

		[SerializeField] private List<AudioClip> mainMenuAudio;
		public List<string> MainMenuAudio
		{
			get
			{
				var res = new List<string>();
				foreach (var track in mainMenuAudio)
				{
					res.Add(track.name);
				}
				return res;
			}
		}

		[SerializeField] private List<AudioClip> worldWeekdayAudio;
		public List<string> WorldWeekdayAudio
		{
			get
			{
				var res = new List<string>();
				foreach (var track in worldWeekdayAudio)
				{
					res.Add(track.name);
				}
				return res;
			}
		}

		[SerializeField] private List<AudioClip> worldWeekendAudio;
		public List<string> WorldWeekendAudio
		{
			get
			{
				var res = new List<string>();
				foreach (var track in worldWeekendAudio)
				{
					res.Add(track.name);
				}
				return res;
			}
		}

		public string DefaultFirstName => DefaultGameData.FirstName;
		public string DefaultLastName => DefaultGameData.LastName;

		[SerializeField] private HudBindings HudUiDisplayPrefab;
		[SerializeField] private WorldMapScreenBindings MainMapUiDisplayPrefab;

		private HudBindings hudUiDisplay;
		private WorldMapScreenBindings mainMapUiDisplay;
		
		public void InitializeGame(string saveDataPath, string firstName, string lastName)
		{
			if (hudUiDisplay != null)
			{
				GameObject.Destroy(hudUiDisplay.gameObject);
			}
			if (mainMapUiDisplay != null)
			{
				GameObject.Destroy(mainMapUiDisplay.gameObject);
			}

			hudUiDisplay = Instantiate(HudUiDisplayPrefab);
			mainMapUiDisplay = Instantiate(MainMapUiDisplayPrefab);

			NullCleanupLogic.CleanUpAnnoyingNulls(DefaultGameData);
			DefaultDataLogic.ImposeDefaultsOnNullFields(DefaultGameData);
			DefaultGameData.Setup(this);

			Data = DefaultGameData;

			if (saveDataPath != null)
			{
				SaveLoadHandler.LoadAndApplyToGameData(saveDataPath, Data);
			}
			else
			{
				Data.FirstName = firstName;
				Data.LastName = lastName;
			}

			hudUiDisplay.Setup(this);
			mainMapUiDisplay.Setup(this, Data.Regions);
			RefreshAllUi();

			TryRunStartOfTurnInteractions();
		}

		void OnApplicationQuit()
		{
			DefaultGameData.Setup(this);
			Debug.Log("Data reset on quit");
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

			string path = LoadSaveHelpers.FileToValidPath("Autosave");
			if (path == null)
				return;
			File.WriteAllText(path, SaveLoadHandler.SaveToJson(Data));

			mainMapUiDisplay.HandleTurnChange();

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
					displayHandler.DisplayInteractionResult(startOfTurnInteraction.Completed, res, !succeeded, this, () =>
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

		public bool IsWeekend()
		{
			var dayOfWeek = GetDateFromTurnNumber().DayOfWeek;
			return dayOfWeek == DayOfWeek.Saturday || dayOfWeek == DayOfWeek.Sunday;
		}
	}
}