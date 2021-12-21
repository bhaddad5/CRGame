using System;
using System.Collections;
using System.Collections.Generic;
using Assets.GameModel;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.GameModel.UiDisplayers
{
	public class LocationUiDisplay : MonoBehaviour, IUiDisplay
	{
		[SerializeField] private TMP_Text Name;
		[SerializeField] private Image BackgroundImage;
		[SerializeField] private Transform NpcOptionsParent;
		[SerializeField] private Button BackButton;

		[SerializeField] private Transform PoliciesButton;
		[SerializeField] private Transform PoliciesPopup;
		[SerializeField] private Transform PolicyOptionsParent;
		[SerializeField] private PolicyUiDisplay SpecificPolicyPopup;

		[SerializeField] private Transform MissionsButton;
		[SerializeField] private Transform MissionsPopup;
		[SerializeField] private Transform MissionOptionsParent;

		[SerializeField] private NpcSelectionUiDisplay _npcButtonPrefab;
		[SerializeField] private NpcUiDisplay _npcUiPrefab;

		[SerializeField] private PolicySelectionUiDisplay policyPrefab;
		[SerializeField] private MissionUiDisplay misisonPrefab;

		[SerializeField] private TrophyCaseUiDisplay TrophyCasePrefab;
		private TrophyCaseUiDisplay trophyCase;

		[SerializeField] private StatusSymbolsDisplay StatusSymbolsPrefab;
		private StatusSymbolsDisplay statusSymbols;

		private Location loc;
		public bool IsAccessible(MainGameManager mgm) => loc.IsAccessible(mgm);

		private MainGameManager mgm;
		public void Setup(Location loc, MainMapUiDisplay mguid, MainGameManager mgm)
		{
			this.loc = loc;
			this.mgm = mgm;
			BackButton.onClick.RemoveAllListeners();
			BackButton.onClick.AddListener(() => mguid.CloseCurrentDepartment(false));

			foreach (Npc npc in loc.Npcs)
			{
				var f = Instantiate(_npcButtonPrefab);
				f.Setup(npc, this, mgm);
				f.transform.SetParent(NpcOptionsParent);
			}

			if (loc.Policies.Count == 0)
				PoliciesButton.gameObject.SetActive(false);
			foreach (Policy policy in loc.Policies)
			{
				var p = Instantiate(policyPrefab);
				p.Setup(policy, loc, this);
				p.transform.SetParent(PolicyOptionsParent);
			}

			if (loc.Missions.Count == 0)
				MissionsButton.gameObject.SetActive(false);
			foreach (Mission mission in loc.Missions)
			{
				var m = Instantiate(misisonPrefab);
				m.Setup(mission, loc, mgm);
				m.transform.SetParent(MissionOptionsParent);
			}

			ClosePolicies();
			CloseMissions();
			ClosePolicy();

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

		void OnDestroy()
		{
			if (trophyCase != null)
				GameObject.Destroy(trophyCase.gameObject);
			if(statusSymbols != null)
				GameObject.Destroy(statusSymbols.gameObject);
		}

		public void OpenPolicies()
		{
			PoliciesPopup.gameObject.SetActive(true);
		}

		public void ClosePolicies()
		{
			PoliciesPopup.gameObject.SetActive(false);
		}

		public void OpenPolicy(Policy p)
		{
			SpecificPolicyPopup.gameObject.SetActive(true);
			SpecificPolicyPopup.Setup(p, loc, mgm);
		}

		public void ClosePolicy()
		{
			SpecificPolicyPopup.gameObject.SetActive(false);
		}

		public void OpenMissions()
		{
			MissionsPopup.gameObject.SetActive(true);
		}

		public void CloseMissions()
		{
			MissionsPopup.gameObject.SetActive(false);
		}

		private NpcUiDisplay _currOpenNpc;
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

			foreach (var policy in PolicyOptionsParent.GetComponentsInChildren<PolicySelectionUiDisplay>(true))
				policy.RefreshUiDisplay(mgm);

			foreach (var mission in MissionOptionsParent.GetComponentsInChildren<MissionUiDisplay>(true))
				mission.RefreshUiDisplay(mgm);

			if (_currOpenNpc != null)
				_currOpenNpc.RefreshUiDisplay(mgm);
		}
	}
}