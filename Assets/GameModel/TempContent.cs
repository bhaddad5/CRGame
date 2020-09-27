using System.Collections;
using System.Collections.Generic;
using Assets.GameModel;
using UnityEngine;

public class TempContent
{
	public static List<Department> GenerateContent()
	{
		#region Interactions

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

		#endregion

		#region Traits

		Trait overAchiever = new Trait()
		{
			Id = "overAchiever",
			Name = "Over-achiever",
			Effect = new Effect()
			{
				AmbitionEffect = .5f
			}
		};

		#endregion

		#region Fems

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

		#endregion

		#region Policies

		Policy peaceOfMind = new Policy()
		{
			Id = "peaceOfMind",
			Name = "Peace of Mind",
		};

		#endregion

		#region Departments

		Department legal = new Department()
		{
			Id = "legal",
			Name = "Legal",
			Fems = new List<Fem>() { deborahJones },
			Policies = new List<Policy>() { peaceOfMind },
			UiPosition = new Vector2(911, 429),
		};

		return new List<Department>() { legal };

		#endregion
	}
}
