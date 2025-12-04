using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GeneralSceneChanges : MonoBehaviour
{
    public void ToMainMenuResetMadeProgress()
    {
        var progression = FindObjectOfType<Progession>();
        if (progression != null)
        {
            progression.RestoreFallbackSnapshot();
        }
        SceneManager.LoadScene("MainMenu");
    }

    public void ToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void ToCredits()
    {
        SceneManager.LoadScene("Credits");
    }

    public void ToGameScene()
    {
        SceneManager.LoadScene("GameScene");
    }

    public void ToHowTo()
    {
        SceneManager.LoadScene("HowTo");
    }

    public void ToPreRun()
    {
        SceneManager.LoadScene("PreRun");
    }
}