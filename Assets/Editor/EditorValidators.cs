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

					if (npc.RemovedImages.Count > 0)
					{
						npc.RemovedImages.Clear();
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

					if (npc.Trophies.Contains(null))
						EditorUtility.SetDirty(npc);
					npc.Trophies.RemoveAll(v => v == null);
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
					var destFolder = $"{Path.GetDirectoryName(AssetDatabase.GetAssetPath(npc))}/Interactions";
					
					if (npc.OptionalDialogClip != null)
					{
						var newClipName = $"{npc.FirstName} {npc.LastName} Screen Dialog";
						newClipName = newClipName.Replace("'", "");
						AssetDatabase.MoveAsset(AssetDatabase.GetAssetPath(npc.OptionalDialogClip), $"{destFolder}/{Path.GetFileName(newClipName)}.wav");
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
								AssetDatabase.MoveAsset(AssetDatabase.GetAssetPath(dialogEntry.OptionalAudioClip), $"{destFolder}/{Path.GetFileName(newClipName)}.wav");
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
								AssetDatabase.MoveAsset(AssetDatabase.GetAssetPath(clip), $"{destFolder}/{Path.GetFileName(newClipName)}.wav");
							}
						}

						i = 0;
						foreach (var dialogEntry in interaction.FailureResult.Dialogs)
						{
							if (dialogEntry.OptionalAudioClip != null)
							{
								string newClipName = $"{npc.FirstName} {npc.LastName} {interaction.Name} FAILED {i}";
								newClipName = newClipName.Replace("'", "");
								AssetDatabase.MoveAsset(AssetDatabase.GetAssetPath(dialogEntry.OptionalAudioClip), $"{destFolder}/{Path.GetFileName(newClipName)}.wav");
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
								AssetDatabase.MoveAsset(AssetDatabase.GetAssetPath(clip), $"{destFolder}/{Path.GetFileName(newClipName)}.wav");
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
		
		foreach (var vidGuid in vids)
		{
			var vid = AssetDatabase.GUIDToAssetPath(vidGuid);
			if (vid.EndsWith("-reduced.mp4"))
				continue;

			vid = vid.Remove(0, "Assets/".Length);
			vid = $"{Application.dataPath}/{vid}";
			var vidDest = vid.Replace(".mp4", "-reduced.mp4");


			if (File.Exists(vidDest))
			{
				Debug.LogError($"Trying to reduce {vid} but {vidDest} already exists!!!");
				File.Delete(vid);
				continue;
			}

			ProcessStartInfo startInfo = new ProcessStartInfo($"{Application.dataPath}/Editor/VideoConverter/VideoConverter.exe");
			startInfo.Arguments = $"\"{vid}\" \"{vidDest}\" 20";
			var p = Process.Start(startInfo);
			p.WaitForExit();

			File.Move($"{vid}.meta", $"{vidDest}.meta");
			File.Delete(vid);
		}

		Debug.Log("Done!");
	}
	/*
	[MenuItem("Company Man Validators/Upgrade Npcs to use Image Sets")]
	public static void UpgradeOldData()
	{
		var gameData = AssetDatabase.LoadAssetAtPath<GameData>("Assets/Data/GameData.asset");

		foreach (var region in gameData.Regions)
		{
			foreach (var location in region.Locations)
			{
				foreach (var npc in location.Npcs)
				{
					ImageSet imageSet_ind = ScriptableObject.CreateInstance<ImageSet>();
					imageSet_ind.Name = $"{npc.name}-Independent";
					imageSet_ind.Id = Guid.NewGuid().ToString();
					imageSet_ind.Images = npc.IndependentImages;
					npc.AllImageSets.Add(imageSet_ind);
					npc.StartingImageSets.Add(imageSet_ind);
					var imageSetFolder_ind = Path.Combine(Path.GetDirectoryName(AssetDatabase.GetAssetPath(npc)), $"Pics-Independent");
					AssetDatabase.CreateAsset(imageSet_ind, $"{imageSetFolder_ind}/{imageSet_ind.Name.ToFolderName()}.asset");
					npc.Legacy_IndependentImages = imageSet_ind;

					ImageSet imageSet_ctrl = null;
					if (npc.ControlledImages.Count > 0)
					{
						imageSet_ctrl = ScriptableObject.CreateInstance<ImageSet>();
						imageSet_ctrl.Name = $"{npc.name}-Controlled";
						imageSet_ctrl.Id = Guid.NewGuid().ToString();
						imageSet_ctrl.Images = npc.ControlledImages;
						npc.AllImageSets.Add(imageSet_ctrl);
						var imageSetFolder_ctrl = Path.Combine(Path.GetDirectoryName(AssetDatabase.GetAssetPath(npc)), $"Pics-Controlled");
						AssetDatabase.CreateAsset(imageSet_ctrl, $"{imageSetFolder_ctrl}/{imageSet_ctrl.Name.ToFolderName()}.asset");
						npc.Legacy_ControlledImages = imageSet_ctrl;
					}

					ImageSet imageSet_trn = null;
					if (npc.TrainedImages.Count > 0)
					{
						imageSet_trn = ScriptableObject.CreateInstance<ImageSet>();
						imageSet_trn.Name = $"{npc.name}-Trained";
						imageSet_trn.Id = Guid.NewGuid().ToString();
						imageSet_trn.Images = npc.TrainedImages;
						npc.AllImageSets.Add(imageSet_trn);
						var imageSetFolder_trn = Path.Combine(Path.GetDirectoryName(AssetDatabase.GetAssetPath(npc)), $"Pics-Trained");
						AssetDatabase.CreateAsset(imageSet_trn, $"{imageSetFolder_trn}/{imageSet_trn.Name.ToFolderName()}.asset");
						npc.Legacy_TrainedImages = imageSet_trn;
					}

					var ctrlInteractions = ProfilingHelpers.GetAllInteractions().Where(i => i.Result.Effect.NpcsToControl.Contains(npc));
					foreach (var interaction in ctrlInteractions)
					{
						interaction.Result.Effect.NpcEffects.Add(new NpcEffect()
						{
							OptionalNpcReference = npc, 
							ImageSetsToRemove = new List<ImageSet>(){ imageSet_ind },
							ImageSetsToAdd= new List<ImageSet>() { imageSet_ctrl },
						});
						EditorUtility.SetDirty(interaction);
					}

					var trainInteractions = ProfilingHelpers.GetAllInteractions().Where(i => i.Result.Effect.NpcsToTrain.Contains(npc));
					foreach (var interaction in trainInteractions)
					{
						interaction.Result.Effect.NpcEffects.Add(new NpcEffect()
						{
							OptionalNpcReference = npc,
							ImageSetsToRemove = new List<ImageSet>() { imageSet_ctrl },
							ImageSetsToAdd = new List<ImageSet>() { imageSet_trn },
						});
						EditorUtility.SetDirty(interaction);
					}

					EditorUtility.SetDirty(npc);

					AssetDatabase.SaveAssets();
				}
			}
		}

		

		Debug.Log("Upgrade Complete!");
	}
	*/ 

	//TODO: USE THIS AS A TEMPLATE FOR DATA UPGRADES!
	/*
	[MenuItem("Company Man Validators/Upgrade Npcs to use Image Sets")]
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
		Debug.Log("Upgrade Complete!");
	}
	*/
}
