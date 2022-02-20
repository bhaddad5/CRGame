using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.GameModel.UiDisplayers
{
	public class LocationNpcEntryBindings : MonoBehaviour
	{
		[SerializeField] private Button Button;
		[SerializeField] private TMP_Text Text;
		[SerializeField] private Image NpcPic;
		[SerializeField] private GameObject NewIndicator;

		[HideInInspector]
		public Npc npc;
		private MainGameManager mgm;
		private LocationScreenBindings deptUi;

		//Dumb, but this doesn't work when called from Setup()
		void Start()
		{
			npc.LocationLayout.ApplyToRectTransform(GetComponent<RectTransform>());
		}

		public void Setup(Npc npc, LocationScreenBindings deptUi, MainGameManager mgm)
		{
			this.npc = npc;
			this.mgm = mgm;
			this.deptUi = deptUi;
		}

		public void OpenNpc()
		{
			deptUi.ShowNpc(npc, mgm);
		}

		public void RefreshUiDisplay(MainGameManager mgm)
		{
			Text.text = $"{npc.FirstName} {npc.LastName}";
			NpcPic.sprite = npc.GetCurrentPicture().ToSprite();
			gameObject.SetActive(npc.IsVisible(mgm));
			NewIndicator.SetActive(npc.HasNewInteractions(mgm));
		}
	}
}