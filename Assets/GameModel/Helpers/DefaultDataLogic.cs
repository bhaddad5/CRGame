using System.Collections;
using System.Collections.Generic;
using Assets.GameModel;
using UnityEngine;

public static class DefaultDataLogic
{
	public static void ImposeDefaultsOnNullFields(GameData gameData)
	{
		foreach (var location in gameData.Locations)
		{
			foreach (var npc in location.Npcs)
			{
				foreach (var interaction in npc.Interactions)
				{
					for (int i = 0; i < interaction.Requirements.NpcRequirements.Count; i++)
					{
						var npcReq = interaction.Requirements.NpcRequirements[i];
						if (npcReq.OptionalNpcReference == null)
							npcReq.OptionalNpcReference = npc;
						interaction.Requirements.NpcRequirements[i] = npcReq;
					}

					for (int i = 0; i < interaction.Result.Dialogs.Count; i++)
					{
						var dialog = interaction.Result.Dialogs[i];
						if (dialog.CurrSpeaker == DialogEntry.Speaker.Npc && dialog.OptionalNpcReference == null)
							dialog.OptionalNpcReference = npc;
						interaction.Result.Dialogs[i] = dialog;
					}

					for (int i = 0; i < interaction.FailureResult.Dialogs.Count; i++)
					{
						var dialog = interaction.FailureResult.Dialogs[i];
						if (dialog.CurrSpeaker == DialogEntry.Speaker.Npc && dialog.OptionalNpcReference == null)
							dialog.OptionalNpcReference = npc;
						interaction.FailureResult.Dialogs[i] = dialog;
					}

					for (int i = 0; i < interaction.Result.Effect.NpcEffects.Count; i++)
					{
						var npcEffect = interaction.Result.Effect.NpcEffects[i];
						if (npcEffect.OptionalNpcReference == null)
							npcEffect.OptionalNpcReference = npc;
						interaction.Result.Effect.NpcEffects[i] = npcEffect;
					}
				}
			}
		}
	}
}
