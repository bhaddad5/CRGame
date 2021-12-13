using System;
using System.Collections;
using System.Collections.Generic;
using Assets.GameModel;
using Assets.GameModel.UiDisplayers;
using UnityEngine;

public class InteractionResultDisplayManager
{
	private List<DialogEntry> currDialogsToShow = new List<DialogEntry>();
	private List<Popup> currPopupsToShow = new List<Popup>();
	private List<Mission> currMissionsToShow = new List<Mission>();
	private Action resultComplete = null;
	private int completedCount;
	private Npc contextualNpc;
	private NpcUiDisplay contextualNpcUiDisplay;
	private MainGameManager mgm;

	public void DisplayInteractionResult(MainGameManager mgm, int completionCount, InteractionResult res, Action resultComplete, Npc contextualNpc = null, NpcUiDisplay contextualNpcDisplay = null)
	{
		this.mgm = mgm;
		this.completedCount = completionCount;
		this.resultComplete = resultComplete;
		this.contextualNpc = contextualNpc;
		this.contextualNpcUiDisplay = contextualNpcDisplay;

		currDialogsToShow = new List<DialogEntry>(res.Dialogs);
		currPopupsToShow = new List<Popup>(res.OptionalPopups);
		currMissionsToShow = new List<Mission>(res.Effect.MissionsToComplete);

		HandleNextDialog();
	}

	private void HandleNextDialog()
	{
		if (currDialogsToShow.Count > 0)
		{
			var dialog = currDialogsToShow[0];
			currDialogsToShow.RemoveAt(0);
			mgm.ShowDialog(dialog, HandleNextDialog, contextualNpc, contextualNpcUiDisplay);
		}
		else if (currPopupsToShow.Count > 0)
		{
			var popup = currPopupsToShow[0];
			currPopupsToShow.RemoveAt(0);
			mgm.ShowPopup(popup, completedCount, HandleNextDialog);
		}
		else if (currMissionsToShow.Count > 0)
		{
			var missionPopup = new Popup()
			{
				Title = $"Mission Complete: {currMissionsToShow[0].MissionName}",
				Texture = currMissionsToShow[0].MissionImage,
				Text = currMissionsToShow[0].MissionDescription,
			};
			currMissionsToShow.RemoveAt(0);
			mgm.ShowPopup(missionPopup, completedCount, HandleNextDialog);
		}
		else
		{
			resultComplete?.Invoke();
		}
	}
}
