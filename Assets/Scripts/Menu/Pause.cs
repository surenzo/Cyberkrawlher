using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pause : MonoBehaviour
{
    [SerializeField] private CanvasGroup _pausePanel;
    [SerializeField] private CanvasGroup _HUDPanel;
    
    void Start()
    {
        _pausePanel.alpha = 0;
        _HUDPanel.alpha = 1;
        _pausePanel.gameObject.SetActive(false);
        _HUDPanel.gameObject.SetActive(true);
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
                    _HUDPanel.gameObject.SetActive(true);
                    _HUDPanel.LeanAlpha(1, 0.4f);
                });
            }
            else
            {
                _HUDPanel.LeanAlpha(0, 0.4f).setOnComplete(() =>
                {
                    _HUDPanel.gameObject.SetActive(false);
                });
                _pausePanel.gameObject.SetActive(true);
                _pausePanel.LeanAlpha(1, 0.4f).setOnComplete(() =>
                {
                    Time.timeScale = 0;
                });
            }
        }
    }
}
