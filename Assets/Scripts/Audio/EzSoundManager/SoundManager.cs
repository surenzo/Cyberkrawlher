using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Audio.EzSoundManager;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Audio;
using Random = UnityEngine.Random;

namespace EzSoundManager
{
    public class SoundManager : MonoBehaviour
    {

        [SerializeField] private AudioMixer masterMixer;
        [SerializeField] private AudioMixerGroup musicGroup;
        [SerializeField] private int poolSize = 10;
        private Queue<AudioSource> _audioSourcePool;

        private AudioSource _musicSource;
        public static SoundManager Instance { get; private set; }

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);

                _musicSource = gameObject.AddComponent<AudioSource>();
                _musicSource.outputAudioMixerGroup = musicGroup;


                _audioSourcePool = new Queue<AudioSource>();
                for (int i = 0; i < poolSize; i++)
                {
                    AudioSource source = gameObject.AddComponent<AudioSource>();
                    source.playOnAwake = false;
                    _audioSourcePool.Enqueue(source);
                }
            }
            else
            {
                Destroy(gameObject);
            }
        }

        public void PlayMusicByCategory(string clipName, string categoryName, bool loop = true)
        {
            AudioClip clip = AudioDatabase.Instance.GetClipByNameAndCategory(clipName, categoryName);
            if (clip != null)
            {
                PlayMusic(clip, loop);
            }
            else
            {
                Debug.LogWarning($"Music clip not found: {clipName} in category {categoryName}");
            }
        }


        private void PlayMusic(AudioClip clip, bool loop)
        {
            _musicSource.clip = clip;
            _musicSource.loop = loop;
            _musicSource.Play();
        }

        public void StopMusic()
        {
            _musicSource.Stop();
        }

        public static void PlaySoundWithFadeOnGameObject(
            string clipName, string category, GameObject targetGameObject,
            float fadeDuration = 0f,
            float targetVolume = 1f, bool is3D = true, bool loop = false
        )
        {
            AudioPlayer audioPlayer = GetOrCreateAudioPlayer(targetGameObject);
            audioPlayer.PlaySoundWithFade(clipName, category, fadeDuration, targetVolume, is3D, loop);
        }

        public void FadeOutToMusicToNewClip(
            string clipName, string categoryName, float fadeDuration,
            float targetVolume
        )
        {
            AudioClip newClip = AudioDatabase.Instance.GetClipByNameAndCategory(clipName, categoryName);
            if (!newClip.IsUnityNull())
            {
                StartCoroutine(FadeOutMusicAndPlayNewClipWithFadeIn(newClip, fadeDuration, targetVolume));
            }
            else
            {
                Debug.LogWarning($"Music clip not found: {clipName} in category {categoryName}");
            }
        }


        private IEnumerator FadeOutMusicAndPlayNewClipWithFadeIn(AudioClip newClip, float fadeDuration, float targetVolume)
        {
            float startVolume = _musicSource.volume;
            float currentTime = 0;
            bool isFadingIn = false;

            while (currentTime < fadeDuration)
            {
                currentTime += Time.deltaTime;
                _musicSource.volume = isFadingIn
                    ? Mathf.Lerp(0, targetVolume, currentTime / fadeDuration)
                    : Mathf.Lerp(startVolume, 0, currentTime / fadeDuration);

                if (currentTime >= fadeDuration && !isFadingIn)
                {
                    _musicSource.Stop();
                    _musicSource.clip = newClip;
                    _musicSource.Play();
                    isFadingIn = true;
                    currentTime = 0;
                }

                yield return null;
            }
        }

        public void FadeOutMusic(float fadeDuration)
        {
            StartCoroutine(FadeOutMusicCoroutine(fadeDuration));
        }

        private IEnumerator FadeOutMusicCoroutine(float fadeDuration)
        {
            float startVolume = _musicSource.volume;
            float currentTime = 0;
            while (currentTime < fadeDuration)
            {
                currentTime += Time.deltaTime;
                _musicSource.volume = Mathf.Lerp(startVolume, 0, currentTime / fadeDuration);
                yield return null;
            }

            _musicSource.Stop();
            _musicSource.volume = startVolume;
        }


        public void PlaySoundEffect(
            string clipName, string categoryName, Vector3 position,
            bool is3D = false, bool loop = false
        )
        {
            if (_audioSourcePool.Count <= 0) return;
            AudioClipInfo clipInfo = AudioDatabase.Instance.GetClipInfoByNameAndCategory(clipName, categoryName);
            if (clipInfo != null)
            {
                AudioSource source = _audioSourcePool.Dequeue();
                source.clip = clipInfo.clip;
                source.volume = clipInfo.volume;
                source.outputAudioMixerGroup = clipInfo.audioMixerGroup;
                source.transform.position = position;
                source.spatialBlend = is3D ? 1 : 0;
                source.loop = loop;
                source.Play();
                StartCoroutine(ReturnToPoolAfterPlay(source));
            }
            else
            {
                Debug.LogWarning($"Audio clip not found: {clipName} in category {categoryName}");
            }
        }


        public static void PlaySoundOnGameObject(
            string clipName, string category, GameObject targetGameObject,
            bool is3D = true, bool loop = false
        )
        {
            AudioPlayer audioPlayer = targetGameObject.GetComponent<AudioPlayer>();
            if (audioPlayer.IsUnityNull())
            {
                audioPlayer = targetGameObject.AddComponent<AudioPlayer>();
            }

            audioPlayer.PlaySound(clipName, category, is3D, loop);
        }


        private IEnumerator ReturnToPoolAfterPlay(AudioSource source)
        {
            yield return new WaitForSeconds(source.clip.length);
            _audioSourcePool.Enqueue(source);
        }

        public void SetVolume(string groupName, float volume)
        {
            masterMixer.SetFloat(groupName, Mathf.Log10(volume) * 20);
        }

        public float GetVolume(string groupName)
        {
            masterMixer.GetFloat(groupName, out float volume);
            return Mathf.Pow(10, volume / 20);
        }

        public void PauseMusic()
        {
            if (_musicSource.isPlaying)
            {
                _musicSource.Pause();
            }
            else
            {
                _musicSource.UnPause();
            }
        }

        public bool IsMusicPlaying(string clipName)
        {
            return _musicSource.isPlaying && _musicSource.clip.name == clipName;
        }

        public bool IsSoundEffectPlaying(string clipName)
        {
            return _audioSourcePool.Any(source => source.isPlaying && source.clip.name == clipName);
        }

        public void RaiseSoundAmongRandomList(
            List<string> clipNames, string category, Vector3 position,
            bool is3D = true
        )
        {
            if (clipNames?.Count > 0)
            {
                PlaySoundEffect(clipNames[Random.Range(0, clipNames.Count)], category, position, is3D);
            }
            else
            {
                Debug.LogWarning("Clip names list is empty or null");
            }
        }

        public static void RaiseSoundAmongRandomList(
            List<string> clipNames, string category, GameObject targetGameObject,
            bool is3D = true, bool loop = false
        )
        {
            if (clipNames?.Count > 0)
            {
                PlaySoundOnGameObject(clipNames[Random.Range(0, clipNames.Count)], category, targetGameObject,
                    is3D, loop);
            }
            else
            {
                Debug.LogWarning("Clip names list is empty or null");
            }
        }

        public void RaiseRandomSoundAmongCategory(
            string category, Vector3 position,
            bool is3D = true, bool loop = false
        )
        {
            List<AudioClipInfo> clips = AudioDatabase.Instance.GetClipInfosByCategory(category);
            if (clips?.Count > 0)
            {
                AudioSource source = _audioSourcePool.Dequeue();
                AudioClipInfo clipInfo = clips[Random.Range(0, clips.Count)];
                source.clip = clipInfo.clip;
                source.transform.position = position;
                source.spatialBlend = is3D ? 1 : 0;
                source.outputAudioMixerGroup = clipInfo.audioMixerGroup;
                source.loop = loop;
                source.Play();
                StartCoroutine(ReturnToPoolAfterPlay(source));
            }
            else
            {
                Debug.LogWarning($"No clips found in category {category}");
            }
        }
        
        private static AudioPlayer GetOrCreateAudioPlayer(GameObject obj)
        {
            AudioPlayer audioPlayer = obj.GetComponent<AudioPlayer>();
            if (audioPlayer.IsUnityNull())
            {
                audioPlayer = obj.AddComponent<AudioPlayer>();
            }

            return audioPlayer;
        }

        public static void RaiseRandomSoundAmongCategory(
            string category, GameObject targetGameObject,
            bool is3D = true, bool loop = false
        )
        {
            List<AudioClipInfo> clips = AudioDatabase.Instance.GetClipInfosByCategory(category);
            if (clips?.Count > 0)
            {
                AudioPlayer audioPlayer = GetOrCreateAudioPlayer(targetGameObject);
                audioPlayer.PlaySound(clips[Random.Range(0, clips.Count)].clipName, category, is3D, loop);
            }
            else
            {
                Debug.LogWarning($"No clips found in category {category}");
            }
        }

        public void RaiseSoundWithRandomPitch(
            string clipName, string category, Vector3 position, float minPitch,
            float maxPitch, bool is3D = false, bool loop = false
        )
        {
            if (_audioSourcePool.Count <= 0) return;
            AudioClipInfo clip = AudioDatabase.Instance.GetClipInfoByNameAndCategory(clipName, category);
            if (clip != null)
            {
                AudioSource source = _audioSourcePool.Dequeue();
                source.clip = clip.clip;

                float randomPitch = Random.Range(minPitch, maxPitch);
                source.pitch = randomPitch;

                source.transform.position = position;
                source.spatialBlend = is3D ? 1 : 0;
                source.outputAudioMixerGroup = clip.audioMixerGroup;
                source.loop = loop;
                source.Play();
                StartCoroutine(ReturnToPoolAfterPlay(source));
            }
            else
            {
                Debug.LogWarning("Audio clip not found: " + clipName);
            }
        }

        public void PlayMusicWithIntro(string introName, string musicName, string category)
        {
            AudioClip introClip = AudioDatabase.Instance.GetClipByNameAndCategory(introName, category);
            if (introClip == null)
            {
                Debug.LogWarning($"Music clip not found: {introName} in category {category}");
                return;
            }

            AudioClip musicClip = AudioDatabase.Instance.GetClipByNameAndCategory(musicName, category);

            if (musicClip == null)
            {
                Debug.LogWarning($"Music clip not found: {musicName} in category {category}");
                return;
            }

            StartCoroutine(PlayMusicWithIntroCoroutine(introClip, musicClip));
        }

        private IEnumerator PlayMusicWithIntroCoroutine(AudioClip introClip, AudioClip musicClip)
        {
            if (!introClip.IsUnityNull())
            {
                _musicSource.clip = introClip;
                _musicSource.loop = false;
                _musicSource.Play();

                yield return new WaitForSeconds(introClip.length);
            }

            _musicSource.clip = musicClip;
            _musicSource.loop = true;
            _musicSource.Play();
        }

        public float GetClipDuration(string clipName, string category)
        {
            AudioClip clip = AudioDatabase.Instance.GetClipByNameAndCategory(clipName, category);
            if (clip != null)
            {
                return clip.length;
            }
            Debug.LogWarning($"Audio clip not found: {clipName} in category {category}");
            return 0;
        }

        private void OnDestroy()
        {
            Instance = null;
        }
    }
}