using System;
using System.Collections;
using System.Collections.Generic;
using Assets.GameModel;
using GameModel.Serializers;
using UnityEngine;

public class DataDeserializer
{
	public GameData DeserializedData(SerializedGameData serializedGameData)
	{
		GameData data = SerializedGameData.Deserialize(serializedGameData);
		data = SerializedGameData.ResolveReferences(new DeserializedDataAccessor(data), data, serializedGameData);
		
		return data;
	}
}

public class DeserializedDataAccessor
{
	private GameData data;
	public DeserializedDataAccessor(GameData data)
	{
		this.data = data;
	}

	public Location FindLocationById(string id)
	{
		foreach (var location in data.Locations)
		{
			if (location.Id == id)
			{
				return location;
			}
		}
		return null;
	}

	public Policy FindPolicyById(string id)
	{
		foreach (var location in data.Locations)
		{
			foreach (var policy in location.Policies)
			{
				if (policy.Id == id)
				{
					return policy;
				}
			}
		}
		return null;
	}

	public Npc FindNpcById(string id)
	{
		foreach (var location in data.Locations)
		{
			foreach (var npc in location.Npcs)
			{
				if (npc.Id == id)
				{
					return npc;
				}
			}
		}
		return null;
	}

	public Trophy FindTrophyById(string id)
	{
		foreach (var location in data.Locations)
		{
			foreach (var npc in location.Npcs)
			{
				foreach (var trophy in npc.Trophies)
				{
					if (trophy.Id == id)
					{
						return trophy;
					}
				}
			}
		}
		return null;
	}

	public Interaction FindInteractionById(string id)
	{
		foreach (var location in data.Locations)
		{
			foreach (var npc in location.Npcs)
			{
				foreach (var interaction in npc.Interactions)
				{
					if (interaction.Id == id)
					{
						return interaction;
					}
				}
			}
		}
		return null;
	}
}