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

		public Location Loc;
		private MainGameManager mgm;
		public void Setup(Location loc, MainMapUiDisplay mguid, MainGameManager mgm)
		{
			this.Loc = loc;
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
				mgm.SetTrophyCaseVisibility(true);
			if (loc.ShowCar)
				mgm.SetStatusSymbolsVisibility(true);
		}

		public void Shutdown()
		{
			if (Loc.ShowTrophyCase)
				mgm.SetTrophyCaseVisibility(false);
			if(Loc.ShowCar)
				mgm.SetStatusSymbolsVisibility(false);
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
			SpecificPolicyPopup.Setup(p, Loc, mgm);
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
				RefreshUiDisplay(MainGameManager.Manager);
			}
		}

		public void RefreshUiDisplay(MainGameManager mgm)
		{
			BackgroundImage.sprite = Loc.BackgroundImage;
			Name.text = Loc.Name;

			foreach (var npc in NpcOptionsParent.GetComponentsInChildren<NpcSelectionUiDisplay>(true))
			{
				//Were they just moved/removed?
				if(!Loc.Npcs.Contains(npc._npc))
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