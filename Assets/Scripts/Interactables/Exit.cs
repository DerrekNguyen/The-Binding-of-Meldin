using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// Handles the exit behavior

public class Exit : MonoBehaviour
{
    public bool playerInside = false;
    public bool playerInitiated = false;

    private InputManager inputsManager;

    void Start()
    {
        inputsManager = InputManager.Instance;
    }

    void Update()
    {
        if (playerInside && inputsManager != null && inputsManager.InteractPressed)
        {
            playerInitiated = true;
        }
        
        if(playerInitiated)
        {
            int currentRunCount = PlayerPrefs.GetInt("RunCount", 0);
            PlayerPrefs.SetInt("RunCount", currentRunCount + 1);
            PlayerPrefs.Save();

            SceneManager.LoadScene("PreRun");
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInside = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInside = false;
        }
    }
}
