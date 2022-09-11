using System;
using System.Collections;
using System.Collections.Generic;
using Assets.GameModel;
using Assets.UI_System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.GameModel.UiDisplayers
{
	public class RegionMapScreenBindings : MonoBehaviour
	{
		[SerializeField] private Color MorningTint;
		[SerializeField] private Color AfternoonTint;
		[SerializeField] private Image MapImage;

		[SerializeField] private RectTransform RegionMapCanvas;
		[SerializeField] private Transform LocationsParent;
		[SerializeField] private Transform QuickAccessLocationsParent;

		[SerializeField] private RegionMapLocationEntryBindings _locationButtonPrefab;
		[SerializeField] private RegionMapLocationEntryBindings _locationShopButtonPrefab;
		[SerializeField] private RegionMapLocationEntryBindings _locationHouseButtonPrefab;
		[SerializeField] private RegionMapLocationEntryBindings _locationOfficeButtonPrefab;
		[SerializeField] private RegionMapLocationEntryBindings _quickAccessButtonPrefab;
		[SerializeField] private LocationScreenBindings _locationUiPrefab;

		[SerializeField] private TMP_Text RegionName;

		[SerializeField] private List<GameObject> RegionUiToHide;


		[SerializeField] private ResourceManagerUiDisplay Power;
		[SerializeField] private ResourceManagerUiDisplay Spreadsheets;
		[SerializeField] private ResourceManagerUiDisplay Culture;
		[SerializeField] private ResourceManagerUiDisplay Brand;
		[SerializeField] private ResourceManagerUiDisplay Revanue;
		[SerializeField] private ResourceManagerUiDisplay Patents;

		private MainGameManager mgm;
		private Action onClose;
		private Region region;
		public void Setup(MainGameManager mgm, Region region, Action onClose)
		{
			this.onClose = onClose;
			this.mgm = mgm;
			this.region = region;

			var mapSize = new Vector2(MapImage.mainTexture.width, MapImage.mainTexture.height);
			
			CameraMover.Instance.ResetCameraPos();
			CameraMover.Instance.SetScreenSize(mapSize);

			RegionMapCanvas.sizeDelta = mapSize;

			foreach (Location loc in region.Locations)
			{
				var prefab = _locationButtonPrefab;
				if (loc.locationType == Location.LocationType.Store)
					prefab = _locationShopButtonPrefab;
				else if (loc.locationType == Location.LocationType.Office)
					prefab = _locationOfficeButtonPrefab;
				else if (loc.locationType == Location.LocationType.Home)
					prefab = _locationHouseButtonPrefab;

				var d = Instantiate(prefab);
				d.Setup(loc, this, mapSize, mgm);
				d.transform.SetParent(LocationsParent, false);
			}

			foreach (var loc in region.QuickAccessLocations)
			{
				var d = Instantiate(_quickAccessButtonPrefab);
				d.Setup(loc, this, mapSize, mgm, true);
				d.transform.SetParent(QuickAccessLocationsParent, false);
			}

			AudioHandler.Instance.SetBackgroundAmbiance(region.BackgroundAmbience);

			AudioHandler.Instance.SetMusicTracks(region.GetCurrMusicTracks(mgm));
		}

		private LocationScreenBindings _currOpenLocation = null;
		public void ShowLocation(Location dept, MainGameManager mgm)
		{
			CloseCurrentDepartment(false);
			foreach (var ui in RegionUiToHide)
				ui.SetActive(false);
			_currOpenLocation = Instantiate(_locationUiPrefab);
			_currOpenLocation.Setup(dept, mgm, () =>
			{
				foreach (var ui in RegionUiToHide)
					ui.SetActive(true);
				AudioHandler.Instance.SetBackgroundAmbiance(region.BackgroundAmbience);
				RefreshUiDisplay(mgm);
			});
			_currOpenLocation.RefreshUiDisplay(mgm);
		}

		public void CloseCurrentDepartment(bool onlyCloseIfInaccessable)
		{
			if (_currOpenLocation != null)
			{
				_currOpenLocation.CloseCurrentNpc();
				if (!onlyCloseIfInaccessable || !_currOpenLocation.IsAccessible(mgm))
				{
					_currOpenLocation.CloseCurrentLocation();
					_currOpenLocation = null;
				}
			}
		}

		public void CloseRegion()
		{
			CloseCurrentDepartment(false);
			GameObject.Destroy(gameObject);
			onClose?.Invoke();
		}

		public void RefreshUiDisplay(MainGameManager mgm)
		{
			RegionName.text = region.Name;

			foreach (var button in LocationsParent.GetComponentsInChildren<RegionMapLocationEntryBindings>(true))
				button.RefreshUiDisplay(mgm);
			foreach (var button in QuickAccessLocationsParent.GetComponentsInChildren<RegionMapLocationEntryBindings>(true))
				button.RefreshUiDisplay(mgm);

			if (_currOpenLocation != null)
				_currOpenLocation.RefreshUiDisplay(mgm);

			ShowTimeOfDay(mgm.Data.TurnNumber % 2 == 1);

			Power.RefreshResourceDisplay(mgm.Data.Power, mgm);
			Culture.RefreshResourceDisplay(mgm.Data.CorporateCulture, mgm);
			Spreadsheets.RefreshResourceDisplay(mgm.Data.Spreadsheets, mgm);
			Patents.RefreshResourceDisplay(mgm.Data.Patents, mgm);
			Brand.RefreshResourceDisplay(mgm.Data.Brand, mgm);
			Revanue.RefreshResourceDisplay(mgm.Data.Revenue, mgm);

			AudioHandler.Instance.SetMusicTracks(region.GetCurrMusicTracks(mgm));
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