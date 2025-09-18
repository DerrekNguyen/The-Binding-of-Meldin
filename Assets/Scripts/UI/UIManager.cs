using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NewBehaviourScript : MonoBehaviour
{
    public GameObject pauseUI;
    public GameObject pauseBackground;

    void Start()
    {
        pauseUI.SetActive(false);
        pauseBackground.SetActive(false);        
    }

    // Shutdown Game Button Press
    public void onGameShutdown()
    {
        Application.Quit();
    }

    // Pause Button Press
    public void onPause()
    {
        pauseUI.SetActive(true);
        pauseBackground.SetActive(true);
    }

    // Unpause Button Press
    public void onUnpause()
    {
        pauseUI.SetActive(false);
        pauseBackground.SetActive(false);
    }

    public void onSettingsPress()
    {

    }

    public void onHomePress()
    {
        
    }
}
