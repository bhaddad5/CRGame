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
			CustomNpcId,
		}
		public Speaker CurrSpeaker;
		[TextArea(3, 10)]
		public string Text;

		[Header("Additional Options")]
		public Npc CustomSpeakerReference;
		public List<Texture2D> CustomNpcImageOptions;

	}
}