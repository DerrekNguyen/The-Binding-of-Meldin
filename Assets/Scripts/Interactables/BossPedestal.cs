using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
