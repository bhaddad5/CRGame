using System.Collections;
using System.Collections.Generic;
using Assets.GameModel;
using UnityEditor;
using UnityEngine;

public static class ProfilingHelpers
{
	[MenuItem("Company Man Debugging/Get Control Interactions Of Selected Object")]
	public static void GetLocationInteractions()
	{
		var gameData = AssetDatabase.LoadAssetAtPath<GameData>("Assets/Data/GameData.asset");

		var loc = Selection.activeObject as Location;

		var selectedNpc = Selection.activeObject as Npc;
		
		foreach (var location in gameData.Locations)
		{
			foreach (var npc in location.Npcs)
			{
				foreach (var interaction in npc.Interactions)
				{
					if(interaction.Result.Effect.LocationsToControl.Contains(loc))
						Debug.Log($"Controlled by: {interaction}");

					if (interaction.Result.Effect.NpcsToControl.Contains(selectedNpc))
						Debug.Log($"Controlled by: {interaction}");

					if (interaction.Result.Effect.NpcsToRemoveFromGame.Contains(selectedNpc))
						Debug.Log($"Removed From Game by: {interaction}");
				}

			}
		}
	}


	[MenuItem("Company Man Debugging/Print Control and Completion Interactions")]
	public static void PrintControlInteractions()
	{
		var gameData = AssetDatabase.LoadAssetAtPath<GameData>("Assets/Data/GameData.asset");

		foreach (var location in gameData.Locations)
		{
			foreach (var npc in location.Npcs)
			{
				foreach (var interaction in npc.Interactions)
				{
					foreach (var ob in interaction.Result.Effect.NpcsToControl)
						Debug.Log($"{ob} controlled by {interaction}");
					foreach (var ob in interaction.Result.Effect.NpcsToTrain)
						Debug.Log($"{ob} trained by {interaction}");
					foreach (var ob in interaction.Result.Effect.NpcsToRemoveFromGame)
						Debug.Log($"{ob} removed from game by {interaction}");
					foreach (var ob in interaction.Result.Effect.LocationsToControl)
						Debug.Log($"{ob} controlled by {interaction}");
					foreach (var ob in interaction.Result.Effect.TrophiesClaimedReferences)
						Debug.Log($"{ob} claimed by {interaction}");
					foreach (var ob in interaction.Result.Effect.MissionsToComplete)
						Debug.Log($"{ob} completed by {interaction}");
				}
			}
		}
		Debug.Log("Upgrade Complete!");
	}

	[MenuItem("Company Man Debugging/Copy All Text")]
	public static void UpgradeOldData()
	{
		var gameData = AssetDatabase.LoadAssetAtPath<GameData>("Assets/Data/GameData.asset");

		string AllText = "";

		foreach (var location in gameData.Locations)
		{
			foreach (var npc in location.Npcs)
			{
				AllText += $"NPC BIO - {npc.name}: {npc.Bio}\n\n";

				foreach (var interaction in npc.Interactions)
				{
					string interactionText = $"INTERACTION - {interaction.name}:\n";
					foreach (var dialog in interaction.Result.Dialogs)
					{
						interactionText += $"{dialog.Text}\n";
					}

					foreach (var popup in interaction.Result.OptionalPopups)
					{
						interactionText += $"POPUP: {popup.Text}";
					}

					if (interaction.CanFail)
						interactionText += $"Fail Result:\n";
					foreach (var dialog in interaction.FailureResult.Dialogs)
					{
						interactionText += $"{dialog.Text}\n";
					}



					AllText += $"{interactionText}\n\n";
				}
			}

			foreach (var policy in location.Policies)
			{
				AllText += $"POLICY - {policy.name}: {policy.Description}\n\n";
			}

			foreach (var mission in location.Missions)
			{
				AllText += $"MISSION - {mission.name}: {mission.MissionDescription}\n\n";
			}
		}

		GUIUtility.systemCopyBuffer = AllText;
		Debug.Log("Copy Complete!  Paste it anywhere");
	}

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
