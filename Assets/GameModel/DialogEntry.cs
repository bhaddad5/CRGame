using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.GameModel
{
	[Serializable]
	public struct DialogEntry
	{
		public enum Speaker
		{
			Npc,
			Player,
			Narrator,
		}
		public Speaker CurrSpeaker;
		[Header("Null defaults to parent npc")]
		public Npc OptionalNpcReference;
		[TextArea(3, 10)]
		public string Text;

		[Header("Additional Options")]
		public List<Texture2D> CustomNpcImageOptions;
		public Texture2D CustomBackground;
		public NpcLayout CustomBackgroundNpcLayout;

		public AudioClip OptionalAudioClip;
		public string OptionalAudioClipTmp
		{
			get
			{
				if (OptionalAudioClip)
					return OptionalAudioClip?.name;
				else
					return null;
			}
		}

		public AudioClip OptionalStartMusicClip;
		public string OptionalStartMusicClipTmp
		{
			get
			{
				if (OptionalStartMusicClip)
					return OptionalStartMusicClip?.name;
				else
					return null;
			}
		}
	}
}