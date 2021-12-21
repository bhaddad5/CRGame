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
	[MenuItem("Tools/Detect Repeatable")]
	public static void DetectFailable()
	{
		var gameData = AssetDatabase.LoadAssetAtPath<GameData>("Assets/Data/GameData.asset");

		foreach (var location in gameData.Locations)
		{
			foreach (var npc in location.Npcs)
			{
				foreach (var interaction in npc.Interactions)
				{
					if (interaction.Repeatable)
					{
						Debug.Log(interaction);
					}
				}
			}
		}

		Debug.Log("Detection Complete!");
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
					n.Interactions.Any(i => i.Result.Effect.MissionsToComplete?.Contains(mission) ?? false)));

				if(!hasCompletionInteractions)
					Debug.Log($"{mission.MissionName} has no interactions that will complete it.");
			}
		}

		Debug.Log("Detection Complete!");
	}

	[MenuItem("Tools/Clean Up Null Data")]
	public static void CleanNullsAndMarkDirty()
	{
		var gameData = AssetDatabase.LoadAssetAtPath<GameData>("Assets/Data/GameData.asset");
		NullCleanupLogic.CleanUpAnnoyingNulls(gameData);
		MarkAllDirty();
	}

	[MenuItem("Tools/Mark All Dirty")]
	public static void MarkAllDirty()
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

				npc.StartingAmbition = npc.Ambition;
				npc.StartingPride = npc.Pride;
				EditorUtility.SetDirty(npc);
			}
			EditorUtility.SetDirty(location);

			foreach (var mission in location.Missions)
			{
				EditorUtility.SetDirty(mission);
			}

			foreach (var policy in location.Policies)
			{
				EditorUtility.SetDirty(policy);
			}
		}

		foreach (var startOfTurnInteraction in gameData.StartOfTurnInteractions)
		{
			EditorUtility.SetDirty(startOfTurnInteraction);
		}

		EditorUtility.SetDirty(gameData);
		Debug.Log("Upgrade Complete!");
	}

	[MenuItem("Tools/Fix Corrupted Data")]
	public static void FixCorruptedData()
	{
		var gameData = AssetDatabase.LoadAssetAtPath<GameData>("Assets/Data/GameData.asset");

		foreach (var location in gameData.Locations)
		{
			foreach (var npc in location.Npcs)
			{
				foreach (var interaction in npc.Interactions)
				{
					interaction.Completed = 0;
					EditorUtility.SetDirty(interaction);
				}
				npc.Controlled = false;
				npc.Trained = false;
				EditorUtility.SetDirty(npc);
			}
			location.Controlled = false;
			EditorUtility.SetDirty(location);

			foreach (var mission in location.Missions)
			{
				mission.Completed = false;
				EditorUtility.SetDirty(mission);
			}

			foreach (var policy in location.Policies)
			{
				policy.Active = false;
				EditorUtility.SetDirty(policy);
			}
		}

		foreach (var startOfTurnInteraction in gameData.StartOfTurnInteractions)
		{
			startOfTurnInteraction.Completed = 0;
			EditorUtility.SetDirty(startOfTurnInteraction);
		}

		EditorUtility.SetDirty(gameData);
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
