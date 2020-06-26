using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.UnityProject.UnityTypeWrappers
{
    internal class AudioSourceWrapper : IAudioSource
    {
        private AudioSource _audioSource;

        public AudioSourceWrapper(AudioSource audioSource)
        {
            _audioSource = audioSource;
        }

        public void Play()
        {
            _audioSource.Play();
        }

        public void PlayOneShot()
        {
            _audioSource.PlayOneShot(_audioSource.clip, 1f);
        }

        public void UnPause()
        {
            _audioSource.UnPause();
        }

        public void Pause()
        {
            _audioSource.Pause();
        }

        public bool IsPlaying
        {
            get
            {
                return _audioSource.isPlaying;
            }
        }

        public string Name
        {
            get
            {
                return _audioSource.name;
            }
        }

        /// <summary>
        /// returns Length in seconds
        /// </summary>
        public float Length
        {
            get
            {
                return _audioSource.clip.length;
            }

        }

        public float Time
        {
            set
            {
                _audioSource.time = value;
            }
        }
    }
}
