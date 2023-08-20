using Assets.GameModel;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Achievement : ScriptableObject
{
	[HideInInspector]
	public string Id;

	public string Name;
	[TextArea(15, 20)]
	public string Description;
	public Texture2D Image;

	public ActionRequirements Requirements;

	[HideInInspector]
	public bool Completed = false;

	public void Setup()
	{
		Completed = false;
	}
}
