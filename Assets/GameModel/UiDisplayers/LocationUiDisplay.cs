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
		[SerializeField] private Transform MissionsButton;
		[SerializeField] private Transform MissionsPopup;
		[SerializeField] private Transform MissionOptionsParent;

		[SerializeField] private NpcSelectionUiDisplay _npcButtonPrefab;
		[SerializeField] private NpcUiDisplay _npcUiPrefab;

		[SerializeField] private PolicySelectionUiDisplay policyPrefab;
		[SerializeField] private MissionUiDisplay misisonPrefab;

		public Location Dept;
		public void Setup(Location dept, MainMapUiDisplay mguid, MainGameManager mgm)
		{
			this.Dept = dept;
			BackButton.onClick.AddListener(() => mguid.CloseCurrentDepartment(false));

			foreach (Npc npc in dept.Npcs)
			{
				var f = Instantiate(_npcButtonPrefab);
				f.Setup(npc, this, mgm);
				f.transform.SetParent(NpcOptionsParent);
			}

			if (dept.Policies.Count == 0)
				PoliciesButton.gameObject.SetActive(false);
			foreach (Policy policy in dept.Policies)
			{
				var p = Instantiate(policyPrefab);
				p.Setup(policy, dept, mgm);
				p.transform.SetParent(PolicyOptionsParent);
			}

			if (dept.Missions.Count == 0)
				MissionsButton.gameObject.SetActive(false);
			foreach (Mission mission in dept.Missions)
			{
				var m = Instantiate(misisonPrefab);
				m.Setup(mission, dept, mgm);
				m.transform.SetParent(MissionOptionsParent);
			}

			ClosePolicies();
			CloseMissions();
		}

		public void OpenPolicies()
		{
			PoliciesPopup.gameObject.SetActive(true);
		}

		public void ClosePolicies()
		{
			PoliciesPopup.gameObject.SetActive(false);
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
			}
		}

		public void RefreshUiDisplay(MainGameManager mgm)
		{
			BackgroundImage.sprite = Dept.BackgroundImage;
			Name.text = Dept.Name;

			foreach (var npc in NpcOptionsParent.GetComponentsInChildren<NpcSelectionUiDisplay>(true))
				npc.RefreshUiDisplay(mgm);

			foreach (var policy in PolicyOptionsParent.GetComponentsInChildren<PolicySelectionUiDisplay>(true))
				policy.RefreshUiDisplay(mgm);

			foreach (var mission in MissionOptionsParent.GetComponentsInChildren<MissionUiDisplay>(true))
				mission.RefreshUiDisplay(mgm);

			if (_currOpenNpc != null)
				_currOpenNpc.RefreshUiDisplay(mgm);
		}
	}
}