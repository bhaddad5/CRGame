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
		public List<string> DialogClipsTmp
		{
			get
			{
				var res = new List<string>();
				foreach (var track in DialogClips)
				{
					res.Add(track.name);
				}
				return res;
			}
		}
		[TextArea(3, 10)]
		public string Text;
	}
}