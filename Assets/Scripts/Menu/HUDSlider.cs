using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class HUDSlider : MonoBehaviour
{
    private Slider _slider;
    [SerializeField] private Image _backgroundSlider;
    
    private float timerRegen = 0.6f;
    private float timerRegenCounter = 0f;
    
    // Start is called before the first frame update
    void Start()
    {
        _slider = GetComponent<Slider>();
    }

    public void ChangeValue()
    {
        var value = _slider.value;

        // Gradually interpolate towards the target value
        _backgroundSlider.fillAmount = Mathf.Lerp(_backgroundSlider.fillAmount, value, Time.deltaTime);

        // Reset timer when setting the fill amount
        timerRegenCounter = 0f;

        // Check if the fill amount is close enough to the target value
        if (Mathf.Abs(_backgroundSlider.fillAmount - value) <= 0.05f)
        {
            // Perform additional actions if needed
        }
    }
}
