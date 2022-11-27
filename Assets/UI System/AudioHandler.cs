using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Assets.UI_System
{
	public class AudioHandler : MonoBehaviour
	{
		#region Setup
		public static AudioHandler Instance => instance;

		private static AudioHandler instance;

		[SerializeField] private AudioSource SourcePrefab;

		private AudioSource DialogSource;
		private AudioSource EffectSource;
		private AudioSource AmbientSource;
		private AudioSource MusicSource;

		public bool GreivousMode = false;

		public void SetGreivousMode(bool val)
		{
			GreivousMode = val;
			PlayerPrefs.SetInt("GreivousMode", val ? 1 : 0);
			PlayerPrefs.Save();
		}
		
		private float masterVolumeMult;
		public float MasterVolumeMult => masterVolumeMult;
		public void SetMasterVolume(float volume)
		{
			masterVolumeMult = volume;
			PlayerPrefs.SetFloat("MasterVolume", volume);
			PlayerPrefs.Save();

			EffectSource.volume = baseEffectsVolume * effectsVolumeMult * masterVolumeMult;
			AmbientSource.volume = baseAmbientVolume * ambientVolumeMult * masterVolumeMult;
			DialogSource.volume = baseDialogVolume * dialogVolumeMult * masterVolumeMult;
			MusicSource.volume = baseMusicVolume * musicFadeMult * musicVolumeMult * masterVolumeMult;
		}

		private float effectsVolumeMult;
		private const float baseEffectsVolume = 1f;
		public float EffectsVolumeMult => effectsVolumeMult;
		public void SetEffectsVolume(float volume)
		{
			effectsVolumeMult = volume;
			PlayerPrefs.SetFloat("EffectVolume", volume);
			PlayerPrefs.Save();
			EffectSource.volume = baseEffectsVolume * effectsVolumeMult * masterVolumeMult;
		}

		private float ambientVolumeMult;
		private const float baseAmbientVolume = .3f;
		public float AmbientVolumeMult => ambientVolumeMult;
		public void SetAmbientVolume(float volume)
		{
			ambientVolumeMult = volume;
			PlayerPrefs.SetFloat("AmbientVolume", volume);
			PlayerPrefs.Save();
			AmbientSource.volume = baseAmbientVolume * ambientVolumeMult * masterVolumeMult;
		}

		private float dialogVolumeMult;
		private const float baseDialogVolume = 1f;
		public float DialogVolumeMult => dialogVolumeMult;
		public void SetDialogVolume(float volume)
		{
			dialogVolumeMult = volume;
			PlayerPrefs.SetFloat("DialogVolume", volume);
			PlayerPrefs.Save();
			DialogSource.volume = baseDialogVolume * dialogVolumeMult * masterVolumeMult;
		}

		private float musicVolumeMult;
		private const float baseMusicVolume = .25f;
		private float musicFadeMult = 1f;
		public float MusicVolumeMult => musicVolumeMult;
		public void SetMusicVolume(float volume)
		{
			musicVolumeMult = volume;
			PlayerPrefs.SetFloat("MusicVolume", volume);
			PlayerPrefs.Save();
			MusicSource.volume = baseMusicVolume * musicFadeMult * musicVolumeMult * masterVolumeMult;
		}

		void Awake()
		{
			instance = this;

			GreivousMode = PlayerPrefs.GetInt("GreivousMode", 0) > 0;

			masterVolumeMult = PlayerPrefs.GetFloat("MasterVolume", 1f);
			musicVolumeMult = PlayerPrefs.GetFloat("MusicVolume", 1f);
			dialogVolumeMult = PlayerPrefs.GetFloat("DialogVolume", 1f);
			effectsVolumeMult = PlayerPrefs.GetFloat("EffectVolume", 1f);
			ambientVolumeMult = PlayerPrefs.GetFloat("AmbientVolume", 1f);

			EffectSource = Instantiate(SourcePrefab);
			EffectSource.volume = baseEffectsVolume * effectsVolumeMult * masterVolumeMult;

			DialogSource = Instantiate(SourcePrefab);
			DialogSource.volume = baseDialogVolume * dialogVolumeMult * masterVolumeMult;

			AmbientSource = Instantiate(SourcePrefab);
			AmbientSource.volume = baseAmbientVolume * ambientVolumeMult * masterVolumeMult;
			AmbientSource.loop = true;

			MusicSource = Instantiate(SourcePrefab);
			MusicSource.volume = baseMusicVolume * musicFadeMult * MusicVolumeMult * masterVolumeMult;
		}

		#endregion

		#region Effect

		public void PlayEffectClip(AudioClip clip)
		{
			if (clip != null)
			{
				EffectSource.clip = clip;
				EffectSource.Play();
			}

		}

		#endregion

		#region Dialog

		public void PlayDialogClip(AudioClip clip)
		{
			if (clip != null)
			{
				DialogSource.clip = clip;
				DialogSource.Play();
			}

		}

		#endregion

		#region Ambience

		public void SetBackgroundAmbiance(AudioClip clip)
		{
			
		}

		#endregion

		#region Music

		private const float fadeTime = 1f;
		

		private List<AudioClip> currAudioClips = new List<AudioClip>();
		private bool fadeToNew = false;
		private float fadeToNewStartTime = 0f;
		public void SetMusicTracks(List<AudioClip> tracks)
		{
			if (tracks.Count == 0)
				return;

			//If it's all just the same clips we don't wanna swap the list and trigger a track restart
			if (tracks.Count == currAudioClips.Count)
			{
				bool sameList = true;
				foreach (var clip in tracks)
				{
					if (!currAudioClips.Contains(clip))
						sameList = false;
				}
				if (sameList)
					return;
			}
			
			currAudioClips = tracks;
			if (MusicSource.isPlaying)
			{
				fadeToNew = true;
				fadeToNewStartTime = Time.time;
			}
		}


		private bool isOverriding = false;
		public void PlayOverridingMusicTrack(AudioClip clip)
		{
			MusicSource.clip = clip;
			MusicSource.Play();
			musicFadeMult = 1f;
			MusicSource.volume = baseMusicVolume * musicFadeMult * MusicVolumeMult * masterVolumeMult;
			isOverriding = true;
		}

		void Update()
		{
			if (!MusicSource.isPlaying)
			{
				isOverriding = false;
				var newClip = currAudioClips[Random.Range(0, currAudioClips.Count)];
				//Try not to repeat if we can avoid it
				if (currAudioClips.Count > 1 && newClip == MusicSource.clip)
					newClip = currAudioClips.First(c => c != MusicSource.clip);
				MusicSource.clip = newClip;
				//Debug.Log($"Setting clip to {MusicSource.clip}, clips count = {currAudioClips.Count}");
				MusicSource.Play();
			}

			if (!isOverriding)
			{
				var timeSinceStart = MusicSource.time;
				var timeTillEnd = MusicSource.clip.length - MusicSource.time;
				if (timeTillEnd < fadeTime)
				{
					musicFadeMult = Mathf.Max(0f, 1f - (timeTillEnd / fadeTime)) * baseMusicVolume;
					MusicSource.volume = baseMusicVolume * musicFadeMult * MusicVolumeMult * masterVolumeMult;
				}
				else if (timeSinceStart < fadeTime)
				{
					musicFadeMult = Mathf.Max(0f, (timeSinceStart / fadeTime)) * baseMusicVolume;
					MusicSource.volume = baseMusicVolume * musicFadeMult * MusicVolumeMult * masterVolumeMult;
				}
				else if (fadeToNew)
				{
					var timeSinceFadeToNew = Time.time - fadeToNewStartTime;
					musicFadeMult = Mathf.Max(0f, 1f - (timeSinceFadeToNew / fadeTime)) * baseMusicVolume;
					MusicSource.volume = baseMusicVolume * musicFadeMult * MusicVolumeMult * masterVolumeMult;
					if (Time.time - fadeToNewStartTime > fadeTime)
					{
						fadeToNew = false;
						MusicSource.Pause();
						return;
					}
				}
				else
				{
					musicFadeMult = 1f;
					MusicSource.volume = baseMusicVolume * musicFadeMult * MusicVolumeMult * masterVolumeMult;
				}
			}
		}

		#endregion
	}
}
