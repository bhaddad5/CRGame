using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.GameModel.UiDisplayers
{
	public class NpcSelectionUiDisplay : MonoBehaviour, IUiDisplay
	{
		[SerializeField] private Button Button;
		[SerializeField] private TMP_Text Text;
		[SerializeField] private Image NpcPic;

		private Npc _npc;

		//Dumb, but this doesn't work when called from Setup()
		void Start()
		{
			GetComponent<RectTransform>().anchorMin = new Vector2(_npc.Layout.X, _npc.Layout.Y);
			GetComponent<RectTransform>().anchorMax = new Vector2(_npc.Layout.X, _npc.Layout.Y);
			GetComponent<RectTransform>().sizeDelta = new Vector2(_npc.Layout.Width, _npc.Layout.Width * 2f);
			GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
		}

		public void Setup(Npc npc, LocationUiDisplay deptUi, MainGameManager mgm)
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
			NpcPic.sprite = NpcPicManager.GetNpcPicFromId(_npc.Id, _npc.DetermineCurrPictureId());
			gameObject.SetActive(_npc.IsVisible(mgm));
		}
	}
}