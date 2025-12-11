using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;
using UnityEngine.SceneManagement;

// In game UI manager

public class InGameUiManager : MonoBehaviour
{
    public GameObject mainHUDUI;
    public GameObject pauseUI;
    public GameObject pauseBackground;
    public GameObject settingsMenu;
    
    public static bool isPaused = false;

    void Start()
    {
        if(pauseUI != null)
        {
            pauseUI.SetActive(false);
        }
        if(pauseBackground != null)
        {
            pauseBackground.SetActive(false);
        }
        if(settingsMenu != null)
        {
            settingsMenu.SetActive(false);
        }        
        isPaused = false;
    }

    public void OnGameShutdown()
    {
        Application.Quit();
    }

    public void OnPause()
    {
        if(pauseUI != null)
        {
            pauseUI.SetActive(true);
        }
        if(pauseBackground != null)
        {
            pauseBackground.SetActive(true);
        }
        if(settingsMenu != null)
        {
            settingsMenu.SetActive(false);
        }
        isPaused = true;
    }

    public void OnUnpause()
    {
        if(pauseUI != null)
        {
            pauseUI.SetActive(false);
        }
        if(pauseBackground != null)
        {
            pauseBackground.SetActive(false);
        }
        if(settingsMenu != null)
        {
            settingsMenu.SetActive(false);
        }
        isPaused = false;
    }

    public void OnSettingsPress()
    {
        if(mainHUDUI != null)
        {
            mainHUDUI.SetActive(false);
        }
        if(pauseUI != null)
        {
            pauseUI.SetActive(false);
        }
        if(pauseBackground != null)
        {
            pauseBackground.SetActive(false);
        }
        if(settingsMenu != null)
        {
            settingsMenu.SetActive(true);
        }
    }

    public void OnSettingsBack()
    {
        if(mainHUDUI != null)
        {
            mainHUDUI.SetActive(true);
        }
        if(pauseUI != null)
        {
            pauseUI.SetActive(true);
        }
        if(pauseBackground != null)
        {
            pauseBackground.SetActive(true);
        }
        if(settingsMenu != null)
        {
            settingsMenu.SetActive(false);
        }
    }

    public void OnHomePress()
    {
        isPaused = false;
        SceneManager.LoadScene("MainMenu");
    }
}
