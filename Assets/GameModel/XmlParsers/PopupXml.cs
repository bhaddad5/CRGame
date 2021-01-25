using System.Collections.Generic;
using System.ComponentModel;
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
			VideoClip vid = null;
			if (!string.IsNullOrEmpty(Video))
				vid = VideoLookup.Videos.GetVideo(Video);

			Texture2D tex = null;
			if (!string.IsNullOrEmpty(Image))
				tex = ImageLookup.Popups.GetImage(Image).texture;

			return new Popup()
			{
				Title = Title,
				Text = Text,
				Texture = tex,
				Video = vid,
			};
		}

		public static PopupXml ToXml(Popup ob)
		{
			var res = new PopupXml()
			{
				Title = ob.Title,
				Text = ob.Text,
				Video = ob.Video?.name ?? "",
				Image = ob.Texture?.name ?? "",
			};
			
			return res;
		}
	}
}
