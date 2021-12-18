using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Assets.GameModel;
using UnityEditor;
using UnityEngine;

public class CreatePolicyWindow : EditorWindow
{
	private static CreatePolicyWindow window;

	[MenuItem("Company Man/Create Policy")]

	static void Init()
	{
		window = (CreatePolicyWindow)EditorWindow.GetWindow(typeof(CreatePolicyWindow));
		window.data = AssetDatabase.LoadAssetAtPath<GameData>("Assets/Data/GameData.asset");
		window.Show();
	}

	private GameData data;
	private string policyName;
	private LocationPicker locPicker = new LocationPicker();

	void OnGUI()
	{
		policyName = EditorGUILayout.TextField("Name:", policyName);

		locPicker.DrawLocationDropdown(data);

		if (GUILayout.Button("Create!"))
		{
			Create();

			window.Close();
		}
	}
	
	private void Create()
	{
		Policy policy = ScriptableObject.CreateInstance<Policy>();
		policy.Name = policyName;
		policy.Id = Guid.NewGuid().ToString();

		var foundLoc = locPicker.Location as Location;

		if (foundLoc == null)
			return;


		foundLoc.Policies.Add(policy);
		EditorUtility.SetDirty(foundLoc);

		var locFolder = Path.GetDirectoryName(AssetDatabase.GetAssetPath(foundLoc));

		AssetDatabase.CreateAsset(policy, $"{locFolder}/_Policies/{policy.Name}.asset");
		AssetDatabase.SaveAssets();
	}

}