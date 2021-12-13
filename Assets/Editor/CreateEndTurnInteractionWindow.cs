using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Assets.GameModel;
using UnityEditor;
using UnityEngine;

public class CreateEndTurnInteractionWindow : EditorWindow
{
	private static CreateEndTurnInteractionWindow window;

	[MenuItem("Company Man/Create Automatic End Turn Event")]

	static void Init()
	{
		window = (CreateEndTurnInteractionWindow)EditorWindow.GetWindow(typeof(CreateEndTurnInteractionWindow));
		window.data = AssetDatabase.LoadAssetAtPath<GameData>("Assets/Data/GameData.asset");
		window.Show();
	}

	private GameData data;
	private string interactionName;

	void OnGUI()
	{
		interactionName = EditorGUILayout.TextField("Name:", interactionName);

		if (GUILayout.Button("Create!"))
		{
			CreateInteraction();

			window.Close();
		}
	}

	private void CreateInteraction()
	{
		Interaction interaction = ScriptableObject.CreateInstance<Interaction>();
		interaction.Name = interactionName;
		interaction.Id = Guid.NewGuid().ToString();
		
		data.EndOfTurnInteractions.Add(interaction);
		EditorUtility.SetDirty(data);


		AssetDatabase.CreateAsset(interaction, $"Assets/Data/_EndOfTurnInteractions/{interaction.Name.ToFolderName()}.asset");
		AssetDatabase.SaveAssets();
	}

}