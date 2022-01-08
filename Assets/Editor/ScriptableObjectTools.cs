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
	[MenuItem("Tools/Clean Up Null Data", false, 0)]
	public static void CleanNullsAndMarkDirty()
	{
		var gameData = AssetDatabase.LoadAssetAtPath<GameData>("Assets/Data/GameData.asset");
		NullCleanupLogic.CleanUpAnnoyingNulls(gameData);
		MarkAllDirty();
	}

	//We don't ever actually want to do this
	/*[MenuItem("Tools/Impose Default Values On Nulls", false, 0)]
	public static void ImposeDefaultsOnNulls()
	{
		var gameData = AssetDatabase.LoadAssetAtPath<GameData>("Assets/Data/GameData.asset");
		DefaultDataLogic.ImposeDefaultsOnNullFields(gameData);
		MarkAllDirty();
	}*/

	[MenuItem("Tools/Mark All Dirty", false, 20)]
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

	[MenuItem("Tools/Find Uncontrollable NPCs")]
	public static void FindUncontrollableNpcs()
	{
		var gameData = AssetDatabase.LoadAssetAtPath<GameData>("Assets/Data/GameData.asset");

		foreach (var location in gameData.Locations)
		{
			foreach (var npc in location.Npcs)
			{
				if(!npc.Interactions.Any(i => i.Result.Effect.NpcsToControl.Count > 0) && npc.Interactions.Any(i => i.Name.ToLowerInvariant().Contains("control")))
					Debug.Log("No control interaction for " + npc);

				if (!npc.Interactions.Any(i => i.Result.Effect.NpcsToTrain.Count > 0) && npc.Interactions.Any(i => i.Name.ToLowerInvariant().Contains("break")))
					Debug.Log("No train interaction for " + npc);
			}
		}
		Debug.Log("Search Complete!");
	}

	/*[MenuItem("Tools/Upgrade Old Data")]
	public static void UpgradeOldData()
	{
		var gameData = AssetDatabase.LoadAssetAtPath<GameData>("Assets/Data/GameData.asset");

		foreach (var location in gameData.Locations)
		{
			foreach (var npc in location.Npcs)
			{
				foreach (var interaction in npc.Interactions)
				{
					if (interaction.Result.CustomBackground != null)
					{
						for (int i = 0; i < interaction.Result.Dialogs.Count; i++)
						{
							var dialog = interaction.Result.Dialogs[i];
							dialog.CustomBackground = interaction.Result.CustomBackground;
							dialog.CustomBackgroundNpcLayout = interaction.Result.CustomBackgroundNpcLayout;
							interaction.Result.Dialogs[i] = dialog;
						}
						EditorUtility.SetDirty(interaction);
					}

					if (interaction.FailureResult.CustomBackground != null)
					{
						for (int i = 0; i < interaction.FailureResult.Dialogs.Count; i++)
						{
							var dialog = interaction.FailureResult.Dialogs[i];
							dialog.CustomBackground = interaction.FailureResult.CustomBackground;
							dialog.CustomBackgroundNpcLayout = interaction.FailureResult.CustomBackgroundNpcLayout;
							interaction.FailureResult.Dialogs[i] = dialog;
						}
						EditorUtility.SetDirty(interaction);
					}
				}
			}
		}
		Debug.Log("Upgrade Complete!");
	}*/

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
