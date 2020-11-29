using System;
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

		[SerializeField] private HudUiDisplay HudUiDisplay;
		[SerializeField] private MainMapUiDisplay MainMapUiDisplay;

		void Start()
		{
			var xmlResolver = new XmlResolver();
			xmlResolver.LoadXmlData();

			Data = new XmlResolver().LoadXmlData();//TempContent.GenerateContent();

			HudUiDisplay.Setup(this);
			MainMapUiDisplay.Setup(this, Data.Departments);
			RefreshAllUi();
		}

		private void RefreshAllUi()
		{
			HudUiDisplay.RefreshUiDisplay(this);
			MainMapUiDisplay.RefreshUiDisplay(this);
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
			foreach (var department in Data.Departments)
			{
				foreach (var fem in department.Fems)
				{
					foreach (var trait in fem.Traits)
					{
						if (fem.Controlled)
						{
							foreach (var effect in trait.ControlledEffects)
							{
								effect?.ExecuteEffect(this, fem);
							}
						}
						else
						{
							foreach (var effect in trait.FreeEffects)
							{
								effect?.ExecuteEffect(this, fem);
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
	}
}