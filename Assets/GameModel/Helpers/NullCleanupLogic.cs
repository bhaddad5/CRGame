using Assets.GameModel;

public static class NullCleanupLogic
{
	public static void CleanUpAnnoyingNulls(GameData gameData)
	{
		gameData.Locations.RemoveAll(i => i == null);
		gameData.StartOfTurnInteractions.RemoveAll(i => i == null);

		foreach (var location in gameData.Locations)
		{
			location.Npcs.RemoveAll(i => i == null);
			location.Policies.RemoveAll(i => i == null);
			location.Missions.RemoveAll(i => i == null);

			foreach (var npc in location.Npcs)
			{
				npc.Interactions.RemoveAll(i => i == null);

				foreach (var interaction in npc.Interactions)
				{
					CleanUpInteractionNulls(interaction);
				}
			}
		}

		foreach (var interaction in gameData.StartOfTurnInteractions)
		{
			CleanUpInteractionNulls(interaction);
		}
	}

	private static void CleanUpInteractionNulls(Interaction interaction)
	{
		interaction.Requirements.RequiredDepartmentsControled.RemoveAll(i => i == null);
		interaction.Requirements.RequiredInteractions.RemoveAll(i => i == null);
		interaction.Requirements.RequiredNotCompletedInteractions.RemoveAll(i => i == null);
		interaction.Requirements.RequiredNpcsControled.RemoveAll(i => i == null);
		interaction.Requirements.RequiredNpcsNotControled.RemoveAll(i => i == null);
		interaction.Requirements.RequiredNpcsTrained.RemoveAll(i => i == null);
		interaction.Requirements.RequiredPolicies.RemoveAll(i => i == null);
		interaction.Requirements.RequiredTrophies.RemoveAll(i => i == null);

		interaction.Result.Effect.LocationsToControl.RemoveAll(i => i == null);
		interaction.Result.Effect.NpcsToControl.RemoveAll(i => i == null);
		interaction.Result.Effect.NpcsToRemoveFromGame.RemoveAll(i => i == null);
		interaction.Result.Effect.NpcsToTrain.RemoveAll(i => i == null);
		interaction.Result.Effect.MissionsToComplete.RemoveAll(i => i == null);
		interaction.Result.Effect.TrophiesClaimedReferences.RemoveAll(i => i == null);

		interaction.FailureResult.Effect.LocationsToControl.RemoveAll(i => i == null);
		interaction.FailureResult.Effect.NpcsToControl.RemoveAll(i => i == null);
		interaction.FailureResult.Effect.NpcsToRemoveFromGame.RemoveAll(i => i == null);
		interaction.FailureResult.Effect.NpcsToTrain.RemoveAll(i => i == null);
		interaction.FailureResult.Effect.MissionsToComplete.RemoveAll(i => i == null);
		interaction.FailureResult.Effect.TrophiesClaimedReferences.RemoveAll(i => i == null);

		for (int i = 0; i < interaction.Result.Dialogs.Count; i++)
		{
			var dialog = interaction.Result.Dialogs[i];
			dialog.CustomNpcImageOptions.RemoveAll(opt => opt == null);
			interaction.Result.Dialogs[i] = dialog;
		}
	}
}
