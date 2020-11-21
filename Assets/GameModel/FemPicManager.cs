using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

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
