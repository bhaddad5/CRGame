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
		public List<VideoClip> Videos;
		public Texture2D Texture;
		public string Text;

		public static SerializedPopup Serialize(Popup ob)
		{
			return new SerializedPopup()
			{
				Title = ob.Title,
				Text = ob.Text,
				Videos = ob.Videos,
				Texture = ob.Texture,
			};
		}

		public static Popup Deserialize(SerializedPopup ob)
		{
			var res = new Popup()
			{
				Title = ob.Title,
				Text = ob.Text,
				Videos = ob.Videos,
				Texture = ob.Texture,
			};

			return res;
		}

		public static Popup ResolveReferences(DeserializedDataAccessor deserializer, Popup data, SerializedPopup ob)
		{
			return data;
		}
	}
}