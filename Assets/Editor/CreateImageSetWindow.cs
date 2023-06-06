using Assets.GameModel;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class CreateImageSetWindow : EditorWindow
{
	private static CreateImageSetWindow window;

	[MenuItem("Company Man/Create Image Set")]

	static void Init()
	{
		window = (CreateImageSetWindow)EditorWindow.GetWindow(typeof(CreateImageSetWindow));
		window.data = AssetDatabase.LoadAssetAtPath<GameData>("Assets/Data/GameData.asset");
		window.Show();
	}

	private GameData data;

	private string name;

	private string picsSrc;

	private NpcPicker npcPicker = new NpcPicker();

	private string errorMsg;

	void OnGUI()
	{
		name = EditorGUILayout.TextField("Name:", name);

		EditorGUILayout.PrefixLabel($"Pics:{picsSrc}");
		if (GUILayout.Button("Choose Pics Source Folder"))
		{
			picsSrc = EditorUtility.OpenFolderPanel("Choose Pics Source Folder", "", "");
		}

		npcPicker.DrawNpcDropdown(data);

		GUILayout.Space(10);

		GUILayout.Label($"Finish:", EditorStyles.boldLabel);

		if (GUILayout.Button("Create Image Set"))
		{
			var npc = npcPicker.Npc as Npc;

			errorMsg = "";

			if (name == null)
			{
				errorMsg = $"ERROR: Image Set not fully named!";
			}
			else if (npc == null)
			{
				errorMsg = $"ERROR: YOU MUST SELECT A NPC";
			}
			else
			{
				CreateImageSet(name, picsSrc, npc);

				window.Close();
			}


		}

		if (!String.IsNullOrEmpty(errorMsg))
			GUILayout.Label(errorMsg, EditorStyles.boldLabel);
	}

	public static ImageSet CreateImageSet(string setName, string srcFolder, Npc npc)
	{
		ImageSet imageSet = ScriptableObject.CreateInstance<ImageSet>();
		imageSet.Name = setName;
		imageSet.Id = Guid.NewGuid().ToString();

		npc.AllImageSets.Add(imageSet);
		EditorUtility.SetDirty(npc);

		AssetDatabase.CreateFolder(Path.GetDirectoryName(AssetDatabase.GetAssetPath(npc)), $"ImageSet-{setName}");

		var imageSetFolder = Path.Combine(Path.GetDirectoryName(AssetDatabase.GetAssetPath(npc)), $"ImageSet-{setName}");

		imageSet.Images = CopyPicsIntoFolder(srcFolder, $"{imageSetFolder}");

		AssetDatabase.CreateAsset(imageSet, $"{imageSetFolder}/{imageSet.Name.ToFolderName()}.asset");
		AssetDatabase.SaveAssets();
		return imageSet;
	}

	public static List<Texture2D> CopyPicsIntoFolder(string srcFolder, string relativeDestFolder)
	{
		List<Texture2D> res = new List<Texture2D>();

		if (String.IsNullOrEmpty(srcFolder))
			return res;

		foreach (var file in Directory.GetFiles(srcFolder, "*.png"))
		{
			//Chop "/Assets" off the end, it's already in the relative path
			var rootGameFolder = Application.dataPath.Substring(0, Application.dataPath.Length - 7);
			var relativeFilePath = Path.Combine(relativeDestFolder, Path.GetFileName(file));
			var destPath = Path.Combine(rootGameFolder, relativeFilePath);
			File.Copy(file, destPath);

			AssetDatabase.ImportAsset(relativeFilePath);
			var tex = AssetDatabase.LoadAssetAtPath<Texture2D>(relativeFilePath);
			Debug.Assert(tex != null);
			res.Add(tex);
		}

		return res;
	}
}
