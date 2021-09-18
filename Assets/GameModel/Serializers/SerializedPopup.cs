using System;
using System.Collections;
using System.Collections.Generic;
using Assets.GameModel;
using UnityEngine;
using UnityEngine.Video;

namespace GameModel.Serializers
{
	[Serializable]
	public struct SerializedPopup
	{
		public string Title;
		public List<string> Videos;
		public string Texture;
		public string Text;

		public static SerializedPopup Serialize(Popup ob)
		{
			List<string> videos = new List<string>();
			foreach (var video in ob.Videos)
			{
				videos.Add(video.name);
			}

			return new SerializedPopup()
			{
				Title = ob.Title,
				Text = ob.Text,
				Videos = videos,
				Texture = ob.Texture?.GetName(),
			};
		}

		public static Popup Deserialize(SerializedPopup ob)
		{
			var res = new Popup()
			{
				Title = ob.Title,
				Text = ob.Text,
				Videos = VideoLookup.Videos.GetVideos(ob.Videos.ToArray()),
				Texture = ImageLookup.Popups.GetImage(ob.Texture),
			};

			return res;
		}

		public static Popup ResolveReferences(DeserializedDataAccessor deserializer, Popup data, SerializedPopup ob)
		{
			return data;
		}
	}
}