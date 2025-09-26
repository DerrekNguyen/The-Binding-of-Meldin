using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InGameUiManager : MonoBehaviour
{
    public GameObject pauseUI;
    public GameObject pauseBackground;

    void Start()
    {
        pauseUI.SetActive(false);
        pauseBackground.SetActive(false);        
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
    }

    // Unpause Button Press
    public void OnUnpause()
    {
        pauseUI.SetActive(false);
        pauseBackground.SetActive(false);
    }

    public void OnSettingsPress()
    {

    }

    public void OnHomePress()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
