using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Assets.GameModel;
using UnityEditor;
using UnityEngine;

public static class CreationWindowHelpers
{
	public static Npc FindNpc(this GameData gameData, string npcId)
	{
		foreach (var loc in gameData.Locations)
		{
			foreach (var locNpc in loc?.Npcs ?? new List<Npc>())
			{
				if (locNpc?.Id == npcId)
				{
					return locNpc;
				}
			}
		}

		return null;
	}

	public static Location FindLocation(this GameData gameData, string locationId)
	{
		foreach (var loc in gameData.Locations)
		{
			if (loc?.Id == locationId)
			{
				return loc;
			}
		}

		return null;
	}

	public static string NpcFileName(this Npc npc)
	{
		return $"{npc.FirstName} {npc.LastName}";
	}

	public static string ToFolderName(this string name)
	{
		foreach (var fileNameChar in Path.GetInvalidFileNameChars())
		{
			name = name.Replace(fileNameChar.ToString(), "");
		}
		name = name.Replace(".", "");
		return name;
	}
}

public class LocationPicker
{
	public string LocationId;

	public void DrawLocationDropdown(GameData gameData)
	{
		GUILayout.Label($"Location:", EditorStyles.boldLabel);

		var content = new GUIContent($"{LocationId}");
		var dropdownPos = GUILayoutUtility.GetRect(content, GUIStyle.none);
		if (!EditorGUILayout.DropdownButton(content, FocusType.Passive))
		{
			return;
		}


		void handleItemClicked(object dept)
		{
			LocationId = (dept as Location).Id;
		}

		GenericMenu menu = new GenericMenu();
		foreach (var loc in gameData.Locations)
		{
			if (loc == null)
				continue;

			menu.AddItem(new GUIContent(loc.Name), false, handleItemClicked, loc);
		}

		menu.DropDown(dropdownPos);
	}
}

public class NpcPicker
{
	public string NpcId;

	public void DrawNpcDropdown(GameData gameData)
	{
		GUILayout.Label($"NPC:", EditorStyles.boldLabel);

		var content = new GUIContent($"{NpcId}");
		var dropdownPos = GUILayoutUtility.GetRect(content, GUIStyle.none);
		if (!EditorGUILayout.DropdownButton(content, FocusType.Passive))
		{
			return;
		}

		void handleItemClicked(object ob)
		{
			NpcId = (ob as Npc).Id;
		}

		GenericMenu menu = new GenericMenu();
		foreach (var loc in gameData.Locations)
		{
			if (loc == null)
				continue;

			foreach (var locNpc in loc.Npcs)
			{
				if (locNpc == null)
					continue;
				menu.AddItem(new GUIContent(locNpc.NpcFileName()), false, handleItemClicked, locNpc);
			}


		}

		menu.DropDown(dropdownPos);
	}
}