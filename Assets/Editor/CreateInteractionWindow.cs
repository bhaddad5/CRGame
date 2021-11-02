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
	private string npcId;

	void OnGUI()
	{
		interactionName = EditorGUILayout.TextField("Name:", interactionName);

		DrawDropdown();

		if (GUILayout.Button("Create!"))
		{
			CreateInteraction();

			window.Close();
		}
	}

	private void DrawDropdown()
	{
		GUILayout.Label($"NPC:", EditorStyles.boldLabel);

		var content = new GUIContent($"{npcId}");
		var dropdownPos = GUILayoutUtility.GetRect(content, GUIStyle.none);
		if (!EditorGUILayout.DropdownButton(content, FocusType.Passive))
		{
			return;
		}
		
		void handleItemClicked(object ob)
		{
			npcId = (ob as Npc).Id;
		}

		GenericMenu menu = new GenericMenu();
		foreach (var loc in data.Locations)
		{
			if (loc == null)
				continue;

			foreach (var locNpc in loc.Npcs)
			{
				if (locNpc == null)
					continue;
				menu.AddItem(new GUIContent(locNpc.Id), false, handleItemClicked, locNpc);
			}

			
		}

		menu.DropDown(dropdownPos);
	}

	private Tuple<Location,Npc> FindNpc()
	{
		foreach (var loc in data.Locations)
		{
			foreach (var locNpc in loc?.Npcs ?? new List<Npc>())
			{
				if (locNpc?.Id == npcId)
				{
					return new Tuple<Location, Npc>(loc, locNpc);
				}
			}
		}

		return null;
	}

	private void CreateInteraction()
	{
		Interaction interaction = ScriptableObject.CreateInstance<Interaction>();
		interaction.Name = interactionName;
		interaction.Id = npcId+interactionName;

		var foundNpc = FindNpc();

		if (foundNpc == null)
			return;


		foundNpc.Item2.Interactions.Add(interaction);
		EditorUtility.SetDirty(foundNpc.Item2);
		
		AssetDatabase.CreateAsset(interaction, $"Assets/Data/{foundNpc.Item1.Id}/{foundNpc.Item2.Id}/Interactions/{interaction.Id}.asset");
		AssetDatabase.SaveAssets();
	}

}