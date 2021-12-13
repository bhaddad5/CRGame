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

	/*
	[MenuItem("Tools/Upgrade Old Data")]
	public static void UpgradeOldData()
	{
		var gameData = AssetDatabase.LoadAssetAtPath<GameData>("Assets/Data/GameData.asset");

		foreach (var location in gameData.Locations)
		{
			if (location?.ControlInteractionReference == null)
				continue;

			Debug.Log(location.ControlInteractionReference);
			
			if (location.ControlInteractionReference.InteractionResults[0].Effects.Count == 0)
				location.ControlInteractionReference.InteractionResults[0].Effects.Add(new Effect() { LocationsToControl = new List<Location>() });
			location.ControlInteractionReference.InteractionResults[0].Effects[0].LocationsToControl.Clear();
			location.ControlInteractionReference.InteractionResults[0].Effects[0].LocationsToControl.Add(location);
			EditorUtility.SetDirty(location.ControlInteractionReference);
		}
		Debug.Log("Upgrade complete!");
	}*/

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
					foreach (var interactionResult in interaction.InteractionResults)
					{
						foreach (var effect in interactionResult.Effects)
						{
							if (effect.ControlEffect)
							{
								effect.NpcsToControl.Clear();
								effect.NpcsToControl.Add(npc);
								EditorUtility.SetDirty(interaction);
							}

							if (effect.RemoveNpcFromGame)
							{
								effect.NpcsToRemoveFromGame.Clear();
								effect.NpcsToRemoveFromGame.Add(npc);
								EditorUtility.SetDirty(interaction);
							}
						}
					}
					
				}
			}

			foreach (var policy in location.Policies)
			{

			}

			foreach (var mission in location.Missions)
			{
				if (mission?.CompletionInteractionReference == null)
					continue;

				mission.Id = Guid.NewGuid().ToString();
				EditorUtility.SetDirty(mission);

				if (mission.CompletionInteractionReference.InteractionResults[0].Effects.Count == 0)
					mission.CompletionInteractionReference.InteractionResults[0].Effects.Add(new Effect(){MissionsToComplete = new List<Mission>()});
				mission.CompletionInteractionReference.InteractionResults[0].Effects[0].MissionsToComplete.Clear();
				mission.CompletionInteractionReference.InteractionResults[0].Effects[0].MissionsToComplete.Add(mission);
				EditorUtility.SetDirty(mission.CompletionInteractionReference);
			}
		}
		Debug.Log("Upgrade complete!");
	}*/


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
							ir.Effects.Any(e => e.MissionsToComplete?.Contains(mission) ?? false)))));

				if(!hasCompletionInteractions)
					Debug.Log($"{mission.MissionName} has no interactions that will complete it.");
			}
		}
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
	}
	*/
}
