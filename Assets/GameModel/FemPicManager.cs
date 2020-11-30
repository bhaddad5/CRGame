using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public static class LocationIconLookup
{
	private static Dictionary<string, Sprite> iconsLookup = null;

	public static Sprite GetLocationIcon(string iconId)
	{
		if (string.IsNullOrEmpty(iconId))
			return null;

		if (iconsLookup == null)
		{
			iconsLookup = new Dictionary<string, Sprite>();
			var icons = Resources.LoadAll<Sprite>("LocationIcons").ToList();
			foreach (var icon in icons)
			{
				iconsLookup[icon.name] = icon;
			}
		}

		if (!iconsLookup.ContainsKey(iconId))
		{
			Debug.LogError($"Cannot find location icon with id {iconId}");
			return null;
		}

		return iconsLookup[iconId];
	}
}

public static class BackgroundImagesLookup
{
	private static Dictionary<string, Sprite> backgroundsLookup = null;

	public static Sprite GetBackgroundImage(string backgroundId)
	{
		if (string.IsNullOrEmpty(backgroundId))
			return null;

		if (backgroundsLookup == null)
		{
			backgroundsLookup = new Dictionary<string, Sprite>();
			var icons = Resources.LoadAll<Sprite>("OfficePics").ToList();
			foreach (var icon in icons)
			{
				backgroundsLookup[icon.name] = icon;
			}
		}

		if (!backgroundsLookup.ContainsKey(backgroundId))
		{
			Debug.LogError($"Cannot find location icon with id {backgroundId}");
			return null;
		}

		return backgroundsLookup[backgroundId];
	}
}

public static class FemPicManager
{
    private static Dictionary<string, FemPicsLookup> femPicsLookups = new Dictionary<string, FemPicsLookup>();

    public static Sprite GetFemPicFromId(string femId, string picId)
    {
		if(!femPicsLookups.ContainsKey(femId))
			femPicsLookups[femId] = new FemPicsLookup(femId);
		return femPicsLookups[femId].GetPicOfType(picId);
    }
}

public class FemPicsLookup
{
	private Dictionary<string, List<Sprite>> picsLookup = new Dictionary<string, List<Sprite>>();
	private string femId;

	public FemPicsLookup(string femId)
	{
		this.femId = femId;
		var femPics = Resources.LoadAll<Sprite>(Path.Combine("FemPics", femId)).ToList();
		foreach (var femPic in femPics)
		{
			string id = "";
			var splitName = femPic.name.Split('_');
			for (int i = 0; i < splitName.Length - 1; i++)
			{
				id += "_" + splitName[i];
			}
			id = id.Substring(1);
			if (!picsLookup.ContainsKey(id))
				picsLookup[id] = new List<Sprite>();

			picsLookup[id].Add(femPic);
		}
	}

	public Sprite GetPicOfType(string type)
	{
		if (!picsLookup.ContainsKey(type))
		{
			Debug.LogError($"Failed to find lookup pic for fem {femId} of type {type}");
			return null;
		}

		var lookup = picsLookup[type];
		return lookup[Random.Range(0, lookup.Count)];
	}
}
