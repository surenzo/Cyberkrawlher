using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

[CreateAssetMenu(fileName = "Neon", menuName = "Audio")]

public class NeonUISO : ScriptableObject
{
        public AudioClip intro;
        public AudioClip loop;
        public AudioClip outro;
        
        public AudioMixerGroup mixerGroup;
}
