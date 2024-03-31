using System.Collections.Generic;
using EzSoundManager;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Menu
{
    public class OptionVolume : MonoBehaviour
    {
        [SerializeField] private List<Slider> sliders;
        [SerializeField] private List<string> groupNames;

        private SoundManager _soundManager;

        private void Start()
        {
            for (int i = 0; i < sliders.Count; i++)
            {
                int index = i;
                sliders[i].onValueChanged.AddListener(value => HandleSliderValueChanged(value, groupNames[index]));
            }

            _soundManager = SoundManager.Instance;

            for (int i = 0; i < sliders.Count; i++)
            {
                sliders[i].value = _soundManager.GetVolume(groupNames[i]);
            }
        }

        private void HandleSliderValueChanged(float value, string groupName)
        {
            _soundManager.SetVolume(groupName, value);
        }
    }
}