using System.Collections;
using System.Collections.Generic;
using Assets.GameModel;
using UnityEditor;
using UnityEngine;

public static class ProfilingHelpers
{
	[MenuItem("Company Man Debugging/Calculate Power Totals")]
	public static void CalculatePowerTotals()
	{
		var gameData = AssetDatabase.LoadAssetAtPath<GameData>("Assets/Data/GameData.asset");

		float totalPowerInGame = 0f;
		foreach (var location in gameData.Locations)
		{
			float locationTotalPower = 0f;
			string npcsString = "";
			float allNpcsPower = 0f;
			foreach (var npc in location.Npcs)
			{
				float npcTotalPower = 0f;
				foreach (var interaction in npc.Interactions)
				{
					npcTotalPower += interaction.Result.Effect.PowerEffect;
				}

				locationTotalPower += npcTotalPower;
				allNpcsPower += npcTotalPower;
				npcsString += $" ({npc.FirstName} {npc.LastName} - Power: {npcTotalPower})";
			}

			float missionsPowerTotal = 0;
			foreach (var mission in location.Missions)
			{
				locationTotalPower += mission.Effect.PowerEffect;
				missionsPowerTotal += mission.Effect.PowerEffect;
			}
			float policiesPowerTotal = 0;
			foreach (var policy in location.Policies)
			{
				locationTotalPower += policy.Effect.PowerEffect;
				policiesPowerTotal += policy.Effect.PowerEffect;
			}

			totalPowerInGame += locationTotalPower;

			if (locationTotalPower != 0)
				Debug.Log($"{location.Name}: Total Power = {locationTotalPower}, From Missions: {missionsPowerTotal}, From Policies {policiesPowerTotal}, From NPCs: {allNpcsPower} {npcsString}");
		}

		Debug.Log($"Total Power In Game = {totalPowerInGame}");
	}
}
