using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Assets.GameModel;
using Assets.GameModel.Save;
using UnityEditor;
using UnityEngine;

public class ScriptableObjectTools
{
	[MenuItem("Tools/Save Default Game Data")]
	public static void SaveGameState()
	{
		var gameData = AssetDatabase.LoadAssetAtPath<GameData>("Assets/Data/GameData.asset");

		string jsonData = SaveLoadHandler.SaveToJson(gameData);

		var savePath = Path.Combine(Application.streamingAssetsPath, "GameData.sav");
		var fs = File.Create(savePath);
		fs.Close();
		File.WriteAllText(savePath, jsonData);

		Debug.Log(jsonData);

		Debug.Log("Save Complete");
	}

	[MenuItem("Tools/Clean Up Data")]
	public static void CleanUpAnnoyingNulls()
	{
		var gameData = AssetDatabase.LoadAssetAtPath<GameData>("Assets/Data/GameData.asset");

		gameData.Locations.RemoveAll(i => i == null);
		EditorUtility.SetDirty(gameData);

		foreach (var location in gameData.Locations)
		{
			location.Npcs.RemoveAll(i => i == null);
			location.Policies.RemoveAll(i => i == null);
			location.Missions.RemoveAll(i => i == null);
			EditorUtility.SetDirty(location);

			foreach (var npc in location.Npcs)
			{
				npc.Interactions.RemoveAll(i => i == null);
				EditorUtility.SetDirty(npc);
			}
		}
	}
	
	[MenuItem("Tools/Detect Bad Data")]
	public static void DetectBadData()
	{
		var gameData = AssetDatabase.LoadAssetAtPath<GameData>("Assets/Data/GameData.asset");

		foreach (var location in gameData.Locations)
		{
			foreach (var mission in location.Missions)
			{
				if (mission == null)
					continue;

				bool hasCompletionInteractions = gameData.Locations.Any(l => l.Npcs.Any(n =>
					n.Interactions.Any(i =>
						i.InteractionResults.Any(ir =>
							ir.Effect.MissionsToComplete?.Contains(mission) ?? false))));

				if(!hasCompletionInteractions)
					Debug.Log($"{mission.MissionName} has no interactions that will complete it.");
			}
		}
	}
	
	[MenuItem("Tools/Count Multi effects & interactions")]
	public static void CountMultiEffects()
	{
		var gameData = AssetDatabase.LoadAssetAtPath<GameData>("Assets/Data/GameData.asset");

		foreach (var location in gameData.Locations)
		{
			foreach (var npc in location.Npcs)
			{
				foreach (var interaction in npc.Interactions)
				{
					if (interaction.InteractionResults.Count > 1)
						Debug.Log($"Multiple results in {interaction}");
				}
			}
		}
	}

	[MenuItem("Tools/Upgrade Old Data")]
	public static void UpgradeOldData()
	{
		var gameData = AssetDatabase.LoadAssetAtPath<GameData>("Assets/Data/GameData.asset");

		foreach (var location in gameData.Locations)
		{
			foreach (var npc in location.Npcs)
			{
				foreach (var interaction in npc.Interactions)
				{
					EditorUtility.SetDirty(interaction);
				}
			}
		}
		Debug.Log("Upgrade Complete!");
	}

	//TODO: USE THIS AS A TEMPLATE FOR DATA UPGRADES!
	/*
	[MenuItem("Tools/Upgrade Old Data")]
	public static void UpgradeOldData()
	{
		var gameData = AssetDatabase.LoadAssetAtPath<GameData>("Assets/Data/GameData.asset");

		foreach (var location in gameData.Locations)
		{
			foreach (var npc in location.Npcs)
			{
				foreach (var interaction in npc.Interactions)
				{
					foreach (var interactionResult in interaction.InteractionResults)
					{
						foreach (var effect in interactionResult.Effects)
						{
							
						}
					}
					EditorUtility.SetDirty(interaction);
				}
			}

			foreach (var policy in location.Policies)
			{
				
			}

			foreach (var mission in location.Missions)
			{
				
			}
		}
		Debug.Log("Upgrade Complete!");
	}
	*/
}
