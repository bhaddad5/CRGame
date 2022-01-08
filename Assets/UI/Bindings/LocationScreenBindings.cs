using System;
using System.Collections;
using System.Collections.Generic;
using Assets.GameModel;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.GameModel.UiDisplayers
{
	public class LocationScreenBindings : MonoBehaviour, IUiDisplay
	{
		[SerializeField] private TMP_Text Name;
		[SerializeField] private TMP_Text Description;
		[SerializeField] private Image BackgroundImage;
		[SerializeField] private Transform NpcOptionsParent;

		[SerializeField] private Transform PoliciesButton;
		[SerializeField] private PoliciesPopupBindings PoliciesPopupPrefab;

		[SerializeField] private Transform MissionsButton;
		[SerializeField] private MissionsPopupBindings MissionsPopupPrefab;

		[SerializeField] private NpcSelectionUiDisplay _npcButtonPrefab;
		[SerializeField] private NpcScreenBindings _npcUiPrefab;
		
		[SerializeField] private TrophyCaseBindings TrophyCasePrefab;
		private TrophyCaseBindings trophyCase;

		[SerializeField] private StatusSymbolsBindings StatusSymbolsPrefab;
		private StatusSymbolsBindings statusSymbols;

		private Location loc;
		public bool IsAccessible(MainGameManager mgm) => loc.IsAccessible(mgm);

		private MainGameManager mgm;
		private MainMapScreenBindings mguid;
		public void Setup(Location loc, MainMapScreenBindings mguid, MainGameManager mgm)
		{
			this.loc = loc;
			this.mgm = mgm;
			this.mguid = mguid;

			foreach (Npc npc in loc.Npcs)
			{
				var f = Instantiate(_npcButtonPrefab);
				f.Setup(npc, this, mgm);
				f.transform.SetParent(NpcOptionsParent);
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
		}

		public void CloseCurrentLocation()
		{
			mguid.CloseCurrentDepartment(false);
		}

		void OnDestroy()
		{
			if (trophyCase != null)
				GameObject.Destroy(trophyCase.gameObject);
			if(statusSymbols != null)
				GameObject.Destroy(statusSymbols.gameObject);
		}

		public void OpenPolicies()
		{
			var popupParent = GameObject.Instantiate(UiPrefabReferences.Instance.PopupOverlayParent);
			var policiesPopup = GameObject.Instantiate(PoliciesPopupPrefab, popupParent.transform);
			policiesPopup.Setup(loc, mgm);
		}

		public void OpenMissions()
		{
			var popupParent = GameObject.Instantiate(UiPrefabReferences.Instance.PopupOverlayParent);
			var missionsPopup = GameObject.Instantiate(MissionsPopupPrefab, popupParent.transform);
			missionsPopup.Setup(loc);
		}

		private NpcScreenBindings _currOpenNpc;
		public void ShowNpc(Npc npc, MainGameManager mgm)
		{
			_currOpenNpc = Instantiate(_npcUiPrefab);
			_currOpenNpc.Setup(npc, mgm, this);
			_currOpenNpc.RefreshUiDisplay(mgm);
		}

		public void CloseCurrentNpc()
		{
			if (_currOpenNpc != null)
			{
				GameObject.Destroy(_currOpenNpc.gameObject);
				_currOpenNpc = null;
				RefreshUiDisplay(mgm);
			}
		}

		public void RefreshUiDisplay(MainGameManager mgm)
		{
			if (loc.ShowMyOfficeCustomBackground)
				BackgroundImage.sprite = mgm.Data.PlayerPromotionLevels[mgm.Data.Promotion].PlayerOfficeBackground.ToSprite();
			else
				BackgroundImage.sprite = loc.BackgroundImage.ToSprite();
			Name.text = loc.Name;
			Description.text = loc.Description;
			if (loc.Controlled)
				Name.text += $" (Controlled)";

			foreach (var npc in NpcOptionsParent.GetComponentsInChildren<NpcSelectionUiDisplay>(true))
			{
				//Were they just moved/removed?
				if(!loc.Npcs.Contains(npc._npc))
					GameObject.Destroy(npc.gameObject);
				else
					npc.RefreshUiDisplay(mgm);
			}
			
			if (_currOpenNpc != null)
				_currOpenNpc.RefreshUiDisplay(mgm);
		}
	}
}