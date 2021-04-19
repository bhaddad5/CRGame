using System.Collections;
using System.Collections.Generic;
using Assets.GameModel;
using UnityEngine;

public class GameData
{
	public string PlayerName = "";
	public int TurnNumber = 0;
	public float Ego = 10;
	public float Funds = 0;
	public float Power = 0;
	public float Patents = 0;
	public float CorporateCulture = 0;
	public float Spreadsheets = 0;
	public float Brand = 0;
	public float Revenue = 0;
	public int Hornical = 0;

	public List<Location> Locations = new List<Location>();

	public List<string> GetControlledDepartmentIds()
	{
		List<string> controlledDepts = new List<string>();
		foreach (var dept in Locations)
		{
			if(dept.Controlled())
				controlledDepts.Add(dept.Id);
		}
		return controlledDepts;
	}

	public List<string> GetActivePolicyIds()
	{
		List<string> activePolicyIds = new List<string>();
		foreach (var dept in Locations)
		{
			foreach (var policy in dept.Policies)
			{
				if(policy.Active)
					activePolicyIds.Add(policy.Id);
			}
		}

		return activePolicyIds;
	}

	public List<string> GetCompletedInteractionIds(string femId)
	{
		List<string> completedInteractionIds = new List<string>();
		foreach (var department in Locations)
		{
			foreach (var fem in department.Fems)
			{
				if (fem.Id == femId)
				{
					foreach (var interaction in fem.Interactions)
					{
						if(interaction.Completed)
							completedInteractionIds.Add(interaction.Id);
					}
				}
			}
		}
		return completedInteractionIds;
	}

	public Interaction GetInteractionById(string femId, string interactionId)
	{
		var fem = GetFemById(femId);

		if (fem == null)
			return null;

		foreach (var femInteraction in fem.Interactions)
		{
			if (femInteraction.Id == interactionId)
				return femInteraction;
		}

		return null;
	}

	public List<Mission> GetMissionsForInteraction(string femId, string interactionId)
	{
		List<Mission> res = new List<Mission>();

		foreach (var department in Locations)
		{
			foreach (var mission in department.Missions)
			{
				if(mission.FemId == femId && mission.InteractionId == interactionId)
					res.Add(mission);
			}
		}

		return res;
	}

	public Fem GetFemById(string femId)
	{
		foreach (var department in Locations)
		{
			foreach (var fem in department.Fems)
			{
				if (fem.Id == femId)
				{
					return fem;
				}
			}
		}
		return null;
	}

	public Location FindFemDepartment(Fem fem)
	{
		foreach (var department in Locations)
		{
			if (department.Fems.Contains(fem))
				return department;
		}
		Debug.LogError($"Could not find department containing fem {fem.Id}");
		return null;
	}

	public List<Trophy> GetOwnedTrophies()
	{
		List<Trophy> res = new List<Trophy>();
		foreach (var department in Locations)
		{
			foreach (var fem in department.Fems)
			{
				foreach (var trophy in fem.Trophies)
				{
					if(trophy.Owned)
						res.Add(trophy);
				}
			}
		}
		return res;
	}

	public List<string> GetOwnedTrophyIds()
	{
		List<string> res = new List<string>();
		foreach (var trophy in GetOwnedTrophies())
		{
			res.Add(trophy.Id);
		}
		return res;
	}
}
