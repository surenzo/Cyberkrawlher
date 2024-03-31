using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pause : MonoBehaviour
{
    [SerializeField] private CanvasGroup _pausePanel;
    
    void Start()
    {
        _pausePanel.alpha = 0;
        _pausePanel.gameObject.SetActive(false);
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (Time.timeScale == 0)
            {
                Time.timeScale = 1;
                _pausePanel.LeanAlpha(0, 0.4f).setOnComplete( () => {
                    _pausePanel.gameObject.SetActive(false);
                });
            }
            else
            {
                
                _pausePanel.gameObject.SetActive(true);
                _pausePanel.LeanAlpha(1, 0.4f).setOnComplete(() =>
                {
                    Time.timeScale = 0;
                });
            }
        }
    }
}
