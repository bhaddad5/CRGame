using System.Collections;
using System.Collections.Generic;
using Assets.GameModel;
using UnityEngine;

public class GameData
{
	public string PlayerName = "";
	public int TurnNumber = 0;
	public int Actions = 4;
	public float Ego = 10;
	public float Funds = 0;
	public float Power = 0;
	public float CorporateCulture = 0;

	public List<Department> Departments = new List<Department>();

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
}
