using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

// Shop card script for prerun screen

public class GenericShopCard : MonoBehaviour
{
    [Header("Current Tab")]
    [SerializeField] private TextMeshProUGUI current;
    [SerializeField] private string currentPlayerPrefString;
    [SerializeField] private string currentPrefix = "Current: ";

    [Header("Cost Tab")]
    [SerializeField] private TextMeshProUGUI cost;
    [SerializeField] private string costPlayerPrefString;
    [SerializeField] private string costPrefix = "Cost: ";

    [Header("Increase Tab")]
    [SerializeField] private TextMeshProUGUI increase;
    [SerializeField] private string increasePlayerPrefString;
    [SerializeField] private string increasePrefix = "Increase: ";

    private const string coinsPlayerPrefString = "PlayerCoinCount";

    
    void Start()
    {
        UpdateUI();
    }

    void Update()
    {
        UpdateUI();
    }

    private void UpdateUI()
    {
        if (current != null)
        {
            int currentValue = PlayerPrefs.GetInt(currentPlayerPrefString, 0);
            current.text = currentPrefix + currentValue;
        }

        if (cost != null)
        {
            int costValue = PlayerPrefs.GetInt(costPlayerPrefString, 0);
            cost.text = costPrefix + costValue;
        }

        if (increase != null)
        {
            int increaseValue = PlayerPrefs.GetInt(increasePlayerPrefString, 0);
            increase.text = increasePrefix + increaseValue;
        }
    }

    public void Buy()
    {
        int cost = PlayerPrefs.GetInt(costPlayerPrefString);
        int currentCoins = PlayerPrefs.GetInt(coinsPlayerPrefString);
        
        if (currentCoins >= cost)
        {
            int currentAmount = PlayerPrefs.GetInt(currentPlayerPrefString);
            int increase = PlayerPrefs.GetInt(increasePlayerPrefString);
            
            PlayerPrefs.SetInt(currentPlayerPrefString, currentAmount + increase);
            PlayerPrefs.SetInt(coinsPlayerPrefString, currentCoins - cost);        
            
            PlayerPrefs.Save();
        }
    }
}
