﻿using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Video;

public class VideoLookup
{
	public static VideoLookup Videos = new VideoLookup("RewardVideos");

	private Dictionary<string, VideoClip> lookup = null;
	private string resourcesDir;

	public VideoLookup(string resourcesDir)
	{
		this.resourcesDir = resourcesDir;
	}

	public VideoClip GetVideo(string name)
	{
		if (string.IsNullOrEmpty(name))
			return null;

		TryBuildLookup();

		if (!lookup.ContainsKey(name))
		{
			Debug.LogError($"Cannot find image in location {resourcesDir} with name {name}");
			return null;
		}

		return lookup[name];
	}

	public List<VideoClip> GetVideos(string[] clipNames)
	{
		List<VideoClip> results = new List<VideoClip>();
		foreach (var clipName in clipNames)
		{
			results.Add(GetVideo(clipName));
		}
		return results;
	}

	public List<string> GetAllContentNames()
	{
		TryBuildLookup();
		return lookup.Keys.ToList();
	}

	private void TryBuildLookup()
	{
		if (lookup == null)
		{
			lookup = new Dictionary<string, VideoClip>();
			var images = Resources.LoadAll<VideoClip>(resourcesDir).ToList();
			foreach (var image in images)
			{
				lookup[image.name] = image;
			}
		}
	}
}

public class ImageLookup
{
	public static ImageLookup Icons = new ImageLookup("LocationIcons");
	public static ImageLookup Backgrounds = new ImageLookup("OfficePics");
	public static ImageLookup Trophies = new ImageLookup("Trophies");
	public static ImageLookup Popups = new ImageLookup("PopupImages");
    public static ImageLookup Policies = new ImageLookup("Policies");
    public static ImageLookup Missions = new ImageLookup("Missions");
    public static ImageLookup StatusSymbols = new ImageLookup("StatusSymbols");

	private Dictionary<string, Sprite> lookup = null;
	private string resourcesDir;

	public ImageLookup(string resourcesDir)
	{
		this.resourcesDir = resourcesDir;
	}

	public Sprite GetImage(string imageName)
	{
		if (string.IsNullOrEmpty(imageName))
			return null;

		TryBuildLookup();

		if (!lookup.ContainsKey(imageName))
		{
			Debug.LogError($"Cannot find image in location {resourcesDir} with name {imageName}");
			return null;
		}

		return lookup[imageName];
	}

	public List<string> GetAllContentNames()
	{
		TryBuildLookup();
		return lookup.Keys.ToList();
	}

	private void TryBuildLookup()
	{
		if (lookup == null)
		{
			lookup = new Dictionary<string, Sprite>();
			var images = Resources.LoadAll<Sprite>(resourcesDir).ToList();
			foreach (var image in images)
			{
				lookup[image.name] = image;
			}
		}
	}

	/*private void TryBuildLookup()
	{
		if (lookup == null)
		{
			var images = Directory.GetFiles(Path.Combine(Application.streamingAssetsPath, resourcesDir), "*.png");

			lookup = new Dictionary<string, Sprite>();
			foreach (var image in images)
			{
				lookup[image.name] = image;
			}
		}
	}

	private Sprite GetSpriteFromAssetPath(string filePath)
	{
		using (var uwr = UnityWebRequestTexture.GetTexture(filePath))
		{
			uwr.SendWebRequest();
		}
	}*/
}

public static class NpcPicManager
{
    private static Dictionary<string, NpcPicsLookup> npcPicsLookups = new Dictionary<string, NpcPicsLookup>();

    public static Sprite GetNpcPicFromId(string npcId, string picId)
    {
		if(!npcPicsLookups.ContainsKey(npcId))
			npcPicsLookups[npcId] = new NpcPicsLookup(npcId);
		return npcPicsLookups[npcId].GetPicOfType(picId);
    }
}

public class NpcPicsLookup
{
	private Dictionary<string, List<Sprite>> picsLookup = new Dictionary<string, List<Sprite>>();
	private string npcId;

	public NpcPicsLookup(string npcId)
	{
		this.npcId = npcId;
		var npcPics = Resources.LoadAll<Sprite>(Path.Combine("NpcPics", npcId)).ToList();
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
				picsLookup[id] = new List<Sprite>();

			picsLookup[id].Add(npcPic);
		}
	}

	public Sprite GetPicOfType(string type)
	{
		if (!picsLookup.ContainsKey(type))
		{
			Debug.LogError($"Failed to find lookup pic for npc {npcId} of type {type}");
			return null;
		}

		var lookup = picsLookup[type];
		return lookup[Random.Range(0, lookup.Count)];
	}
}
