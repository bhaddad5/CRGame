using System.Collections;
using System.Collections.Generic;
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

		void Start()
		{
			Fem deborahJones = new Fem()
			{
				Id = "deborahJones",
				Ambition = 50,
				Pride = 100,
				Name = "Deborah Jones",
				Age = 24,
			};

			Interaction insinuatingComments = new Interaction()
			{
				Id = "insinuatingComments",
				EgoCost = 10,
				AmbitionEffect = -4,
			};

			Interaction takeControl = new Interaction()
			{
				Id = "takeControl",
				RequiredAmbition = 0,
				EgoEffect = 100,
				ControlEffect = true,
			};

			Interaction motivationRoomApt = new Interaction()
			{
				Id = "motivationRoomApt",
				RequiredControl = true,
				RequiredPolicies = new List<string>() { "peaceOfMind" },
				TurnCost = 4,
				EgoCost = 20,
				PrideEffect = -20,
			};
		}
	}
}