using System.Collections;
using System.Collections.Generic;
using Assets.GameModel;
using Assets.UI_System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.GameModel.UiDisplayers
{
	public class WorldMapScreenBindings : MonoBehaviour
	{
		[SerializeField] private Color MorningTint;
		[SerializeField] private Color AfternoonTint;
		[SerializeField] private Image MapImage;

		[SerializeField] private RectTransform WorldMapCanvas;
		[SerializeField] private Transform RegionsParent;

		[SerializeField] private WorldMapRegionEntryBindings _regionEntryPrefab;
		[SerializeField] private RegionMapScreenBindings _regionUiPrefab;
		
		private MainGameManager mgm;
		public void Setup(MainGameManager mgm, List<Region> regions)
		{
			Vector2 mapSize = new Vector2(MapImage.mainTexture.width, MapImage.mainTexture.height);

			CameraMover.Instance.ResetCameraPos();
			CameraMover.Instance.SetScreenSize(mapSize);

			WorldMapCanvas.sizeDelta = mapSize;

			this.mgm = mgm;
			foreach (Region reg in regions)
			{
				var d = Instantiate(_regionEntryPrefab);
				d.Setup(reg, this, mapSize, mgm);
				d.transform.SetParent(RegionsParent, false);
			}

			AudioHandler.Instance.SetMusicTracks(mgm.IsWeekend() ? mgm.WorldWeekendAudio : mgm.WorldWeekdayAudio);
		}

		private RegionMapScreenBindings _currOpenRegion = null;
		public void ShowRegion(Region region, MainGameManager mgm)
		{
			gameObject.SetActive(false);
			_currOpenRegion = Instantiate(_regionUiPrefab);
			_currOpenRegion.Setup(mgm, region, () =>
			{
				CameraMover.Instance.ResetCameraPos();
				CameraMover.Instance.SetScreenSize(new Vector2(MapImage.mainTexture.width, MapImage.mainTexture.height));
				gameObject.SetActive(true);
				RefreshUiDisplay(mgm);
			});
			_currOpenRegion.RefreshUiDisplay(mgm);
		}

		public void HandleTurnChange()
		{
			if(_currOpenRegion != null)
				_currOpenRegion.CloseCurrentDepartment(true);
		}

		public void RefreshUiDisplay(MainGameManager mgm)
		{
			foreach (var button in RegionsParent.GetComponentsInChildren<WorldMapRegionEntryBindings>(true))
				button.RefreshUiDisplay(mgm);

			if (_currOpenRegion != null)
				_currOpenRegion.RefreshUiDisplay(mgm);

			if(gameObject.activeInHierarchy)
				AudioHandler.Instance.SetMusicTracks(mgm.IsWeekend() ? mgm.WorldWeekendAudio : mgm.WorldWeekdayAudio);

			ShowTimeOfDay(mgm.Data.TurnNumber % 2 == 1);
		}

		public void ShowTimeOfDay(bool afternoon)
		{
			if (afternoon)
				MapImage.color = AfternoonTint;
			else
				MapImage.color = MorningTint;
		}
	}
}