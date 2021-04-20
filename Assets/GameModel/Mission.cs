﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.GameModel
{
	public class Mission
	{
		public string MissionName;
		public string MissionDescription;
		public Sprite MissionImage;

		public string npcId;
		public string InteractionId;
		public List<Effect> Rewards;

		public bool IsComplete(MainGameManager mgm)
		{
			var interaction = mgm.Data.GetInteractionById(npcId, InteractionId);
			return interaction.Completed;
		}
	}
}
