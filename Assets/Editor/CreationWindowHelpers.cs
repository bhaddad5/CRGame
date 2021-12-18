using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Assets.GameModel;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

public static class CreationWindowHelpers
{
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
	public Object Location;

	public void DrawLocationDropdown(GameData gameData)
	{
		Location = EditorGUILayout.ObjectField("Location: ", Location, typeof(Location), false);
	}
}

public class NpcPicker
{
	public Object Npc;

	public void DrawNpcDropdown(GameData gameData)
	{
		Npc = EditorGUILayout.ObjectField("NPC: ", Npc, typeof(Npc), false);
	}
}