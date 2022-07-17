using System;
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

		//TODO: Crossfade!!!
		public void PlayBackgroundClip(AudioClip clip)
		{
			if (clip != null)
			{
				BackgroundSource.clip = clip;
				BackgroundSource.Play();
			}
		}
	}
}
