﻿using System;
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
		public bool DebugAll;

		public int MajorVersion;
		public int MinorVersion;
		public int Patch;
		public string VersionName;

		[SerializeField] private GameData DefaultGameData;
		[HideInInspector] public GameData Data;

		[SerializeField] private AudioClip missionAudioClip;
		public AudioClip MissionAudioClip => missionAudioClip;
		[SerializeField] private AudioClip trophyAudioClip;
		public AudioClip TrophyAudioClip => trophyAudioClip;
		[SerializeField] private AudioClip policyAudioClip;

		public AudioClip GreivousModeClip => greivousModeClip;
		[SerializeField] private AudioClip greivousModeClip;

		public AudioClip PolicyAudioClip => policyAudioClip;

		[SerializeField] private AudioClip resourceTickAudio;
		public AudioClip ResourceTickAudio => resourceTickAudio;

		[SerializeField] private List<AudioClip> mainMenuAudio;
		public List<AudioClip> MainMenuAudio => mainMenuAudio;

		[SerializeField] private List<AudioClip> worldWeekdayAudio;
		public List<AudioClip> WorldWeekdayAudio => worldWeekdayAudio;

		[SerializeField] private List<AudioClip> worldWeekendAudio;
		public List<AudioClip> WorldWeekendAudio => worldWeekendAudio;
	
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
				ReturnToWorldMap();
				GameObject.Destroy(mainMapUiDisplay.gameObject);
			}
			if (InteractionResultDisplayManager.LiveDisplayHandler != null)
			{
				InteractionResultDisplayManager.LiveDisplayHandler.NukeAndShutdown();
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

			mainMapUiDisplay.HandleTurnChange();

			RefreshAllUi();

			foreach (var achievement in Data.Achievements)
			{
				if (!achievement.Completed && achievement.Requirements.RequirementsAreMet(this))
				{
					achievement.Completed = true;

					var popupParent = GameObject.Instantiate(UiPrefabReferences.Instance.PopupOverlayParent);
					Popup popup = new Popup();
					popup.Title = $"Achievement Earned: {achievement.Name}";
					popup.Text = achievement.Description;
					popup.Textures = new List<Texture2D>() { achievement.Image };
					GameObject.Instantiate(UiPrefabReferences.Instance.GetPrefabByName("Popup Display"), popupParent.transform).GetComponent<PopupBindings>().Setup(popup, 0, this, () => { });
				}
			}

			string path = LoadSaveHelpers.FileToValidPath("Autosave");
			if (path == null)
				return;
			File.WriteAllText(path, SaveLoadHandler.SaveToJson(Data));

			TryRunStartOfTurnInteractions();
		}

		public void OpenRegion(Region region)
		{
			ReturnToWorldMap();
			mainMapUiDisplay.ShowRegion(region, this);
		}

		public void ReturnToWorldMap()
		{
			mainMapUiDisplay?.CurrOpenRegion?.CloseRegion();
		}

		private void TryRunStartOfTurnInteractions()
		{
			if (DebugAll)
				return;

			foreach (var startOfTurnInteraction in Data.StartOfTurnInteractions)
			{
				if (startOfTurnInteraction.InteractionValid(this))
				{
					bool succeeded = startOfTurnInteraction.GetInteractionSucceeded();
					var res = startOfTurnInteraction.GetInteractionResult(succeeded);
					var displayHandler = new InteractionResultDisplayManager();
					displayHandler.DisplayInteractionResult(succeeded ? startOfTurnInteraction.Completed : startOfTurnInteraction.FailCount, res, !succeeded, new NpcDisplayInfo(), this, () =>
					{
						res.Execute(this);
						if (succeeded)
						{
							startOfTurnInteraction.Completed++;
							startOfTurnInteraction.TurnCompletedOn = Data.TurnNumber;
						}
						else
						{
							startOfTurnInteraction.FailCount++;
						}
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