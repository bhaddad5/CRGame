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
		private AudioSource AmbientSource;
		private AudioSource MusicSource;

		void Awake()
		{
			instance = this;
			DialogSource = Instantiate(SourcePrefab);
			DialogSource.volume = 1f;

			AmbientSource = Instantiate(SourcePrefab);
			AmbientSource.volume = .3f;
			AmbientSource.loop = true;

			MusicSource = Instantiate(SourcePrefab);
			MusicSource.volume = musicVolume;
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
		private const float musicVolume = .4f;

		private List<AudioClip> currAudioClips = new List<AudioClip>();
		private bool fadeToNew = false;
		private float fadeToNewStartTime = 0f;
		public void SetMusicTracks(List<AudioClip> tracks)
		{
			if (tracks.Equals(currAudioClips))
				return;


			Debug.Log("Setting new Tracks " + tracks.Count);


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
			Debug.Log("Play Overriding!");

			MusicSource.clip = clip;
			MusicSource.Play();
			MusicSource.volume = musicVolume;
			isOverriding = true;
		}

		void Update()
		{
			if (!MusicSource.isPlaying)
			{
				isOverriding = false;
				MusicSource.clip = currAudioClips[Random.Range(0, currAudioClips.Count)];
				MusicSource.Play();
			}

			if (!isOverriding)
			{
				var timeSinceStart = MusicSource.time;
				var timeTillEnd = MusicSource.clip.length - MusicSource.time;
				if (timeTillEnd < fadeTime)
				{
					float newVolume = Mathf.Max(0f, 1f - (timeTillEnd / fadeTime)) * musicVolume;
					MusicSource.volume = newVolume;
				}
				else if (timeSinceStart < fadeTime)
				{
					float newVolume = Mathf.Max(0f, (timeSinceStart / fadeTime)) * musicVolume;
					MusicSource.volume = newVolume;
				}
				else if (fadeToNew)
				{
					var timeSinceFadeToNew = Time.time - fadeToNewStartTime;
					float newVolume = Mathf.Max(0f, 1f - (timeSinceFadeToNew / fadeTime)) * musicVolume;
					MusicSource.volume = newVolume;
					if (Time.time - fadeToNewStartTime > fadeTime)
					{
						fadeToNew = false;
						MusicSource.Pause();
						return;
					}
				}
			}
		}

		#endregion
	}
}
