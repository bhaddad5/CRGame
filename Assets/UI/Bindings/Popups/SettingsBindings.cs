using System.Collections;
using System.Collections.Generic;
using Assets.UI_System;
using UnityEngine;
using UnityEngine.UI;

public class SettingsBindings : MonoBehaviour
{
	[SerializeField] private Slider masterVolumeSlider;
	[SerializeField] private Slider dialogVolumeSlider;
	[SerializeField] private Slider ambientVolumeSlider;
	[SerializeField] private Slider effectsVolumeSlider;
	[SerializeField] private Slider musicVolumeSlider;
	private GameObject parentToNuke;

	public void Setup(GameObject parentToNuke = null)
	{
		this.parentToNuke = parentToNuke;

		masterVolumeSlider.value = AudioHandler.Instance.MasterVolumeMult;
		ambientVolumeSlider.value = AudioHandler.Instance.AmbientVolumeMult;
		effectsVolumeSlider.value = AudioHandler.Instance.EffectsVolumeMult;
		dialogVolumeSlider.value = AudioHandler.Instance.DialogVolumeMult;
		musicVolumeSlider.value = AudioHandler.Instance.MusicVolumeMult;
	}

	public void MasterVolumeChanged(float volume)
	{
		AudioHandler.Instance.SetMasterVolume(volume);
	}

	public void EffectsVolumeChanged(float volume)
	{
		AudioHandler.Instance.SetEffectsVolume(volume);
	}

	public void AmbientVolumeChanged(float volume)
	{
		AudioHandler.Instance.SetAmbientVolume(volume);
	}

	public void DialogVolumeChanged(float volume)
	{
		AudioHandler.Instance.SetDialogVolume(volume);
	}

	public void MusicVolumeChanged(float volume)
	{
		AudioHandler.Instance.SetMusicVolume(volume);
	}

	public void Cancel()
	{
		if (parentToNuke != null)
			GameObject.Destroy(parentToNuke);
		GameObject.Destroy(gameObject);
	}
}
