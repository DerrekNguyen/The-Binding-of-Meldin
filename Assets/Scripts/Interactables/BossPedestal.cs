using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Handles the boss pedestal behavior

public class BossPedestal : MonoBehaviour
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
            if (SoundManager.Instance != null) SoundManager.Instance.PlaySound2D("countdown");
            Debug.Log("[BossPedestal] Player initiated boss fight!");
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
