using System;
using System.Collections;
using System.Linq;
using Assets.GameModel.UiDisplayers;
using UnityEngine;
using UnityEngine.UI;

public class TesterAI : MonoBehaviour
{
	/*public void Play()
	{
		StartCoroutine(PlayGame());
	}

	private IEnumerator PlayGame()
	{
		while (true)
		{
			
			yield return new WaitForSeconds(.05f);
		}
	}*/

	void Update()
	{
		TakeAction();
	}

	private void TakeAction()
	{
		var popup = GameObject.Find("Popup Overlay Parent(Clone)");
		if (popup != null)
		{
			popup.GetComponentInChildren<Button>(true).onClick.Invoke();
			return;
		}

		var dialogScreen = GameObject.Find("Dialog Screen(Clone)");
		if (dialogScreen != null)
		{
			dialogScreen.GetComponentInChildren<Button>().onClick.Invoke();
			return;
		}

		var choicesScreen = GameObject.Find("Choices Screen(Clone)");
		if (choicesScreen != null)
		{
			var choices = choicesScreen.GetComponentsInChildren<ChoicesEntryBindings>().Where(e => e.GetComponent<Button>().IsInteractable()).ToArray();
			if (choices.Length == 0)
			{
				Debug.LogError("NO CHOICES AVAILABLE!!!!!");
				throw new Exception();
			}

			var choice = TakeRandom(choices);
			choice.GetComponent<Button>().onClick.Invoke();
			return;
		}

		var npcParent = GameObject.Find("Npc Screen(Clone)");
		if (npcParent != null)
		{
			var interactions = npcParent.GetComponentsInChildren<NpcInteractionEntryBindings>().Where(e => e.GetComponent<Button>().IsInteractable()).ToArray();

			if (interactions.Length == 0)
			{
				npcParent.GetComponent<NpcScreenBindings>().CloseNpc();
				return;
			}

			var newInteractions = interactions.Where(i => i.IsNew).ToArray();
			if (newInteractions.Length > 0)
			{
				var interactionToDo = TakeRandom(newInteractions);
				interactionToDo.GetComponent<Button>().onClick.Invoke();
			}
			else if (GoBackUp(.9f))
			{
				npcParent.GetComponent<NpcScreenBindings>().CloseNpc();
			}
			else
			{
				var interactionToDo = TakeRandom(interactions);
				interactionToDo.GetComponent<Button>().onClick.Invoke();
			}
			
			return;
		}

		var locationMapParent = GameObject.Find("Location Screen(Clone)");
		if (locationMapParent != null)
		{
			var npcs = locationMapParent.GetComponentsInChildren<LocationNpcEntryBindings>().Where(e => e.GetComponent<Button>().IsInteractable()).ToArray();

			if (npcs.Length == 0)
			{
				locationMapParent.GetComponent<LocationScreenBindings>().CloseCurrentLocation();
				return;
			}

			var newNpcs = npcs.Where(i => i.IsNew).ToArray();
			if (newNpcs.Length > 0)
			{
				var npcToTalkTo = TakeRandom(newNpcs);
				npcToTalkTo.GetComponent<Button>().onClick.Invoke();
			}
			else if (GoBackUp(.9f))
			{
				locationMapParent.GetComponent<LocationScreenBindings>().CloseCurrentLocation();
			}
			else
			{
				var npcToTalkTo = TakeRandom(npcs);
				npcToTalkTo.GetComponent<Button>().onClick.Invoke();
			}
			
			return;
		}

		var regionMapParent = GameObject.Find("Region Map Screen(Clone)");
		if (regionMapParent != null)
		{
			var locations = regionMapParent.GetComponentsInChildren<RegionMapLocationEntryBindings>().Where(e => e.GetComponent<Button>().IsInteractable()).ToArray();
			locations = locations.Where(l => l.HasNpcs).ToArray();

			if (locations.Length == 0)
			{
				regionMapParent.GetComponent<RegionMapScreenBindings>().CloseRegion();
				return;
			}

			var newLocations = locations.Where(i => i.IsNew).ToArray();
			if (newLocations.Length > 0)
			{
				var locationToEnter = TakeRandom(newLocations);
				locationToEnter.GetComponent<Button>().onClick.Invoke();
			}
			else if (GoBackUp(.9f))
			{
				regionMapParent.GetComponent<RegionMapScreenBindings>().CloseRegion();
			}
			else
			{
				var locationToEnter = TakeRandom(locations);
				locationToEnter.GetComponent<Button>().onClick.Invoke();
			}

			return;
		}

		var worldMapParent = GameObject.Find("World Map Screen(Clone)");

		//We haven't started yet.
		if (worldMapParent == null)
			return;
		
		var regions = worldMapParent.GetComponentsInChildren<WorldMapRegionEntryBindings>().Where(e => e.GetComponent<Button>().IsInteractable()).ToArray();
		if (regions.Length > 0)
		{
			

			var newRegions = regions.Where(i => i.IsNew).ToArray();
			if (newRegions.Length > 0)
			{
				var regionToUse = TakeRandom(newRegions);
				regionToUse.GetComponent<Button>().onClick.Invoke();
			}
			else if (GoBackUp(.9f))
			{
				GameObject.Find("Hud(Clone)").transform.Find("Hud Canvas").Find("Bottom Right").Find("Downtime").GetComponent<Button>().onClick.Invoke();
			}
			else
			{
				var regionToUse = TakeRandom(regions);
				regionToUse.GetComponent<Button>().onClick.Invoke();
			}

			return;
		}
	}

	private T TakeRandom<T>(T[] collection)
	{
		return collection[UnityEngine.Random.Range(0, collection.Count())];
	}

	public static bool GoBackUp(float oddsOfUp)
	{
		var res = UnityEngine.Random.Range(0f, 1f);
		return res <= oddsOfUp;
	}


}
