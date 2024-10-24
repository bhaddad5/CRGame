﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Assets.GameModel;
using Assets.UI_System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.GameModel.UiDisplayers
{
	public class LocationScreenBindings : MonoBehaviour
	{
		[SerializeField] private TMP_Text Name;
		[SerializeField] private TMP_Text Description;
		[SerializeField] private Image BackgroundImage;
		[SerializeField] private Transform NpcOptionsParent;

		[SerializeField] private Transform PoliciesButton;
		[SerializeField] private PoliciesPopupBindings PoliciesPopupPrefab;

		[SerializeField] private Transform MissionsButton;
		[SerializeField] private MissionsPopupBindings MissionsPopupPrefab;

		[SerializeField] private LocationNpcEntryBindings _npcButtonPrefab;
		[SerializeField] private NpcScreenBindings _npcUiPrefab;
		
		[SerializeField] private TrophyCaseBindings TrophyCasePrefab;
		private TrophyCaseBindings trophyCase;

		[SerializeField] private StatusSymbolsBindings StatusSymbolsPrefab;
		private StatusSymbolsBindings statusSymbols;

		[SerializeField] private GameObject PoliciesNewIndicator;

		private Location loc;
		public bool IsAccessible(MainGameManager mgm) => loc.IsAccessible(mgm);

		private MainGameManager mgm;
		private Action onClose;
		public void Setup(Location loc, MainGameManager mgm, Action onClose)
		{
			this.loc = loc;
			this.mgm = mgm;
			this.onClose = onClose;

			PoliciesNewIndicator.SetActive(loc.Policies.Any(p => p.IsNew(mgm)));

			foreach (Npc npc in loc.Npcs)
			{
				var f = Instantiate(_npcButtonPrefab, NpcOptionsParent);
				f.Setup(npc, this, mgm);
			}

			if (loc.Policies.Count == 0)
				PoliciesButton.gameObject.SetActive(false);

			if (loc.Missions.Count == 0)
				MissionsButton.gameObject.SetActive(false);
			
			if (loc.ShowTrophyCase)
			{
				trophyCase = Instantiate(TrophyCasePrefab);
				trophyCase.UpdateVisuals(mgm);
			}

			if (loc.ShowCar)
			{
				statusSymbols = Instantiate(StatusSymbolsPrefab);
				statusSymbols.UpdateVisuals(mgm);
			}

			AudioHandler.Instance.SetBackgroundAmbiance(loc.OptionalBackgroundAudio);
		}

		public void CloseCurrentLocation()
		{
			GameObject.Destroy(gameObject);
			onClose();
		}

		void OnDestroy()
		{
			if (trophyCase != null)
				GameObject.Destroy(trophyCase.gameObject);
			if(statusSymbols != null)
				GameObject.Destroy(statusSymbols.gameObject);
			if(currNpc != null)
				GameObject.Destroy(currNpc);
		}

		public void OpenPolicies()
		{
			var popupParent = GameObject.Instantiate(UiPrefabReferences.Instance.PopupOverlayParent, transform);
			var policiesPopup = GameObject.Instantiate(PoliciesPopupPrefab, popupParent.transform);
			policiesPopup.Setup(loc, mgm);
		}

		public void OpenMissions()
		{
			var popupParent = GameObject.Instantiate(UiPrefabReferences.Instance.PopupOverlayParent, transform);
			var missionsPopup = GameObject.Instantiate(MissionsPopupPrefab, popupParent.transform);
			missionsPopup.Setup(loc);
		}

		private GameObject currNpc;
		public void ShowNpc(Npc npc, MainGameManager mgm)
		{
			currNpc = Instantiate(_npcUiPrefab).gameObject;
			currNpc.GetComponent<NpcScreenBindings>().Setup(npc, mgm, () => RefreshUiDisplay(mgm));

		}

		public void CloseCurrentNpc()
		{
			if (currNpc != null)
			{
				GameObject.Destroy(currNpc);
				RefreshUiDisplay(mgm);
			}
		}

		public void RefreshUiDisplay(MainGameManager mgm)
		{
			if (loc.ShowMyOfficeCustomBackground)
				BackgroundImage.sprite = mgm.Data.PlayerPromotionLevels[mgm.Data.Promotion].PlayerOfficeBackground.ToSprite();
			else if (loc.ShowMyHome)
				BackgroundImage.sprite = mgm.Data.PlayerHomeLevels[mgm.Data.Home].BackgroundImage.ToSprite();
			else
				BackgroundImage.sprite = loc.BackgroundImage.ToSprite();

			if (loc.ShowMyHome)
				Name.text = $"My Home - {UiDisplayHelpers.ApplyDynamicValuesToString(mgm.Data.PlayerHomeLevels[mgm.Data.Home].HomeName, mgm)}";
			else
				Name.text = UiDisplayHelpers.ApplyDynamicValuesToString(loc.Name, mgm);
			Description.text = loc.Description;
			if (loc.Controlled)
				Name.text += $" (Controlled)";

			PoliciesNewIndicator.SetActive(loc.Policies.Any(p => p.IsNew(mgm)));

			foreach (var npc in NpcOptionsParent.GetComponentsInChildren<LocationNpcEntryBindings>(true))
			{
				//Were they just moved/removed?
				if(!loc.Npcs.Contains(npc.npc))
					GameObject.Destroy(npc.gameObject);
				else
					npc.RefreshUiDisplay(mgm);
			}
		}
	}
}