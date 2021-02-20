using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public static class EditorTools
{
	[MenuItem("Tools/Export Content Maps")]
	public static void ExportContentMaps()
	{
		string contentMap = "ContentNames:\n\n";

		contentMap += ContentNamesToString("Videos", VideoLookup.Videos.GetAllContentNames());
		contentMap += ContentNamesToString("Popup Images", ImageLookup.Popups.GetAllContentNames());
		contentMap += ContentNamesToString("Trophy Images", ImageLookup.Trophies.GetAllContentNames());
		contentMap += ContentNamesToString("Policy Images", ImageLookup.Policies.GetAllContentNames());
		contentMap += ContentNamesToString("Office Background Images", ImageLookup.Backgrounds.GetAllContentNames());

		var path = Path.Combine(Application.streamingAssetsPath, "ContentMap.txt");
		var fs = File.Create(path);
		fs.Close();
		File.WriteAllText(path, contentMap);

		Debug.Log($"Finished exporting Content Map to {path}");
	}

	private static string ContentNamesToString(string title, List<string> names)
	{
		string res = $"{title}:\n";
		foreach (var name in names)
		{
			res += $"\t{name}\n";
		}

		res += "\n";
		return res;
	}
}
