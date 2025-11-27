using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;
using UnityEngine;

public class Progession : MonoBehaviour
{
    private const int playerStartHealth = 10;
    private const int playerStartRevivesCount = 0;
    private const int playerStartWeaponDamage = 1;

    private const int playerHealthUpgradeCost = 10;
    private const int playerRevivesUpgradeCost = 20;
    private const int playerWeaponDamageUpgradeCost = 4;

    private const int playerHealthUpgradeAmount = 10;
    private const int playerRevivesUpgradeAmount = 1;
    private const int playerWeaponDamageUpgradeAmount = 2;

    // PlayerPrefs keys
    private const string PREF_HEALTH = "PlayerHealth";
    private const string PREF_REVIVES = "PlayerRevives";
    private const string PREF_WEAPON_DAMAGE = "PlayerWeaponDamage";
    private const string PREF_COINS = "PlayerCoinCount";

    private const string RUN_COUNT = "RunCount";

    // Public accessors
    public int PlayerHealth => PlayerPrefs.GetInt(PREF_HEALTH, playerStartHealth);
    public int PlayerRevives => PlayerPrefs.GetInt(PREF_REVIVES, playerStartRevivesCount);
    public int PlayerWeaponDamage => PlayerPrefs.GetInt(PREF_WEAPON_DAMAGE, playerStartWeaponDamage);
    public int PlayerCoinCount => PlayerPrefs.GetInt(PREF_COINS, 0);
    public int RunCount => PlayerPrefs.GetInt(RUN_COUNT, 0);

    public void HardResetProgression()
    {
        // All player preferences set to start amount
        PlayerPrefs.SetInt(PREF_HEALTH, playerStartHealth);
        PlayerPrefs.SetInt(PREF_REVIVES, playerStartRevivesCount);
        PlayerPrefs.SetInt(PREF_WEAPON_DAMAGE, playerStartWeaponDamage);
        
        // Coins set to zero
        PlayerPrefs.SetInt(PREF_COINS, 0);

        PlayerPrefs.SetInt(RUN_COUNT, 0);
        
        PlayerPrefs.Save();
    }

    public void PurchaseHealthUpgrade()
    {
        // if we have enough to buy in PlayerCoinCount
        if (PlayerCoinCount >= playerHealthUpgradeCost)
        {
            // Add respective amount to PlayerHealth player preference
            int currentHealth = PlayerPrefs.GetInt(PREF_HEALTH, playerStartHealth);
            PlayerPrefs.SetInt(PREF_HEALTH, currentHealth + playerHealthUpgradeAmount);
            
            // Subtract cost from PlayerCoinCount
            int currentCoins = PlayerPrefs.GetInt(PREF_COINS, 0);
            PlayerPrefs.SetInt(PREF_COINS, currentCoins - playerHealthUpgradeCost);
            
            PlayerPrefs.Save();
        }
    }

    public void PurchaseReviveUpgrade()
    {
        // if we have enough to buy in PlayerCoinCount
        if (PlayerCoinCount >= playerRevivesUpgradeCost)
        {
            // Add respective amount to PlayerRevives player preference
            int currentRevives = PlayerPrefs.GetInt(PREF_REVIVES, playerStartRevivesCount);
            PlayerPrefs.SetInt(PREF_REVIVES, currentRevives + playerRevivesUpgradeAmount);
            
            // Subtract cost from PlayerCoinCount
            int currentCoins = PlayerPrefs.GetInt(PREF_COINS, 0);
            PlayerPrefs.SetInt(PREF_COINS, currentCoins - playerRevivesUpgradeCost);
            
            PlayerPrefs.Save();
        }
    }

    public void PurchaseWeaponDamageUpgrade()
    {
        // if we have enough to buy in PlayerCoinCount
        if (PlayerCoinCount >= playerWeaponDamageUpgradeCost)
        {
            // Add respective amount to WeaponDamage player preference
            int currentDamage = PlayerPrefs.GetInt(PREF_WEAPON_DAMAGE, playerStartWeaponDamage);
            PlayerPrefs.SetInt(PREF_WEAPON_DAMAGE, currentDamage + playerWeaponDamageUpgradeAmount);
            
            // Subtract cost from PlayerCoinCount
            int currentCoins = PlayerPrefs.GetInt(PREF_COINS, 0);
            PlayerPrefs.SetInt(PREF_COINS, currentCoins - playerWeaponDamageUpgradeCost);
            
            PlayerPrefs.Save();
        }
    }

    public void AddToCoinCount(int count)
    {
        // add count amount to PlayerCoinCount player preference
        int currentCoins = PlayerPrefs.GetInt(PREF_COINS, 0);
        PlayerPrefs.SetInt(PREF_COINS, currentCoins + count);
        PlayerPrefs.Save();
    }

    public void AddToRunCount()
    {
        // add count amount to PlayerCoinCount player preference
        int currentRunCount = PlayerPrefs.GetInt(RUN_COUNT, 0);
        PlayerPrefs.SetInt(PREF_COINS, currentRunCount + 1);
        PlayerPrefs.Save();
    }
}
