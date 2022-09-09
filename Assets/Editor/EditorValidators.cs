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
using Object = System.Object;

public class EditorValidators
{
	[MenuItem("Company Man Validators/Fix Everything", false, 0)]
	public static void FixEverything()
	{
		RemoveNulls();
		FixMissingIds();
		FixCorruptedData();
		FixStringNewlines();
		Debug.Log("All fixes processed!");
	}

	[MenuItem("Company Man Validators/Fix String Newlines", false, 100)]
	public static void FixStringNewlines()
	{
		var gameData = AssetDatabase.LoadAssetAtPath<GameData>("Assets/Data/GameData.asset");

		foreach (var region in gameData.Regions)
		{
			foreach (var location in region.Locations)
			{
				foreach (var policy in location.Policies)
				{
					policy.Description = FixItemNewlines(policy.Description, policy);
				}
				foreach (var mission in location.Missions)
				{
					mission.MissionDescription = FixItemNewlines(mission.MissionDescription, mission);
				}

				foreach (var npc in location.Npcs)
				{
					npc.Bio = FixItemNewlines(npc.Bio, npc);

					foreach (var interaction in npc.Interactions)
					{
						for (int i = 0; i < interaction.Result.Dialogs.Count; i++)
						{
							var dialog = interaction.Result.Dialogs[i];
							dialog.Text = FixItemNewlines(dialog.Text, interaction);
							interaction.Result.Dialogs[i] = dialog;
						}
						for (int i = 0; i < interaction.FailureResult.Dialogs.Count; i++)
						{
							var dialog = interaction.FailureResult.Dialogs[i];
							dialog.Text = FixItemNewlines(dialog.Text, interaction);
							interaction.FailureResult.Dialogs[i] = dialog;
						}
						for (int i = 0; i < interaction.Result.OptionalPopups.Count; i++)
						{
							var dialog = interaction.Result.OptionalPopups[i];
							dialog.Text = FixItemNewlines(dialog.Text, interaction);
							interaction.Result.OptionalPopups[i] = dialog;
						}
					}
				}
			}
		}
		

		Debug.Log("Newlines fixed!");
	}

	private static string FixItemNewlines(string str, UnityEngine.Object replacement)
	{
		var s = Regex.Replace(str, "\r", "\r\n");
		var res = Regex.Replace(s, "\r\n\n", "\r\n");

		if(res != str)
			EditorUtility.SetDirty(replacement);
		return res;
	}
	
	[MenuItem("Company Man Validators/Fix Missing IDs", false, 100)]
	public static void FixMissingIds()
	{
		var gameData = AssetDatabase.LoadAssetAtPath<GameData>("Assets/Data/GameData.asset");

		foreach (var region in gameData.Regions)
		{
			foreach (var location in region.Locations)
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
		Debug.Log("IDs Validated!");
	}
	
	[MenuItem("Company Man Validators/Fix Corrupted Data", false, 100)]
	public static void FixCorruptedData()
	{
		var gameData = AssetDatabase.LoadAssetAtPath<GameData>("Assets/Data/GameData.asset");

		foreach (var region in gameData.Regions)
		{
			foreach (var location in region.Locations)
			{
				foreach (var npc in location.Npcs)
				{
					if (npc.Controlled || npc.Trained)
					{
						npc.Controlled = false;
						npc.Trained = false;
						EditorUtility.SetDirty(npc);
					}
				}

				if (location.Controlled)
				{
					location.Controlled = false;
					EditorUtility.SetDirty(location);
				}

				foreach (var mission in location.Missions)
				{
					if (mission.Completed)
					{
						mission.Completed = false;
						EditorUtility.SetDirty(mission);
					}
				}

				foreach (var policy in location.Policies)
				{
					if (policy.Active)
					{
						policy.Active = false;
						EditorUtility.SetDirty(policy);
					}
				}
			}
		}

		foreach (var interaction in ProfilingHelpers.GetAllInteractions())
		{
			if (interaction.Completed > 0)
			{
				interaction.Completed = 0;
				EditorUtility.SetDirty(interaction);
			}
		}
		
		Debug.Log("Corrupted data reset!");
	}

	[MenuItem("Company Man Validators/Remove Nulls", false, 100)]
	public static void RemoveNulls()
	{
		var gameData = AssetDatabase.LoadAssetAtPath<GameData>("Assets/Data/GameData.asset");

		foreach (var region in gameData.Regions)
		{
			foreach (var location in region.Locations)
			{
				if (location.Npcs.Contains(null))
					EditorUtility.SetDirty(location);
				location.Npcs.RemoveAll(v => v == null);

				foreach (var npc in location.Npcs)
				{
					if (npc.Interactions.Contains(null))
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
		}
			
		Debug.Log("Null values removed!");
	}

	[MenuItem("Company Man Validators/UpgradeOldData", false, 200)]
	public static void UpgradeOldData()
	{
		var gameData = AssetDatabase.LoadAssetAtPath<GameData>("Assets/Data/GameData.asset");

		foreach (var region in gameData.Regions)
		{
			foreach (var location in region.Locations)
			{
				foreach (var npc in location.Npcs)
				{
					foreach (var interaction in npc.Interactions)
					{
						if (interaction.Cost.HornicalCost > 0)
						{
							interaction.Cost.Items.Add(DataUpgradeRefs.Instance.Hornical);
							EditorUtility.SetDirty(interaction);
						}
						if (interaction.Result.Effect.HornicalEffect > 0)
						{
							interaction.Result.Effect.ItemsToAdd.Add(DataUpgradeRefs.Instance.Hornical);
							EditorUtility.SetDirty(interaction);
						}
					}
				}
				location.Missions.RemoveAll(v => v == null);
			}
		}
			
		Debug.Log("Null values removed!");
	}

	//TODO: USE THIS AS A TEMPLATE FOR DATA UPGRADES!
	/*
	[MenuItem("Company Man Validators/Upgrade Old Data")]
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
