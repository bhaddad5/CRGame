﻿using System;
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

		[SerializeField] private Transform LocationsParent;
		[SerializeField] private Transform QuickAccessLocationsParent;

		[SerializeField] private RegionMapLocationEntryBindings _locationButtonPrefab;
		[SerializeField] private RegionMapLocationEntryBindings _quickAccessButtonPrefab;
		[SerializeField] private LocationScreenBindings _locationUiPrefab;

		[SerializeField] private AudioClip OptionalBackgroundAudio;

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

			CameraMover.Instance.ResetCameraPos();
			CameraMover.Instance.SetScreenSize(new Vector2(MapImage.mainTexture.width, MapImage.mainTexture.height));

			foreach (Location loc in region.Locations)
			{
				var d = Instantiate(_locationButtonPrefab);
				d.Setup(loc, this, mgm);
				d.transform.SetParent(LocationsParent, false);
			}

			foreach (var loc in region.QuickAccessLocations)
			{
				var d = Instantiate(_quickAccessButtonPrefab);
				d.Setup(loc, this, mgm);
				d.transform.SetParent(QuickAccessLocationsParent, false);
			}

			if (OptionalBackgroundAudio != null)
				AudioHandler.Instance.PlayBackgroundClip(OptionalBackgroundAudio);
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
				if (OptionalBackgroundAudio != null)
					AudioHandler.Instance.PlayBackgroundClip(OptionalBackgroundAudio);
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
					GameObject.Destroy(_currOpenLocation.gameObject);
					_currOpenLocation = null;
				}
			}

			if (OptionalBackgroundAudio != null)
				AudioHandler.Instance.PlayBackgroundClip(OptionalBackgroundAudio);
		}

		public void CloseRegion()
		{
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

			Power.RefreshResourceDisplay(mgm.Data.Power);
			Culture.RefreshResourceDisplay(mgm.Data.CorporateCulture);
			Spreadsheets.RefreshResourceDisplay(mgm.Data.Spreadsheets);
			Patents.RefreshResourceDisplay(mgm.Data.Patents);
			Brand.RefreshResourceDisplay(mgm.Data.Brand);
			Revanue.RefreshResourceDisplay(mgm.Data.Revenue);
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