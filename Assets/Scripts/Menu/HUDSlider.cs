using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class HUDSlider : MonoBehaviour
{
    private Slider _slider;
    [SerializeField] private Image _backgroundSlider;
    public float waitTime = 0.6f;
    
    // Start is called before the first frame update
    void Start()
    {
        _slider = GetComponent<Slider>();
    }

    public void ChangeValue()
    {
        StartCoroutine(ChangeValueCoroutine());
    }
    
    IEnumerator ChangeValueCoroutine()
    {
        var value = _slider.value;
        if (_backgroundSlider.fillAmount <= value - 0.05f)
        {
            while (_backgroundSlider.fillAmount <= value - 0.05f)
            {
                _backgroundSlider.fillAmount = Mathf.Lerp(_backgroundSlider.fillAmount, value, Time.deltaTime);
            }
        }
        else if (_backgroundSlider.fillAmount >= value)
        {
            yield return new WaitForSeconds(0.6f);
            while (_backgroundSlider.fillAmount >= value)
            {
                _backgroundSlider.fillAmount = Mathf.Lerp(_backgroundSlider.fillAmount, value - 0.05f, Time.deltaTime);
            }
        }

        yield return null;
    }
    
    
}
