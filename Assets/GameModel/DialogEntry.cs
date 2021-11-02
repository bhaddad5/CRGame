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
		//Should these be up a level in InteractionResult?
		public Npc CustomSpeakerReference;
		public bool InPlayerOffice;
		public Texture2D CustomBackground;
		public List<Texture2D> CustomNpcImageOptions;

	}
}