using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

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
    
    // Start is called before the first frame update
    void Start()
    {
        // Find the Progression script in the scene
        progression = FindObjectOfType<Progession>();

        // Ensure reset menu starts hidden
        if (resetMenu != null)
        {
            resetMenu.SetActive(false);
            isDisplayed = false;
        }
    }

    // Update is called once per frame
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
