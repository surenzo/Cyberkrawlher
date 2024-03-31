using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private CanvasGroup _mainMenuPanel;
    [SerializeField] private CanvasGroup _optionsPanel;
    [SerializeField] private CanvasGroup _creditsPanel;
    
    [SerializeField] private CanvasGroup _blackerScreen;
    
    void Start()
    {
        Time.timeScale = 1;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        
        _mainMenuPanel.alpha = 1;
        _optionsPanel.alpha = 0;
        _creditsPanel.alpha = 0;
        
        _blackerScreen.alpha = 0;
        
        
        _mainMenuPanel.gameObject.SetActive(true);
        _optionsPanel.gameObject.SetActive(false);
        _creditsPanel.gameObject.SetActive(false);
    }
    
    public void PlayGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
    
    public void QuitGame()
    {
        Debug.Log("QUIT!");
        Application.Quit();
    }

    public void CreditMenu()
    {
        _mainMenuPanel.LeanAlpha(0, 0.1f);
        _blackerScreen.LeanAlpha(1, 1).setOnComplete(() => {
            _mainMenuPanel.gameObject.SetActive(false);
            _creditsPanel.gameObject.SetActive(true);
            _creditsPanel.LeanAlpha(1, 0.5f);
        });
    }
    
    public void OptionsMenu()
    {
        _mainMenuPanel.LeanAlpha(0, 0.5f);
        _blackerScreen.LeanAlpha(1, 1).setOnComplete(() => {
            _mainMenuPanel.gameObject.SetActive(false);
            _optionsPanel.gameObject.SetActive(true);
            _optionsPanel.LeanAlpha(1, 0.5f);
        });
    }
    
    public void MainMenuButton()
    {
        if (_optionsPanel.alpha == 1)
        {
            _optionsPanel.LeanAlpha(0, 0.5f);
        }
        else if (_creditsPanel.alpha == 1)
        {
            _creditsPanel.LeanAlpha(0, 0.5f);
        }
        
        _blackerScreen.LeanAlpha(0, 1).setOnComplete(() => {
            _optionsPanel.gameObject.SetActive(false);
            _creditsPanel.gameObject.SetActive(false);
            _mainMenuPanel.gameObject.SetActive(true);
            _mainMenuPanel.LeanAlpha(1, 0.5f);
        });
    }
    
        
}
