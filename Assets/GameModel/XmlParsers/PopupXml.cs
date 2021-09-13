using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Xml.Serialization;
using GameModel.Serializers;
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

		public SerializedPopup FromXml()
		{
			List<string> videos = null;
			if (!string.IsNullOrEmpty(Video))
			{
				var vids = Video.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
				for (int i = 0; i < vids.Length; i++)
				{
					vids[i] = vids[i].Trim();
				}
				videos = vids.ToList();
			}

			return new SerializedPopup()
			{
				Title = Title,
				Text = Text,
				Texture = Image,
				Videos = videos,
			};
		}
	}
}
