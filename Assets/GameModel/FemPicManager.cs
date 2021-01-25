﻿using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
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

		if (lookup == null)
		{
			lookup = new Dictionary<string, VideoClip>();
			var images = Resources.LoadAll<VideoClip>(resourcesDir).ToList();
			foreach (var image in images)
			{
				lookup[image.name] = image;
			}
		}

		if (!lookup.ContainsKey(name))
		{
			Debug.LogError($"Cannot find image in location {resourcesDir} with name {name}");
			return null;
		}

		return lookup[name];
	}
}

public class ImageLookup
{
	public static ImageLookup Icons = new ImageLookup("LocationIcons");
	public static ImageLookup Backgrounds = new ImageLookup("OfficePics");
	public static ImageLookup Trophies = new ImageLookup("Trophies");
	public static ImageLookup Popups = new ImageLookup("PopupImages");

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

		if (lookup == null)
		{
			lookup = new Dictionary<string, Sprite>();
			var images = Resources.LoadAll<Sprite>(resourcesDir).ToList();
			foreach (var image in images)
			{
				lookup[image.name] = image;
			}
		}

		if (!lookup.ContainsKey(imageName))
		{
			Debug.LogError($"Cannot find image in location {resourcesDir} with name {imageName}");
			return null;
		}

		return lookup[imageName];
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
