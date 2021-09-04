using System;
using System.Collections;
using System.Collections.Generic;
using Assets.GameModel;
using UnityEngine;

[Serializable]
public class GameData : ScriptableObject
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

	public PlayerStatusSymbols StatusSymbols = new PlayerStatusSymbols();

	public List<Location> Locations = new List<Location>();

	public List<Location> GetControlledLocations()
	{
		List<Location> controlledDepts = new List<Location>();
		foreach (var dept in Locations)
		{
			if(dept.Controlled())
				controlledDepts.Add(dept);
		}
		return controlledDepts;
	}

	public List<Policy> GetActivePolicies()
	{
		List<Policy> activePolicies = new List<Policy>();
		foreach (var dept in Locations)
		{
			foreach (var policy in dept.Policies)
			{
				if(policy.Active)
					activePolicies.Add(policy);
			}
		}

		return activePolicies;
	}

	public List<string> GetCompletedInteractionIds(string npcId)
	{
		List<string> completedInteractionIds = new List<string>();
		foreach (var department in Locations)
		{
			foreach (var npc in department.Npcs)
			{
				if (npc.Id == npcId)
				{
					foreach (var interaction in npc.Interactions)
					{
						if(interaction.Completed)
							completedInteractionIds.Add(interaction.Id);
					}
				}
			}
		}
		return completedInteractionIds;
	}

	public Interaction GetInteractionById(string npcId, string interactionId)
	{
		var npc = GetNpcById(npcId);

		if (npc == null)
			return null;

		foreach (var npcInteraction in npc.Interactions)
		{
			if (npcInteraction.Id == interactionId)
				return npcInteraction;
		}

		return null;
	}

	public List<Mission> GetMissionsForInteraction(string npcId, string interactionId)
	{
		List<Mission> res = new List<Mission>();

		foreach (var department in Locations)
		{
			foreach (var mission in department.Missions)
			{
				if(mission.npcId == npcId && mission.InteractionId == interactionId)
					res.Add(mission);
			}
		}

		return res;
	}

	public Npc GetNpcById(string NpcId)
	{
		foreach (var location in Locations)
		{
			foreach (var npc in location.Npcs)
			{
				if (npc.Id == NpcId)
				{
					return npc;
				}
			}
		}
		return null;
	}

	public Location GetLocationById(string locationId)
	{
		foreach (var location in Locations)
		{
			if (location.Id == locationId)
				return location;
		}
		return null;
	}

	public Location FindNpcLocation(Npc npc)
	{
		foreach (var department in Locations)
		{
			if (department.Npcs.Contains(npc))
				return department;
		}
		Debug.LogError($"Could not find department containing npc {npc.Id}");
		return null;
	}

	public Trophy GetTrophyById(string trophyId)
	{
		foreach (var location in Locations)
		{
			foreach (var npc in location.Npcs)
			{
				foreach (var trophy in npc.Trophies)
				{
					if (trophy.Id == trophyId)
						return trophy;
				}
			}
		}
		return null;
	}

	public Location DeadNpcPool
	{
		get
		{
			foreach (var department in Locations)
			{
				if (department.Id == "deadPool")
					return department;
			}
			Debug.LogError($"Could not find dead NPC pool.");
			return null;
		}
	}

	public Location MyOffice
	{
		get
		{
			foreach (var department in Locations)
			{
				if (department.Id == "playerOffice")
					return department;
			}
			Debug.LogError($"Could not find myOffice.");
			return null;
		}
	}

	public List<Trophy> GetOwnedTrophies()
	{
		List<Trophy> res = new List<Trophy>();
		foreach (var department in Locations)
		{
			foreach (var npc in department.Npcs)
			{
				foreach (var trophy in npc.Trophies)
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

	public Policy GetPolicyFromId(string id)
	{
		foreach (var department in Locations)
		{
			foreach (var policy in department.Policies)
			{
				if (policy.Id == id)
					return policy;
			}
		}

		Debug.LogError($"Failed to find policy for id {id}");
		return null;
	}
}
