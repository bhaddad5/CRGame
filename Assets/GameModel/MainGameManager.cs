﻿using System.Collections;
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
			//Test Shit!!!
			Interaction insinuatingComments = new Interaction()
			{
				Id = "insinuatingComments",
				Name = "Insinuating Comments",
				Dialog = "No wonder you got this job, with an ass like that...",
				EgoCost = 10,
				Effects = new List<Effect>()
				{
					new Effect()
					{
						AmbitionEffect = -20,
					}
				}
			};

			Interaction takeControl = new Interaction()
			{
				Id = "takeControl",
				Name = "Take Control",
				Dialog = "I own you now.",
				RequiredAmbition = 0,
				Effects = new List<Effect>()
				{
					new Effect()
					{
						EgoEffect = 100,
						ControlEffect = true,
					}
				}
			};

			Interaction motivationRoomApt = new Interaction()
			{
				Id = "motivationRoomApt",
				Name = "Motivation Room Appointment",
				Dialog = "I think your attitude needs correcting...",
				RequiredControl = true,
				RequiredPolicies = new List<string>() { "peaceOfMind" },
				TurnCost = 4,
				EgoCost = 20,
				Effects = new List<Effect>()
				{
					new Effect()
					{
						PrideEffect = -20,
					}
				}
			};

			Trait overAchiever = new Trait()
			{
				Id = "overAchiever",
				Name = "Over-achiever",
				Effect = new Effect()
				{
					AmbitionEffect = .5f
				}
			};

			Fem deborahJones = new Fem()
			{
				Id = "deborahJones",
				Ambition = 90,
				Pride = 100,
				Name = "Deborah Jones",
				Age = 24,
				Interactions = new List<Interaction>() { insinuatingComments, takeControl, motivationRoomApt },
				Traits = new List<Trait>()
				{
					overAchiever,
				}
			};

			Policy peaceOfMind = new Policy()
			{
				Id = "peaceOfMind",
				Name = "Peace of Mind",
			};

			Department legal = new Department()
			{
				Id = "legal",
				Name = "Legal",
				Fems = new List<Fem>(){ deborahJones },
				Policies = new List<Policy>() { peaceOfMind },
				UiPosition = new Vector2(911, 429),
			};

			Locations.Add(legal);

			//END TEST SHIT

			RootLevelUiDisplays.Add(HudUiDisplay);
			RootLevelUiDisplays.Add(MainMapUiDisplay);
			HudUiDisplay.Setup(this);
			MainMapUiDisplay.Setup(this, Locations);
			RefreshAllUi();
		}
	}
}