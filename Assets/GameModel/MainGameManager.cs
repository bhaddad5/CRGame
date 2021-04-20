﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Assets.GameModel.UiDisplayers;
using Assets.GameModel.XmlParsers;
using UnityEngine;

namespace Assets.GameModel
{
	public class MainGameManager : MonoBehaviour
	{
		public static MainGameManager Manager = null;

		public GameData Data;

		[SerializeField] private HudUiDisplay HudUiDisplayPrefab;
		[SerializeField] private MainMapUiDisplay MainMapUiDisplayPrefab;

		private HudUiDisplay hudUiDisplay;
		private MainMapUiDisplay mainMapUiDisplay;

		private XmlResolver xmlResolver = new XmlResolver();

		void Start()
		{
			Manager = this;

			InitializeGame(Path.Combine(Application.streamingAssetsPath, "GameData.xml"));
		}

		public void InitializeGame(string xmlDataPath)
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

			Data = xmlResolver.LoadXmlData(xmlDataPath);

			hudUiDisplay.Setup(this);
			mainMapUiDisplay.Setup(this, Data.Locations);
			RefreshAllUi();
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

			//Traits execute once a week
			if (Data.TurnNumber % 2 == 0 &&
			    Data.TurnNumber % 14 == 0)
			{
				HandleWeekChange();
			}

			mainMapUiDisplay.CloseCurrentDepartment(true);
			RefreshAllUi();
		}

		private void HandleBiMonthlyChange()
		{
			Data.Funds += GetPlayerBiMonthlySalaryFromPower();
		}

		public DateTime GetDateFromTurnNumber()
		{
			var currentDate = new DateTime(2030, 7, 1);
			currentDate += new TimeSpan(Data.TurnNumber/2, 0, 0, 0);

			return currentDate;
		}

		private void HandleWeekChange()
		{
			foreach (var department in Data.Locations)
			{
				foreach (var npc in department.Npcs)
				{
					foreach (var trait in npc.Traits)
					{
						if (npc.Controlled)
						{
							foreach (var effect in trait.ControlledEffects)
							{
								effect?.ExecuteEffect(this, npc);
							}
						}
						else
						{
							foreach (var effect in trait.FreeEffects)
							{
								effect?.ExecuteEffect(this, npc);
							}
						}
					}
				}
			}
		}

		public string GetPlayerTitleFromPower()
		{
			if (Data.Power < 10)
				return "Management Trainee";
			else if (Data.Power < 20)
				return "Executive";
			else if (Data.Power < 30)
				return "Manager";
			else if (Data.Power < 40)
				return "Director";
			else if (Data.Power < 50)
				return "General Manager";
			else if (Data.Power < 60)
				return "Managing Director ";
			else if (Data.Power < 70)
				return "Vice President";
			else if (Data.Power < 80)
				return "Executive Vice President";
			else if (Data.Power < 90)
				return "CEO";
			else return "Chairman of the Board";
		}

		public float GetPlayerBiMonthlySalaryFromPower()
		{
			if (Data.Power < 10)
				return 2000;
			else if (Data.Power < 20)
				return 3000;
			else if (Data.Power < 30)
				return 4000;
			else if (Data.Power < 40)
				return 5000;
			else if (Data.Power < 50)
				return 6000;
			else if (Data.Power < 60)
				return 7500;
			else if (Data.Power < 70)
				return 9000;
			else if (Data.Power < 80)
				return 12500;
			else if (Data.Power < 90)
				return 20000;
			else return 40000;
		}

		public string GetPlayerOfficeBackgroundId()
		{
			if (Data.Power < 10)
				return "cubicleOffice";
			else if (Data.Power < 20)
				return "execOffice";
			else if (Data.Power < 30)
				return "managerOffice";
			else if (Data.Power < 40)
				return "directorOffice";
			else if (Data.Power < 50)
				return "generalManagerOffice";
			else if (Data.Power < 60)
				return "managingDirectorOffice";
			else if (Data.Power < 70)
				return "VicePresidentOffice";
			else if (Data.Power < 80)
				return "execVpOffice";
			else if (Data.Power < 90)
				return "CEOOffice";
			else return "CEOOffice";
		}

		public string GetXmlSaveData()
		{
			return xmlResolver.SerializeXmlData(Data);
		}

		public void ShowPopup(Popup popup, Action onPopupDone)
		{
			hudUiDisplay.ShowPopup(popup, onPopupDone);
		}
	}
}