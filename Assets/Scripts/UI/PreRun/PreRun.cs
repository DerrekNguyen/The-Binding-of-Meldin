using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

// PreRun screen UI

public class PreRun : MonoBehaviour
{
    [Header("Coin Text")]
    [SerializeField] private TextMeshProUGUI coinCount;
    [SerializeField] private string coinCountPlayerPrefString = "PlayerCoinCount";
    [SerializeField] private string currentPrefix = "COINS: ";

    [Header("Reset Menu")]
    [SerializeField] private GameObject resetMenu;
    [SerializeField] private bool isDisplayed;

    private Progession progression;
    
    void Start()
    {
        progression = FindObjectOfType<Progession>();

        if (resetMenu != null)
        {
            resetMenu.SetActive(false);
            isDisplayed = false;
        }
    }

    void Update()
    {
        UpdateCoinDisplay();
    }

    private void UpdateCoinDisplay()
    {
        if (coinCount != null)
        {
            int coins = PlayerPrefs.GetInt(coinCountPlayerPrefString, 0);
            coinCount.text = currentPrefix + coins;
        }
    }

    public void OpenResetMenu()
    {
        if (resetMenu != null)
        {
            resetMenu.SetActive(true);
            isDisplayed = true;
        }
    }

    public void CloseResetMenu()
    {
        if (resetMenu != null)
        {
            resetMenu.SetActive(false);
            isDisplayed = false;
        }
    }

    public void ResetProgress()
    {
        if (progression != null)
        {
            progression.HardResetProgression();
        }
        CloseResetMenu();
    }
}
