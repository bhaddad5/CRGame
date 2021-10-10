using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Video;

public static class NpcPicManager
{
    private static Dictionary<string, NpcPicsLookup> npcPicsLookups = new Dictionary<string, NpcPicsLookup>();

    public static Texture2D GetNpcPicFromId(string npcId, string picId)
    {
		if(!npcPicsLookups.ContainsKey(npcId))
			npcPicsLookups[npcId] = new NpcPicsLookup(npcId);
		return npcPicsLookups[npcId].GetPicOfType(picId);
    }
}

public class NpcPicsLookup
{
	private Dictionary<string, List<Texture2D>> picsLookup = new Dictionary<string, List<Texture2D>>();
	private string npcId;

	public NpcPicsLookup(string npcId)
	{
		this.npcId = npcId;
		var npcPics = Resources.LoadAll<Texture2D>(Path.Combine("NpcPics", npcId)).ToList();
		foreach (var npcPic in npcPics)
		{
			string id = "";
			var splitName = npcPic.name.Split('_');
			for (int i = 0; i < splitName.Length - 1; i++)
			{
				id += "_" + splitName[i];
			}
			id = id.Substring(1);
			if (!picsLookup.ContainsKey(id))
				picsLookup[id] = new List<Texture2D>();

			picsLookup[id].Add(npcPic);
		}
	}

	public Texture2D GetPicOfType(string type)
	{
		if (!picsLookup.ContainsKey(type))
		{
			Debug.LogError($"Failed to find lookup pic for npc {npcId} of type {type}");
			return null;
		}

		var lookup = picsLookup[type];
		UnityEngine.Random.InitState(System.DateTime.Now.Millisecond);
		return lookup[Random.Range(0, lookup.Count)];
	}
}
