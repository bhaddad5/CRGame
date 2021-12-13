using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Assets.GameModel;
using UnityEditor;
using UnityEngine;

public class CreateStartTurnInteractionWindow : EditorWindow
{
	private static CreateStartTurnInteractionWindow window;

	[MenuItem("Company Man/Create Automatic Start of Turn Event")]

	static void Init()
	{
		window = (CreateStartTurnInteractionWindow)EditorWindow.GetWindow(typeof(CreateStartTurnInteractionWindow));
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
		
		data.StartOfTurnInteractions.Add(interaction);
		EditorUtility.SetDirty(data);


		AssetDatabase.CreateAsset(interaction, $"Assets/Data/_StartOfTurnInteractions/{interaction.Name.ToFolderName()}.asset");
		AssetDatabase.SaveAssets();
	}

}