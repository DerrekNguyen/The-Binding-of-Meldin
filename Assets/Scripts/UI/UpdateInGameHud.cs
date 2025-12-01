using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UpdateInGameHud : MonoBehaviour
{
    [Header("HUD Text Elements")]
    [SerializeField] private TextMeshProUGUI runText;
    [SerializeField] private TextMeshProUGUI coinText;
    [SerializeField] private TextMeshProUGUI reviveText;
    [SerializeField] private TextMeshProUGUI healthText;
    
    [Header("Player Reference")]
    [SerializeField] private GameObject player;
    
    private PlayerLifecycle playerLifecycle;

    void Start()
    {
        if (player != null)
        {
            playerLifecycle = player.GetComponent<PlayerLifecycle>();
            if (playerLifecycle == null)
            {
                playerLifecycle = player.GetComponentInChildren<PlayerLifecycle>();
            }
        }
        
        if (playerLifecycle == null)
        {
            playerLifecycle = FindObjectOfType<PlayerLifecycle>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (runText != null)
        {
            int runCount = PlayerPrefs.GetInt("RunCount");
            runText.text = $"Run {runCount}";
        }

        if (coinText != null)
        {
            int coins = PlayerPrefs.GetInt("PlayerCoinCount");
            coinText.text = coins.ToString();
        }

        if (playerLifecycle != null)
        {
            if (reviveText != null)
            {
                reviveText.text = playerLifecycle.CurrentRevives.ToString();
            }

            if (healthText != null)
            {
                healthText.text = playerLifecycle.CurrentHealth.ToString();
            }
        }
    }
}
