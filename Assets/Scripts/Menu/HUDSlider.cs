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
        while (_backgroundSlider.fillAmount <= value - 0.05f)
        {
            _backgroundSlider.fillAmount = Mathf.Lerp(_backgroundSlider.fillAmount, value, Time.deltaTime);
            timerRegenCounter = 0f;
        }
        while (_backgroundSlider.fillAmount >= value - 0.05f){
            timerRegenCounter += Time.deltaTime;
            if (timerRegenCounter >= timerRegen)
            {
                _backgroundSlider.fillAmount = Mathf.Lerp(_backgroundSlider.fillAmount, value - 0.05f, Time.deltaTime);
            }
        }
    }
}
