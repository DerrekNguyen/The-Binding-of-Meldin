using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

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

    
    // Start is called before the first frame update
    void Start()
    {
        UpdateUI();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateUI();
    }

    private void UpdateUI()
    {
        // Update current value
        if (current != null)
        {
            int currentValue = PlayerPrefs.GetInt(currentPlayerPrefString, 0);
            current.text = currentPrefix + currentValue;
        }

        // Update cost value
        if (cost != null)
        {
            int costValue = PlayerPrefs.GetInt(costPlayerPrefString, 0);
            cost.text = costPrefix + costValue;
        }

        // Update increase value
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
        
        // Check if player has enough coins
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
