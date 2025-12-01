using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InGameUiManager : MonoBehaviour
{
    public GameObject mainHUDUI;
    public GameObject pauseUI;
    public GameObject pauseBackground;
    public GameObject settingsMenu;
    
    public static bool isPaused = false;

    void Start()
    {
        pauseUI.SetActive(false);
        pauseBackground.SetActive(false);
        settingsMenu.SetActive(false);
        isPaused = false;
    }

    // Shutdown Game Button Press
    public void OnGameShutdown()
    {
        Application.Quit();
    }

    // Pause Button Press
    public void OnPause()
    {
        pauseUI.SetActive(true);
        pauseBackground.SetActive(true);
        settingsMenu.SetActive(false);
        isPaused = true;
    }

    // Unpause Button Press
    public void OnUnpause()
    {
        pauseUI.SetActive(false);
        pauseBackground.SetActive(false);
        settingsMenu.SetActive(false);
        isPaused = false;
    }

    public void OnSettingsPress()
    {
        mainHUDUI.SetActive(false);
        pauseUI.SetActive(false);
        pauseBackground.SetActive(false);
        settingsMenu.SetActive(true);
    }

    public void OnSettingsBack()
    {
        mainHUDUI.SetActive(true);
        pauseUI.SetActive(true);
        pauseBackground.SetActive(true);
        settingsMenu.SetActive(false);
    }

    public void OnHomePress()
    {
        isPaused = false;
        SceneManager.LoadScene("MainMenu");
    }
}
