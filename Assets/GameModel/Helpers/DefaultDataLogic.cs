using System;
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
					for (int i = 0; i < interaction.Requirements.NpcStatRequirements.Count; i++)
					{
						var npcReq = interaction.Requirements.NpcStatRequirements[i];
						if (npcReq.OptionalNpcReference == null)
							npcReq.OptionalNpcReference = npc;
						interaction.Requirements.NpcStatRequirements[i] = npcReq;
					}

					for (int i = 0; i < interaction.Result.Dialogs.Count; i++)
					{
						var dialog = interaction.Result.Dialogs[i];
						if (dialog.OptionalNpcReference == null)
							dialog.OptionalNpcReference = npc;

						if (dialog.CustomBackground == null)
						{
							dialog.CustomBackground = npc.BackgroundImage;
							dialog.CustomBackgroundNpcLayout = npc.PersonalLayout;
						}

						interaction.Result.Dialogs[i] = dialog;
					}
					
					for (int i = 0; i < interaction.FailureResult.Dialogs.Count; i++)
					{
						var dialog = interaction.FailureResult.Dialogs[i];
						if (dialog.OptionalNpcReference == null)
							dialog.OptionalNpcReference = npc;

						if (dialog.CustomBackground == null)
						{
							dialog.CustomBackground = npc.BackgroundImage;
							dialog.CustomBackgroundNpcLayout = npc.PersonalLayout;
						}

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

		foreach (var interaction in gameData.StartOfTurnInteractions)
		{
			for (int i = 0; i < interaction.Requirements.NpcStatRequirements.Count; i++)
			{
				var npcReq = interaction.Requirements.NpcStatRequirements[i];
				if (npcReq.OptionalNpcReference == null)
					throw new Exception($"No npc reference provided for interaction {interaction}");
			}
			
			for (int i = 0; i < interaction.Result.Dialogs.Count; i++)
			{
				var dialog = interaction.Result.Dialogs[i];
				if (dialog.OptionalNpcReference == null)
					throw new Exception($"No npc reference provided for interaction {interaction}");

				if (dialog.CustomBackground == null)
					throw new Exception($"No dialog background image provided for interaction {interaction}");
			}

			for (int i = 0; i < interaction.FailureResult.Dialogs.Count; i++)
			{
				var dialog = interaction.FailureResult.Dialogs[i];
				if (dialog.OptionalNpcReference == null)
					throw new Exception($"No npc reference provided for interaction {interaction}");

				if (dialog.CustomBackground == null)
					throw new Exception($"No dialog background image provided for interaction {interaction}");
			}

			for (int i = 0; i < interaction.Result.Effect.NpcEffects.Count; i++)
			{
				var npcEffect = interaction.Result.Effect.NpcEffects[i];
				if (npcEffect.OptionalNpcReference == null)
					throw new Exception($"No npc reference provided for interaction {interaction}");
			}
		}
	}
}
