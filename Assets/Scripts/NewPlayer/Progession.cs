using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;
using UnityEngine;

public class Progession : MonoBehaviour
{
    public const int playerStartHealth = 10;
    public const int playerStartRevivesCount = 1;
    public const int playerStartWeaponDamage = 1;

    private const int playerHealthUpgradeCost = 10;
    private const int playerRevivesUpgradeCost = 16;
    private const int playerWeaponDamageUpgradeCost = 8;

    private const int playerHealthUpgradeAmount = 10;
    private const int playerRevivesUpgradeAmount = 1;
    private const int playerWeaponDamageUpgradeAmount = 2;

    // PlayerPrefs keys
    private const string PREF_HEALTH = "PlayerHealth";
    private const string PREF_REVIVES = "PlayerRevives";
    private const string PREF_WEAPON_DAMAGE = "PlayerWeaponDamage";
    
    // 
    private const string PREF_COINS = "PlayerCoinCount";
    private const string RUN_COUNT = "RunCount";

    // Cost PlayerPrefs keys
    private const string PREF_HEALTH_COST = "PlayerHealthCost";
    private const string PREF_REVIVES_COST = "PlayerRevivesCost";
    private const string PREF_WEAPON_DAMAGE_COST = "PlayerWeaponDamageCost";

    // Upgrade Amount PlayerPrefs keys
    private const string PREF_HEALTH_UPGRADE = "PlayerHealthUpgrade";
    private const string PREF_REVIVES_UPGRADE = "PlayerRevivesUpgrade";
    private const string PREF_WEAPON_DAMAGE_UPGRADE = "PlayerWeaponDamageUpgrade";

    // Public accessors
    public int PlayerHealth => PlayerPrefs.GetInt(PREF_HEALTH, playerStartHealth);
    public int PlayerRevives => PlayerPrefs.GetInt(PREF_REVIVES, playerStartRevivesCount);
    public int PlayerWeaponDamage => PlayerPrefs.GetInt(PREF_WEAPON_DAMAGE, playerStartWeaponDamage);
    public int PlayerCoinCount => PlayerPrefs.GetInt(PREF_COINS, 0);
    public int RunCount => PlayerPrefs.GetInt(RUN_COUNT, 0);

    void Start()
    {
        CheckFirstTimePlay();
    }

    private void CheckFirstTimePlay()
    {
        // Check if all player prefs are at default (0) - indicates first time playing
        bool healthIsZero = !PlayerPrefs.HasKey(PREF_HEALTH);
        bool revivesIsZero = !PlayerPrefs.HasKey(PREF_REVIVES);
        bool weaponDamageIsZero = !PlayerPrefs.HasKey(PREF_WEAPON_DAMAGE);
        bool coinsIsZero = !PlayerPrefs.HasKey(PREF_COINS);
        bool runCountIsZero = !PlayerPrefs.HasKey(RUN_COUNT);

        // If all are uninitialized, it's the first time playing
        if (healthIsZero && revivesIsZero && weaponDamageIsZero && coinsIsZero && runCountIsZero)
        {
            HardResetProgression();
        }
    }

    public void HardResetProgression()
    {
        // All player preferences set to start amount
        PlayerPrefs.SetInt(PREF_HEALTH, playerStartHealth);
        PlayerPrefs.SetInt(PREF_REVIVES, playerStartRevivesCount);
        PlayerPrefs.SetInt(PREF_WEAPON_DAMAGE, playerStartWeaponDamage);
        
        // Coins set to zero
        PlayerPrefs.SetInt(PREF_COINS, 0);
        PlayerPrefs.SetInt(RUN_COUNT, 0);

        // Set costs
        PlayerPrefs.SetInt(PREF_HEALTH_COST, playerHealthUpgradeCost);
        PlayerPrefs.SetInt(PREF_REVIVES_COST, playerRevivesUpgradeCost);
        PlayerPrefs.SetInt(PREF_WEAPON_DAMAGE_COST, playerWeaponDamageUpgradeCost);

        // Set upgrade amounts
        PlayerPrefs.SetInt(PREF_HEALTH_UPGRADE, playerHealthUpgradeAmount);
        PlayerPrefs.SetInt(PREF_REVIVES_UPGRADE, playerRevivesUpgradeAmount);
        PlayerPrefs.SetInt(PREF_WEAPON_DAMAGE_UPGRADE, playerWeaponDamageUpgradeAmount);
        
        PlayerPrefs.Save();
    }

    public void AddToCoinCount(int count)
    {
        // add count amount to PlayerCoinCount player preference
        int currentCoins = PlayerPrefs.GetInt(PREF_COINS);
        PlayerPrefs.SetInt(PREF_COINS, currentCoins + count);
        PlayerPrefs.Save();
    }

    public void AddToRunCount()
    {
        // add count amount to RunCount player preference
        int currentRunCount = PlayerPrefs.GetInt(RUN_COUNT);
        PlayerPrefs.SetInt(RUN_COUNT, currentRunCount + 1);
        PlayerPrefs.Save();
    }
}
