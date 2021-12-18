using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Assets.GameModel;
using UnityEditor;
using UnityEngine;

public class CreateInteractionWindow : EditorWindow
{
	private static CreateInteractionWindow window;

	[MenuItem("Company Man/Create Interaction")]

	static void Init()
	{
		window = (CreateInteractionWindow)EditorWindow.GetWindow(typeof(CreateInteractionWindow));
		window.data = AssetDatabase.LoadAssetAtPath<GameData>("Assets/Data/GameData.asset");
		window.Show();
	}

	private GameData data;
	private string interactionName;
	private NpcPicker npcPicker = new NpcPicker();

	void OnGUI()
	{
		interactionName = EditorGUILayout.TextField("Name:", interactionName);

		npcPicker.DrawNpcDropdown(data);

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

		var foundNpc = npcPicker.Npc as Npc;

		if (foundNpc == null)
			return;


		foundNpc.Interactions.Add(interaction);
		EditorUtility.SetDirty(foundNpc);

		var npcFolder = Path.GetDirectoryName(AssetDatabase.GetAssetPath(foundNpc));
		
		AssetDatabase.CreateAsset(interaction, $"{npcFolder}/Interactions/{interaction.Name.ToFolderName()}.asset");
		AssetDatabase.SaveAssets();
	}

}