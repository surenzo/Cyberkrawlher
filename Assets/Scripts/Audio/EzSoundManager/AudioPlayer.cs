using System.Collections;
using System.Collections.Generic;
using System.Linq;
using EzSoundManager;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Audio;

namespace Audio.EzSoundManager
{
    public class AudioPlayer : MonoBehaviour
    {
        private const int MaxAudioSourceCount = 10;

        public AudioMixerGroup masterGroup;

        [SerializeField] private int poolSize = 3;
        private readonly List<AudioSource> _audioSources = new List<AudioSource>();

        private void Awake()
        {
            for (int i = 0; i < poolSize; i++)
            {
                CreateNewAudioSource();
            }
        }


        public void PlaySound(string clipName, string category, bool is3D = true, bool loop = false)
        {
            AudioClipInfo clipInfo = AudioDatabase.Instance.GetClipInfoByNameAndCategory(clipName, category);
            if (clipInfo == null)
            {
                Debug.LogWarning("Audio clip not found: " + clipName);
                return;
            }

            AudioSource source = GetAvailableAudioSource();
            source.clip = clipInfo.clip;
            source.volume = clipInfo.volume;
            source.outputAudioMixerGroup = clipInfo.audioMixerGroup;
            source.loop = loop;
            source.spatialBlend = is3D ? 1 : 0;
            source.Play();
        }

        private AudioSource GetAvailableAudioSource()
        {
            if (_audioSources.Count < MaxAudioSourceCount || _audioSources.Exists(source => !source.isPlaying))
            {
                return _audioSources.FirstOrDefault(source => !source.isPlaying) ?? CreateNewAudioSource();
            }

            AudioSource oldestSource = _audioSources.OrderBy(s => s.time).First();
            oldestSource.Stop();
            return oldestSource;
        }

        private AudioSource CreateNewAudioSource()
        {
            AudioSource newSource = gameObject.AddComponent<AudioSource>();
            newSource.playOnAwake = false;
            newSource.outputAudioMixerGroup = masterGroup;
            _audioSources.Add(newSource);
            return newSource;
        }

        public void PlaySoundWithFade(
            string clipName, string category, float fadeDuration, float targetVolume,
            bool is3D = true, bool loop = false
        )
        {
            AudioClip clip = AudioDatabase.Instance.GetClipByNameAndCategory(clipName, category);
            if (clip.IsUnityNull())
            {
                Debug.LogWarning("Audio clip not found: " + clipName);
                return;
            }

            AudioSource source = GetAvailableAudioSource();
            source.clip = clip;
            source.volume = 0;
            source.spatialBlend = is3D ? 1 : 0;
            source.outputAudioMixerGroup = masterGroup;
            source.loop = loop;
            source.Play();
            StartCoroutine(FadeIn(source, fadeDuration, targetVolume));
        }

        private static IEnumerator FadeIn(AudioSource audioSource, float duration, float targetVolume)
        {
            float currentTime = 0;
            while (currentTime < duration)
            {
                currentTime += Time.deltaTime;
                audioSource.volume = Mathf.Lerp(0, targetVolume, currentTime / duration);
                yield return null;
            }
        }

        public void SetPitch(string clipName, float pitch)
        {
            AudioSource source = _audioSources.FirstOrDefault(s => {
                AudioClip clip;
                return (clip = s.clip) != null && clip.name == clipName;
            });
            if (source != null)
            {
                source.pitch = pitch;
            }
        }

        public void AddEchoEffect(string clipName, float delay, float decayRatio)
        {
            AudioSource source = _audioSources.FirstOrDefault(s => {
                AudioClip clip;
                return (clip = s.clip) != null && clip.name == clipName;
            });
            AudioEchoFilter echoFilter = null;
            if (source != null && !source.gameObject.TryGetComponent(out echoFilter))
            {
                echoFilter = source.gameObject.AddComponent<AudioEchoFilter>();
            }

            if (echoFilter == null) return;
            echoFilter.delay = delay;
            echoFilter.decayRatio = decayRatio;
            echoFilter.enabled = true;
        }

        public void RemoveEchoEffect(string clipName)
        {
            AudioSource source = _audioSources.FirstOrDefault(s => {
                AudioClip clip;
                return (clip = s.clip) != null && clip.name == clipName;
            });
            if (source != null && source.gameObject.TryGetComponent(out AudioEchoFilter echoFilter))
            {
                echoFilter.enabled = false;
            }
        }
    }
}