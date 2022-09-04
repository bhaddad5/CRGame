using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Assets.GameModel;
using UnityEditor;
using UnityEngine;

public class CreateItemWindow : EditorWindow
{
	private static CreateItemWindow window;

	[MenuItem("Company Man/Create Item")]

	static void Init()
	{
		window = (CreateItemWindow)EditorWindow.GetWindow(typeof(CreateItemWindow));
		window.data = AssetDatabase.LoadAssetAtPath<GameData>("Assets/Data/GameData.asset");
		window.Show();
	}

	private GameData data;
	private string itemName;

	void OnGUI()
	{
		itemName = EditorGUILayout.TextField("Name:", itemName);

		if (GUILayout.Button("Create!"))
		{
			CreateInteraction();

			window.Close();
		}
	}

	private void CreateInteraction()
	{
		InventoryItem item = ScriptableObject.CreateInstance<InventoryItem>();
		item.Name = itemName;
		item.Id = Guid.NewGuid().ToString();

		AssetDatabase.CreateAsset(item, $"Assets/Data/_Items/{item.Name.ToFolderName()}.asset");
		AssetDatabase.SaveAssets();
	}

}