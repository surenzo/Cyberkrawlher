using System.Collections;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UIElements;

namespace Menu
{
    public class NeonButton : MonoBehaviour
    {
        [SerializeField] private NeonUISO _neonUiso;
        private AudioSource _audioSource;
        private bool _isPlaying;
    
        void Awake()
        {
            _audioSource = gameObject.AddComponent<AudioSource>();
            _audioSource.loop = false;
            _audioSource.playOnAwake = false;
            _audioSource.outputAudioMixerGroup = _neonUiso.mixerGroup;
        }
    
        public void StartPlaying()
        {
            _isPlaying = true;
            _audioSource.clip = _neonUiso.intro;
            _audioSource.loop = false;
            _audioSource.Play();
            StartCoroutine(WaitAndPlayLoop());
        }
    
        public void StopPlaying()
        {
            _audioSource.loop = false;
            _audioSource.clip = _neonUiso.outro;
            _audioSource.Play();
            _isPlaying = false;
        }

        IEnumerator WaitAndPlayLoop()
        {
            float startTime = Time.time;
            while (Time.time - startTime <= _neonUiso.intro.length - 0.05f)
            {
                yield return null;
            }
            
            if(!_isPlaying) yield break;
            
            _audioSource.clip = _neonUiso.loop;
            _audioSource.loop = true;
            _audioSource.Play();
        }
    }
}
