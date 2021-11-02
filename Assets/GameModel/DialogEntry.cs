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
		public Npc CustomSpeakerReference;
		[TextArea(15, 20)]
		public string Text;

		//Should these be up a level in InteractionResult?
		public bool InPlayerOffice;
		public Texture2D CustomBackground;
		public List<Texture2D> CustomNpcImageOptions;
	}
}