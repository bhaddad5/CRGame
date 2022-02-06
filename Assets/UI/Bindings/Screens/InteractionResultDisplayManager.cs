using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Assets.GameModel;
using Assets.GameModel.UiDisplayers;
using UnityEngine;

public class InteractionResultDisplayManager
{
	private List<DialogEntry> currDialogsToShow = new List<DialogEntry>();
	private List<Popup> currPopupsToShow = new List<Popup>();
	private List<Mission> currMissionsToShow = new List<Mission>();
	private List<Trophy> currTrophiesToShow = new List<Trophy>();
	private Action resultComplete = null;
	private int completedCount;
	private MainGameManager mgm;

	public void DisplayInteractionResult(int completionCount, InteractionResult res, bool failed, MainGameManager mgm, Action resultComplete)
	{
		this.mgm = mgm;
		this.completedCount = completionCount;
		this.resultComplete = resultComplete;

		currDialogsToShow = new List<DialogEntry>(res.Dialogs);
		if (failed && currDialogsToShow.Count > 0)
		{
			var modifiedDialog = currDialogsToShow[0];
			modifiedDialog.Text = $"FAILED: {modifiedDialog.Text}";
			currDialogsToShow[0] = modifiedDialog;
		}

		string effectsString = res.Effect.GetEffectsString();
		if (!String.IsNullOrEmpty(effectsString))
		{
			currDialogsToShow.Add(new DialogEntry(){CurrSpeaker = DialogEntry.Speaker.Narrator, Text = effectsString });
		}
		currPopupsToShow = new List<Popup>(res.OptionalPopups);
		currMissionsToShow = new List<Mission>(res.Effect.MissionsToComplete);
		currTrophiesToShow = new List<Trophy>(res.Effect.TrophiesClaimedReferences);

		HandleNextDialog();
	}

	private void HandleNextDialog()
	{
		if (currDialogsToShow.Count > 0)
		{
			var dialog = currDialogsToShow[0];
			currDialogsToShow.RemoveAt(0);
			GameObject.Instantiate(UiPrefabReferences.Instance.GetPrefabByName("Dialog Screen")).GetComponent<DialogScreenBindings>().Setup(dialog, mgm, HandleNextDialog);
		}
		else if (currPopupsToShow.Count > 0)
		{
			var popup = currPopupsToShow[0];
			currPopupsToShow.RemoveAt(0);

			var popupParent = GameObject.Instantiate(UiPrefabReferences.Instance.PopupOverlayParent);
			GameObject.Instantiate(UiPrefabReferences.Instance.GetPrefabByName("Popup Display"), popupParent.transform).GetComponent<PopupBindings>().Setup(popup, completedCount, mgm, HandleNextDialog);
		}
		else if (currTrophiesToShow.Count > 0)
		{
			var popup = new Popup()
			{
				Title = $"Trophy Claimed: {currTrophiesToShow[0].Name}",
				Texture = currTrophiesToShow[0].Image,
				Text = currTrophiesToShow[0].Description,
			};
			currTrophiesToShow.RemoveAt(0);

			var popupParent = GameObject.Instantiate(UiPrefabReferences.Instance.PopupOverlayParent);
			GameObject.Instantiate(UiPrefabReferences.Instance.GetPrefabByName("Popup Display"), popupParent.transform).GetComponent<PopupBindings>().Setup(popup, completedCount, mgm, HandleNextDialog);
		}
		else if (currMissionsToShow.Count > 0)
		{
			var popup = new Popup()
			{
				Title = $"Mission Complete: {currMissionsToShow[0].MissionName}",
				Texture = currMissionsToShow[0].MissionImage,
				Text = currMissionsToShow[0].MissionDescription,
			};
			currMissionsToShow.RemoveAt(0);

			var popupParent = GameObject.Instantiate(UiPrefabReferences.Instance.PopupOverlayParent);
			GameObject.Instantiate(UiPrefabReferences.Instance.GetPrefabByName("Popup Display"), popupParent.transform).GetComponent<PopupBindings>().Setup(popup, completedCount, mgm, HandleNextDialog);
		}
		else
		{
			resultComplete?.Invoke();
		}
	}
}
