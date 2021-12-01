using System;
using System.Collections;
using System.Collections.Generic;
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
		interaction.InteractionResults = new List<InteractionResult>();
		interaction.InteractionResults.Add(new InteractionResult(){Probability = 1});

		var foundNpc = data.FindNpc(npcPicker.NpcId);

		if (foundNpc == null)
			return;


		foundNpc.Item2.Interactions.Add(interaction);
		EditorUtility.SetDirty(foundNpc.Item2);
		
		AssetDatabase.CreateAsset(interaction, $"Assets/Data/{foundNpc.Item1.Name}/{foundNpc.Item2.NpcFileName()}/Interactions/{interaction.Name}.asset");
		AssetDatabase.SaveAssets();
	}

}