using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

namespace Assets.GameModel
{
	[Serializable]
	public struct Popup
	{
		public string Title;
		public List<VideoClip> Videos;
		public List<Texture2D> Textures;
		public List<AudioClip> DialogClips;
		[TextArea(3, 10)]
		public string Text;
	}
}