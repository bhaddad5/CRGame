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

	public List<Department> Departments = new List<Department>();

	public List<string> GetControlledDepartmentIds()
	{
		List<string> controlledDepts = new List<string>();
		foreach (var dept in Departments)
		{
			if(dept.Controlled())
				controlledDepts.Add(dept.Id);
		}
		return controlledDepts;
	}

	public List<string> GetActivePolicyIds()
	{
		List<string> activePolicyIds = new List<string>();
		foreach (var dept in Departments)
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
		foreach (var department in Departments)
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

	public Fem GetFemById(string femId)
	{
		foreach (var department in Departments)
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

	public Department FindFemDepartment(Fem fem)
	{
		foreach (var department in Departments)
		{
			if (department.Fems.Contains(fem))
				return department;
		}
		Debug.LogError($"Could not find department containing fem {fem.Id}");
		return null;
	}
}
