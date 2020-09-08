using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
				Name = "Insinuating Comments",
				EgoCost = 10,
				AmbitionEffect = -4,
			};

			Interaction takeControl = new Interaction()
			{
				Id = "takeControl",
				Name = "Take Control",
				RequiredAmbition = 0,
				EgoEffect = 100,
				ControlEffect = true,
			};

			Interaction motivationRoomApt = new Interaction()
			{
				Id = "motivationRoomApt",
				Name = "Motivation Room Appointment",
				RequiredControl = true,
				RequiredPolicies = new List<string>() { "peaceOfMind" },
				TurnCost = 4,
				EgoCost = 20,
				PrideEffect = -20,
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

			Debug.Log(insinuatingComments.InteractionValid(this, deborahJones));
			Debug.Log(takeControl.InteractionValid(this, deborahJones));
			Debug.Log(motivationRoomApt.InteractionValid(this, deborahJones));

			insinuatingComments.ExecuteInteraction(this, deborahJones);
			Debug.Log(deborahJones.Ambition);
			insinuatingComments.ExecuteInteraction(this, deborahJones);
			insinuatingComments.ExecuteInteraction(this, deborahJones); 
			insinuatingComments.ExecuteInteraction(this, deborahJones);
			insinuatingComments.ExecuteInteraction(this, deborahJones);
			insinuatingComments.ExecuteInteraction(this, deborahJones);
			insinuatingComments.ExecuteInteraction(this, deborahJones);
			insinuatingComments.ExecuteInteraction(this, deborahJones);
			insinuatingComments.ExecuteInteraction(this, deborahJones);
			insinuatingComments.ExecuteInteraction(this, deborahJones);
			insinuatingComments.ExecuteInteraction(this, deborahJones);
			insinuatingComments.ExecuteInteraction(this, deborahJones);
			insinuatingComments.ExecuteInteraction(this, deborahJones);
			insinuatingComments.ExecuteInteraction(this, deborahJones);
			insinuatingComments.ExecuteInteraction(this, deborahJones);
			insinuatingComments.ExecuteInteraction(this, deborahJones);
			insinuatingComments.ExecuteInteraction(this, deborahJones);
			insinuatingComments.ExecuteInteraction(this, deborahJones);
			insinuatingComments.ExecuteInteraction(this, deborahJones);
			insinuatingComments.ExecuteInteraction(this, deborahJones);
			insinuatingComments.ExecuteInteraction(this, deborahJones);
			insinuatingComments.ExecuteInteraction(this, deborahJones);
			insinuatingComments.ExecuteInteraction(this, deborahJones);
			insinuatingComments.ExecuteInteraction(this, deborahJones);
			insinuatingComments.ExecuteInteraction(this, deborahJones);
			insinuatingComments.ExecuteInteraction(this, deborahJones);
			insinuatingComments.ExecuteInteraction(this, deborahJones);
			insinuatingComments.ExecuteInteraction(this, deborahJones);

			Debug.Log(deborahJones.Ambition);

			RemainingTurnActions = 4;

			Debug.Log(takeControl.InteractionValid(this, deborahJones));
			takeControl.ExecuteInteraction(this, deborahJones);

			Debug.Log(legal.Controlled());
			//legal.Policies.First().Active 
		}
	}
}