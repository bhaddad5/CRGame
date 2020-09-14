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

		public int RemainingTurnActions = 4;
		public float Ego = 10;
		public float Funds = 0;
		public float CorporateCulture = 0;
		public List<string> ActivePolicies;

		List<IUiDisplay> RootLevelUiDisplays = new List<IUiDisplay>();

		public void RefreshAllUi()
		{
			foreach (var uiDisplay in RootLevelUiDisplays)
			{
				uiDisplay.RefreshUiDisplay(this);
			}
		}

		void Start()
		{
			Interaction insinuatingComments = new Interaction()
			{
				Id = "insinuatingComments",
				Name = "Insinuating Comments",
				Dialog = "No wonder you got this job, with an ass like that...",
				EgoCost = 10,
				AmbitionEffect = -4,
			};

			Interaction takeControl = new Interaction()
			{
				Id = "takeControl",
				Name = "Take Control",
				Dialog = "I own you now.",
				RequiredAmbition = 0,
				EgoEffect = 100,
				ControlEffect = true,
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
				PrideEffect = -20,
			};

			Fem deborahJones = new Fem()
			{
				Id = "deborahJones",
				Ambition = 50,
				Pride = 100,
				Name = "Deborah Jones",
				Age = 24,
				Interactions = new List<Interaction>() { insinuatingComments, takeControl, motivationRoomApt }
			};

			Policy peaceOfMind = new Policy()
			{
				Id = "peaceOfMind",
			};

			Department legal = new Department()
			{
				Id = "legal",
				Name = "Legal",
				Fems = new List<Fem>(){ deborahJones },
				Policies = new List<Policy>() { peaceOfMind },
			};

			var fem = Instantiate(FemPrefab);
			fem.Setup(deborahJones);
			fem.RefreshUiDisplay(this);
			RootLevelUiDisplays.Add(fem);
		}

		//Test Shit
		[SerializeField] private FemUiDisplay FemPrefab;
	}
}