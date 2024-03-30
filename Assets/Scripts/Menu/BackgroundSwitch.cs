using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundSwitch : MonoBehaviour
{
    [SerializeField] private CanvasGroup[] _backgrounds;
    private int _currentBackground = 0;
    public float _speedDown = 0.1f;
    public float _speedLeft = 0.1f;
    

    void Update()
    {
        TranslationBackground();
        if (_backgrounds[_currentBackground].transform.localPosition.y <= -100)
        {
            SwitchBackground();
        }
    }
    
    public void TranslationBackground()
    {
        _backgrounds[_currentBackground].transform.Translate(Vector3.down * _speedDown);
        _backgrounds[_currentBackground].transform.Translate(Vector3.left * _speedLeft);
    }
    
    public void SwitchBackground()
    {
        
        _backgrounds[_currentBackground].transform.localPosition = new Vector3(0, 0, 0);
        _currentBackground++;
        if (_currentBackground >= _backgrounds.Length)
        {
            _currentBackground = 0;
        }
    }
    
    IEnumerator BackgroundSwitchCoroutine()
    {
        
        _backgrounds[_currentBackground].alpha = 0;
        yield return new WaitForSeconds(0.7f);
    }
}
