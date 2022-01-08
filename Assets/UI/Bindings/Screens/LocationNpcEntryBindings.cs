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

		public Npc _npc;

		//Dumb, but this doesn't work when called from Setup()
		void Start()
		{
			_npc.LocationLayout.ApplyToRectTransform(GetComponent<RectTransform>());
		}

		public void Setup(Npc npc, LocationScreenBindings deptUi, MainGameManager mgm)
		{
			this._npc = npc;
			Button.onClick.AddListener(() =>
			{
				deptUi.ShowNpc(npc, mgm);
			});
		}

		public void RefreshUiDisplay(MainGameManager mgm)
		{
			Text.text = $"{_npc.FirstName} {_npc.LastName}";
			NpcPic.sprite = _npc.GetCurrentPicture().ToSprite();
			gameObject.SetActive(_npc.IsVisible(mgm));
		}
	}
}