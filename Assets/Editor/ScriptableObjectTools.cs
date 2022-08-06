using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Assets.GameModel;
using Assets.GameModel.Save;
using UnityEditor;
using UnityEngine;

public class ScriptableObjectTools
{
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

	[MenuItem("Tools/Fix String Newlines", false, 0)]
	public static void FixStringNewlines()
	{
		var gameData = AssetDatabase.LoadAssetAtPath<GameData>("Assets/Data/GameData.asset");

		foreach (var location in gameData.Locations)
		{
			foreach (var policy in location.Policies)
			{
				if (policy == null)
					continue;

				policy.Description = Fix(policy.Description);
				EditorUtility.SetDirty(policy);
			}
			foreach (var mission in location.Missions)
			{
				if (mission == null)
					continue;

				mission.MissionDescription = Fix(mission.MissionDescription);
				EditorUtility.SetDirty(mission);
			}

			foreach (var npc in location.Npcs)
			{
				npc.Bio = Fix(npc.Bio);
				EditorUtility.SetDirty(npc);

				foreach (var interaction in npc.Interactions)
				{
					for (int i = 0; i < interaction.Result.Dialogs.Count; i++)
					{
						var dialog = interaction.Result.Dialogs[i];
						dialog.Text = Fix(dialog.Text);
						interaction.Result.Dialogs[i] = dialog;
					}
					for (int i = 0; i < interaction.FailureResult.Dialogs.Count; i++)
					{
						var dialog = interaction.FailureResult.Dialogs[i];
						dialog.Text = Fix(dialog.Text);
						interaction.FailureResult.Dialogs[i] = dialog;
					}
					for (int i = 0; i < interaction.Result.OptionalPopups.Count; i++)
					{
						var dialog = interaction.Result.OptionalPopups[i];
						dialog.Text = Fix(dialog.Text);
						interaction.Result.OptionalPopups[i] = dialog;
					}
				}
			}
		}

		Debug.Log("Detection Complete!");
	}

	private static string Fix(string str)
	{
		var s = Regex.Replace(str, "\r", "\r\n");
		return Regex.Replace(s, "\r\n\n", "\r\n");
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
			foreach (var mission in location.Policies)
			{
				if (mission == null)
					continue;
			}
		}

		Debug.Log("Detection Complete!");
	}

	[MenuItem("Tools/Fix Missing IDs")]
	public static void FixMissingIds()
	{
		var gameData = AssetDatabase.LoadAssetAtPath<GameData>("Assets/Data/GameData.asset");

		foreach (var location in gameData.Locations)
		{
			if (String.IsNullOrEmpty(location.Id))
			{
				location.Id = Guid.NewGuid().ToString();
				EditorUtility.SetDirty(location);
			}


			foreach (var npc in location.Npcs)
			{
				if (String.IsNullOrEmpty(npc.Id))
				{
					npc.Id = Guid.NewGuid().ToString();
					EditorUtility.SetDirty(npc);
				}

				foreach (var interaction in npc.Interactions)
				{
					if (String.IsNullOrEmpty(interaction.Id))
					{
						interaction.Id = Guid.NewGuid().ToString();
						EditorUtility.SetDirty(interaction);
					}
				}
			}

			foreach (var mission in location.Missions)
			{
				if (String.IsNullOrEmpty(mission.Id))
				{
					mission.Id = Guid.NewGuid().ToString();
					EditorUtility.SetDirty(mission);
				}
			}

			foreach (var policy in location.Policies)
			{
				if (String.IsNullOrEmpty(policy.Id))
				{
					policy.Id = Guid.NewGuid().ToString();
					EditorUtility.SetDirty(policy);
				}
			}
		}

		foreach (var startOfTurnInteraction in gameData.StartOfTurnInteractions)
		{
			if (String.IsNullOrEmpty(startOfTurnInteraction.Id))
			{
				startOfTurnInteraction.Id = Guid.NewGuid().ToString();
				EditorUtility.SetDirty(startOfTurnInteraction);
			}
		}

		EditorUtility.SetDirty(gameData);
		Debug.Log("Upgrade Complete!");
	}

	/*[MenuItem("Tools/Update Popup Textures")]
	public static void UpdatePopupTextures()
	{
		foreach (var interaction in ProfilingHelpers.GetAllInteractions())
		{
			for (int i = 0; i < interaction.Result.OptionalPopups.Count; i++)
			{
				var popup = interaction.Result.OptionalPopups[i];
				if (popup.Texture == null)
					continue;

				popup.Textures = new List<Texture2D>() { popup.Texture };
				interaction.Result.OptionalPopups[i] = popup;
				EditorUtility.SetDirty(interaction);
			}
		}
	}*/

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

	[MenuItem("Tools/Remove Nulls")]
	public static void UpgradeOldData()
	{
		var gameData = AssetDatabase.LoadAssetAtPath<GameData>("Assets/Data/GameData.asset");

		foreach (var location in gameData.Locations)
		{
			if (location.Npcs.Contains(null))
				EditorUtility.SetDirty(location);
			location.Npcs.RemoveAll(v => v == null);

			foreach (var npc in location.Npcs)
			{
				if(npc.Interactions.Contains(null))
					EditorUtility.SetDirty(npc);
				npc.Interactions.RemoveAll(v => v == null);
			}

			if (location.Policies.Contains(null))
				EditorUtility.SetDirty(location);
			location.Policies.RemoveAll(v => v == null);

			if (location.Missions.Contains(null))
				EditorUtility.SetDirty(location);
			location.Missions.RemoveAll(v => v == null);
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
