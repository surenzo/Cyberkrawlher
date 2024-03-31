using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

namespace EzSoundManager
{
    public class CrossfadeMusicPlayer : MonoBehaviour
    {

        [SerializeField] private AudioMixerGroup masterGroup;
        [SerializeField] private float crossfadeTime = 2.0f;
        [SerializeField] private float introDuration;
        [SerializeField] private string category;
        [SerializeField] private List<string> clipNames;
        [SerializeField] private bool hasIntro;
        [SerializeField] private bool startOnAwake;
        [SerializeField] private bool crossfadeAfterIntro;

        private readonly List<AudioClip> _clips = new List<AudioClip>();
        private AudioSource _activeSource;

        private int _currentClipIndex;
        private bool _firstMusicTrackAfterIntro;
        private bool _isCrossFading;
        private bool _isIntroPlaying;
        private AudioSource _nextSource;

        private void Start()
        {
            InitializeAudioSources();
            LoadClips();
            _firstMusicTrackAfterIntro = hasIntro;
            if (startOnAwake) StartMusics();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space)) CrossfadeToNextTrack();
        }

        private void InitializeAudioSources()
        {
            _activeSource = gameObject.AddComponent<AudioSource>();
            _nextSource = gameObject.AddComponent<AudioSource>();
            SetupAudioSource(_activeSource);
            SetupAudioSource(_nextSource);
        }

        private void SetupAudioSource(AudioSource source)
        {
            source.loop = true;
            source.playOnAwake = false;
            source.outputAudioMixerGroup = masterGroup;
        }

        private void LoadClips()
        {
            foreach (string clipName in clipNames)
            {
                AudioClip clip = AudioDatabase.Instance.GetClipByNameAndCategory(clipName, category);
                if (clip) _clips.Add(clip);
                else Debug.LogError($"Clip not found: {clipName} in category {category}");
            }
            if (_clips.Count < 2) Debug.LogError("Need at least two clips for crossfade functionality.");
        }
        private void StartMusics()
        {
            _activeSource.clip = _clips[_currentClipIndex];
            _activeSource.Play();
            if (!hasIntro) return;
            _isIntroPlaying = true;
            float delay = introDuration > 0 ? introDuration : _activeSource.clip.length;
            StartCoroutine(crossfadeAfterIntro ? StartCrossfadeAfterDelay(delay) : PlayNextTrackAfterDelay(delay));
        }


        private IEnumerator StartCrossfadeAfterDelay(float delay)
        {
            yield return new WaitForSeconds(delay);
            _isIntroPlaying = false;
            CrossfadeToNextTrack();
        }

        private IEnumerator PlayNextTrackAfterDelay(float delay)
        {
            yield return new WaitForSeconds(delay);
            _isIntroPlaying = false;
            PlayNextTrack();
        }

        private void PlayNextTrack()
        {
            PrepareNextClip();
            (_activeSource, _nextSource) = (_nextSource, _activeSource);
            _activeSource.volume = 1;
            _nextSource.volume = 0;
            _activeSource.Play();
        }

        private void CrossfadeToNextTrack()
        {
            if (_isIntroPlaying || _isCrossFading) return;
            PrepareNextClip();
            StartCoroutine(Crossfade(_activeSource, _nextSource));
        }

        private void PrepareNextClip()
        {
            _currentClipIndex = hasIntro && _currentClipIndex == 0 ? 1 : (_currentClipIndex + 1) % _clips.Count;
            if (hasIntro && _currentClipIndex == 0) _currentClipIndex = 1;

            _nextSource.clip = _clips[_currentClipIndex];
            _nextSource.Play();
            _nextSource.volume = 0;

            if (_firstMusicTrackAfterIntro)
            {
                _nextSource.time = 0;
                _firstMusicTrackAfterIntro = false;
            }
            else
            {
                _nextSource.time = _activeSource.time % _activeSource.clip.length;
            }
        }

        private IEnumerator Crossfade(AudioSource fadeOutSource, AudioSource fadeInSource)
        {
            _isCrossFading = true;
            for (float timeElapsed = 0; timeElapsed < crossfadeTime; timeElapsed += Time.deltaTime)
            {
                fadeOutSource.volume = 1 - timeElapsed / crossfadeTime;
                fadeInSource.volume = timeElapsed / crossfadeTime;
                yield return null;
            }
            fadeOutSource.volume = 0;
            fadeOutSource.Stop();
            (_activeSource, _nextSource) = (_nextSource, _activeSource);
            _isCrossFading = false;
        }
    }
}