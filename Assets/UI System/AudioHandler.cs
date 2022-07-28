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

		void Awake()
		{
			instance = this;
		}

		[SerializeField] private AudioSource ForegroundSource;
		[SerializeField] private AudioSource BackgroundSource;

		public void PlayForegroundClip(AudioClip clip)
		{
			if (clip != null)
			{
				ForegroundSource.clip = clip;
				ForegroundSource.Play();
			}
			
		}

		private Coroutine fadeCoroutine = null;
		public void PlayBackgroundClip(AudioClip clip)
		{
			if (clip != null && BackgroundSource.clip != clip)
			{
				if(fadeCoroutine != null)
					StopCoroutine(fadeCoroutine);
				fadeCoroutine = StartCoroutine(FadeOutAudio(clip));
			}
		}

		private const float fadeDuration = .5f;
		private IEnumerator FadeOutAudio(AudioClip clip)
		{
			float startTime = Time.time;
			
			while (Time.time < startTime + fadeDuration)
			{
				var fadePercent = (Time.time - startTime) / fadeDuration;
				BackgroundSource.volume = 1-fadePercent;

				yield return null;
			}

			BackgroundSource.clip = clip;
			BackgroundSource.Play();

			startTime = Time.time;
			while (Time.time < startTime + fadeDuration)
			{
				var fadePercent = (Time.time - startTime) / fadeDuration;
				BackgroundSource.volume = fadePercent;

				yield return null;
			}

			fadeCoroutine = null;
		}
	}
}
