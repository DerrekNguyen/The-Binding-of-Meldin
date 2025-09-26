using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GeneralSceneChanges : MonoBehaviour
{
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
}