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
		public Texture2D Texture;
		[TextArea(3, 10)]
		public string Text;
	}
}