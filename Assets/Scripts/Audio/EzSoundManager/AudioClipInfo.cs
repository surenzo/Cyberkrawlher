using System;
using UnityEngine;
using UnityEngine.Audio;

namespace EzSoundManager
{
    [Serializable]
    public class AudioClipInfo
    {
        public string clipName;
        public AudioClip clip;
        public float volume = 1f;
        public AudioMixerGroup audioMixerGroup;
    }
}