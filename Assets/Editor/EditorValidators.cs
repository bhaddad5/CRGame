using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Assets.GameModel;
using Assets.GameModel.Save;
using UnityEditor;
using UnityEngine;
using Debug = UnityEngine.Debug;
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

	[MenuItem("Company Man Validators/Update Audio Clip Names", false, 200)]
	public static void UpdateAudioClipNames()
	{
		var gameData = AssetDatabase.LoadAssetAtPath<GameData>("Assets/Data/GameData.asset");

		foreach (var region in gameData.Regions)
		{
			foreach (var location in region.Locations)
			{
				foreach (var npc in location.Npcs)
				{
					if (npc.OptionalDialogClip != null)
					{
						var newClipName = $"{npc.FirstName} {npc.LastName} Screen Dialog";
						newClipName = newClipName.Replace("'", "");
						AssetDatabase.MoveAsset(AssetDatabase.GetAssetPath(npc.OptionalDialogClip), $"Assets/Resources/Audio/{newClipName}.wav");
					}

					foreach (var interaction in npc.Interactions)
					{
						int i = 0;
						foreach (var dialogEntry in interaction.Result.Dialogs)
						{
							if (dialogEntry.OptionalAudioClip != null)
							{
								string newClipName = $"{npc.FirstName} {npc.LastName} {interaction.Name} {i}";
								newClipName = newClipName.Replace("'", "");
								AssetDatabase.MoveAsset(AssetDatabase.GetAssetPath(dialogEntry.OptionalAudioClip), $"Assets/Resources/Audio/{newClipName}.wav");
							}

							i++;
						}

						i = 0;
						foreach (var popup in interaction.Result.OptionalPopups)
						{
							int j = 0;
							foreach (var clip in popup.DialogClips)
							{
								if(clip == null)
									continue;
								string newClipName = $"{npc.FirstName} {npc.LastName} {interaction.Name} Popup {i} - {j}";
								newClipName = newClipName.Replace("'", "");
								AssetDatabase.MoveAsset(AssetDatabase.GetAssetPath(clip), $"Assets/Resources/Audio/{newClipName}.wav");
							}
						}

						i = 0;
						foreach (var dialogEntry in interaction.FailureResult.Dialogs)
						{
							if (dialogEntry.OptionalAudioClip != null)
							{
								string newClipName = $"{npc.FirstName} {npc.LastName} {interaction.Name} FAILED {i}";
								newClipName = newClipName.Replace("'", "");
								AssetDatabase.MoveAsset(AssetDatabase.GetAssetPath(dialogEntry.OptionalAudioClip), $"Assets/Resources/Audio/{newClipName}.wav");
							}

							i++;
						}

						i = 0;
						foreach (var popup in interaction.FailureResult.OptionalPopups)
						{
							int j = 0;
							foreach (var clip in popup.DialogClips)
							{
								if (clip == null)
									continue;
								string newClipName = $"{npc.FirstName} {npc.LastName} {interaction.Name} FAILED Popup {i} - {j}";
								newClipName = newClipName.Replace("'", "");
								AssetDatabase.MoveAsset(AssetDatabase.GetAssetPath(clip), $"Assets/Resources/Audio/{newClipName}.wav");
							}
						}
					}
				}
			}
		}
		Debug.Log("Done!");
	}

	[MenuItem("Company Man Validators/Reduce Video Clips", false, 200)]
	public static void ReduceVideoClips()
	{
		var vids = AssetDatabase.FindAssets("t:videoClip");

		int count = 0;

		foreach (var vidGuid in vids)
		{
			if (count > 10)
				break;

			var vid = AssetDatabase.GUIDToAssetPath(vidGuid);
			if (vid.EndsWith("-reduced.mp4"))
				continue;

			vid = vid.Remove(0, "Assets/".Length);
			vid = $"{Application.dataPath}/{vid}";
			var vidDest = vid.Replace(".mp4", "-reduced.mp4");
			ProcessStartInfo startInfo = new ProcessStartInfo($"{Application.dataPath}/Editor/VideoConverter/VideoConverter.exe");
			startInfo.Arguments = $"\"{vid}\" \"{vidDest}\" 20";
			var p = Process.Start(startInfo);
			p.WaitForExit();

			File.Move($"{vid}.meta", $"{vidDest}.meta");
			File.Delete(vid);

			count++;

			//Rename old .meta file to move asset ref?
		}

		Debug.Log("Done!");
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
