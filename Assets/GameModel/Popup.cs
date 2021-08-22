using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

namespace Assets.GameModel
{
	public class Popup : ScriptableObject
	{
		public string Title;
		public List<VideoClip> Videos;
		public Texture2D Texture;
		public string Text;
	}
}