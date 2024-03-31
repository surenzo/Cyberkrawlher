using UnityEngine;
using UnityEngine.UI;

namespace EzSoundManager.Example
{
    public class VolumeControl : MonoBehaviour
    {
        [SerializeField] private string groupName; // Example: "MusicVolume"

        private Slider _slider;

        private void Start()
        {
            _slider = GetComponent<Slider>();
            _slider.onValueChanged.AddListener(HandleSliderValueChanged);
        }

        private void HandleSliderValueChanged(float value)
        {
            SoundManager.Instance.SetVolume(groupName, value);
        }
    }
}