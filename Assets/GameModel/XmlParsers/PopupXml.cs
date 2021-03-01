using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Xml.Serialization;
using UnityEngine;
using UnityEngine.Video;

namespace Assets.GameModel.XmlParsers
{
	public class PopupXml
	{
		[XmlAttribute] [DefaultValue("")] public string Title = "";
		[XmlAttribute] [DefaultValue("")] public string Text = "";
		[XmlAttribute] [DefaultValue("")] public string Image = "";
		[XmlAttribute] [DefaultValue("")] public string Video = "";

		public Popup FromXml()
		{
			List<VideoClip> videos = null;
			if (!string.IsNullOrEmpty(Video))
			{
				var vids = Video.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
				for (int i = 0; i < vids.Length; i++)
				{
					vids[i] = vids[i].Trim();
				}
				videos = VideoLookup.Videos.GetVideos(vids);
			}

			Texture2D tex = null;
			if (!string.IsNullOrEmpty(Image))
				tex = ImageLookup.Popups.GetImage(Image).texture;

			return new Popup()
			{
				Title = Title,
				Text = Text,
				Texture = tex,
				Videos = videos,
			};
		}

		public static PopupXml ToXml(Popup ob)
		{
			string vidsString = "";
			foreach (var videoClip in ob.Videos)
			{
				vidsString += $"{videoClip.name},";
			}
			if (vidsString.EndsWith(","))
				vidsString = vidsString.Substring(0, vidsString.Length - 1);

			var res = new PopupXml()
			{
				Title = ob.Title,
				Text = ob.Text,
				Video = vidsString,
				Image = ob.Texture?.name ?? "",
			};
			
			return res;
		}
	}
}
