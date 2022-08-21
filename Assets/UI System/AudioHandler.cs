using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.UI_System
{
	public class AudioHandler : MonoBehaviour
	{
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
			MusicSource.volume = .6f;
			MusicSource.loop = true;
		}

		public void PlayDialogClip(AudioClip clip)
		{
			if (clip != null)
			{
				DialogSource.clip = clip;
				DialogSource.Play();
			}
			
		}

		private Coroutine playNextAudioClip = null;
		private List<AudioClip> currMainTracks = new List<AudioClip>();
		public void SetMusicTracks(List<AudioClip> tracks)
		{
			if (tracks.Equals(currMainTracks))
				return;

			FadeToNewClip(tracks.FirstOrDefault(), MusicSource);
		}

		public void PlayOverridingMusicTrack(AudioClip clip)
		{
			FadeToNewClip(clip, MusicSource);
		}

		public void SetBackgroundAmbiance(AudioClip clip)
		{
			FadeToNewClip(clip, AmbientSource);
		}

		private Dictionary<AudioSource, Coroutine> fadeCoroutines = new Dictionary<AudioSource, Coroutine>();
		public void FadeToNewClip(AudioClip clip, AudioSource currPlayer)
		{
			if (fadeCoroutines.ContainsKey(currPlayer))
			{
				StopCoroutine(fadeCoroutines[currPlayer]);
				fadeCoroutines.Remove(currPlayer);
			}

			fadeCoroutines[currPlayer] = StartCoroutine(FadeToClip(clip, currPlayer));
		}

		private const float fadeDuration = .5f;
		private IEnumerator FadeToClip(AudioClip clip, AudioSource currPlayer)
		{
			float startTime = Time.time;

			while (Time.time < startTime + fadeDuration)
			{
				var fadePercent = (Time.time - startTime) / fadeDuration;
				MusicSource.volume = 1 - fadePercent;

				yield return null;
			}

			MusicSource.clip = clip;
			MusicSource.Play();

			startTime = Time.time;
			while (Time.time < startTime + fadeDuration)
			{
				var fadePercent = (Time.time - startTime) / fadeDuration;
				MusicSource.volume = fadePercent;

				yield return null;
			}

			fadeCoroutines.Remove(currPlayer);
		}
	}
}
