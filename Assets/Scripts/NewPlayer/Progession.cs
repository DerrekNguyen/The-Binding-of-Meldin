using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;
using UnityEngine;

// Handles progression

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

    private const string PREF_HEALTH = "PlayerHealth";
    private const string PREF_REVIVES = "PlayerRevives";
    private const string PREF_WEAPON_DAMAGE = "PlayerWeaponDamage";
    
    private const string PREF_COINS = "PlayerCoinCount";
    private const string RUN_COUNT = "RunCount";

    private const string PREF_HEALTH_COST = "PlayerHealthCost";
    private const string PREF_REVIVES_COST = "PlayerRevivesCost";
    private const string PREF_WEAPON_DAMAGE_COST = "PlayerWeaponDamageCost";

    private const string PREF_HEALTH_UPGRADE = "PlayerHealthUpgrade";
    private const string PREF_REVIVES_UPGRADE = "PlayerRevivesUpgrade";
    private const string PREF_WEAPON_DAMAGE_UPGRADE = "PlayerWeaponDamageUpgrade";

    public int PlayerHealth => PlayerPrefs.GetInt(PREF_HEALTH, playerStartHealth);
    public int PlayerRevives => PlayerPrefs.GetInt(PREF_REVIVES, playerStartRevivesCount);
    public int PlayerWeaponDamage => PlayerPrefs.GetInt(PREF_WEAPON_DAMAGE, playerStartWeaponDamage);
    public int PlayerCoinCount => PlayerPrefs.GetInt(PREF_COINS, 0);
    public int RunCount => PlayerPrefs.GetInt(RUN_COUNT, 0);

    private int fallbackHealth;
    private int fallbackRevives;
    private int fallbackWeaponDamage;
    private int fallbackCoins;
    private int fallbackRunCount;

    private int fallbackHealthCost;
    private int fallbackRevivesCost;
    private int fallbackWeaponDamageCost;

    private int fallbackHealthUpgrade;
    private int fallbackRevivesUpgrade;
    private int fallbackWeaponDamageUpgrade;

    private bool fallbackCaptured = false;

    void Start()
    {
        CheckFirstTimePlay();
        CaptureFallbackSnapshot();
    }

    private void CaptureFallbackSnapshot()
    {
        fallbackHealth = PlayerPrefs.GetInt(PREF_HEALTH);
        fallbackRevives = PlayerPrefs.GetInt(PREF_REVIVES);
        fallbackWeaponDamage = PlayerPrefs.GetInt(PREF_WEAPON_DAMAGE);

        fallbackCoins = PlayerPrefs.GetInt(PREF_COINS);
        fallbackRunCount = PlayerPrefs.GetInt(RUN_COUNT);

        fallbackHealthCost = PlayerPrefs.GetInt(PREF_HEALTH_COST);
        fallbackRevivesCost = PlayerPrefs.GetInt(PREF_REVIVES_COST);
        fallbackWeaponDamageCost = PlayerPrefs.GetInt(PREF_WEAPON_DAMAGE_COST);

        fallbackHealthUpgrade = PlayerPrefs.GetInt(PREF_HEALTH_UPGRADE);
        fallbackRevivesUpgrade = PlayerPrefs.GetInt(PREF_REVIVES_UPGRADE);
        fallbackWeaponDamageUpgrade = PlayerPrefs.GetInt(PREF_WEAPON_DAMAGE_UPGRADE);

        fallbackCaptured = true;
    }

    public void RestoreFallbackSnapshot()
    {
        if (!fallbackCaptured) return;

        PlayerPrefs.SetInt(PREF_HEALTH, fallbackHealth);
        PlayerPrefs.SetInt(PREF_REVIVES, fallbackRevives);
        PlayerPrefs.SetInt(PREF_WEAPON_DAMAGE, fallbackWeaponDamage);

        PlayerPrefs.SetInt(PREF_COINS, fallbackCoins);
        PlayerPrefs.SetInt(RUN_COUNT, fallbackRunCount);

        PlayerPrefs.SetInt(PREF_HEALTH_COST, fallbackHealthCost);
        PlayerPrefs.SetInt(PREF_REVIVES_COST, fallbackRevivesCost);
        PlayerPrefs.SetInt(PREF_WEAPON_DAMAGE_COST, fallbackWeaponDamageCost);

        PlayerPrefs.SetInt(PREF_HEALTH_UPGRADE, fallbackHealthUpgrade);
        PlayerPrefs.SetInt(PREF_REVIVES_UPGRADE, fallbackRevivesUpgrade);
        PlayerPrefs.SetInt(PREF_WEAPON_DAMAGE_UPGRADE, fallbackWeaponDamageUpgrade);

        PlayerPrefs.Save();
    }

    private void CheckFirstTimePlay()
    {
        bool healthIsZero = !PlayerPrefs.HasKey(PREF_HEALTH);
        bool revivesIsZero = !PlayerPrefs.HasKey(PREF_REVIVES);
        bool weaponDamageIsZero = !PlayerPrefs.HasKey(PREF_WEAPON_DAMAGE);
        bool coinsIsZero = !PlayerPrefs.HasKey(PREF_COINS);
        bool runCountIsZero = !PlayerPrefs.HasKey(RUN_COUNT);

        if (healthIsZero && revivesIsZero && weaponDamageIsZero && coinsIsZero && runCountIsZero)
        {
            HardResetProgression();
        }
    }

    public void HardResetProgression()
    {
        PlayerPrefs.SetInt(PREF_HEALTH, playerStartHealth);
        PlayerPrefs.SetInt(PREF_REVIVES, playerStartRevivesCount);
        PlayerPrefs.SetInt(PREF_WEAPON_DAMAGE, playerStartWeaponDamage);
        
        PlayerPrefs.SetInt(PREF_COINS, 0);
        PlayerPrefs.SetInt(RUN_COUNT, 0);

        PlayerPrefs.SetInt(PREF_HEALTH_COST, playerHealthUpgradeCost);
        PlayerPrefs.SetInt(PREF_REVIVES_COST, playerRevivesUpgradeCost);
        PlayerPrefs.SetInt(PREF_WEAPON_DAMAGE_COST, playerWeaponDamageUpgradeCost);

        PlayerPrefs.SetInt(PREF_HEALTH_UPGRADE, playerHealthUpgradeAmount);
        PlayerPrefs.SetInt(PREF_REVIVES_UPGRADE, playerRevivesUpgradeAmount);
        PlayerPrefs.SetInt(PREF_WEAPON_DAMAGE_UPGRADE, playerWeaponDamageUpgradeAmount);
        
        PlayerPrefs.Save();
    }

    public void AddToCoinCount(int count)
    {
        int currentCoins = PlayerPrefs.GetInt(PREF_COINS);
        PlayerPrefs.SetInt(PREF_COINS, currentCoins + count);
        PlayerPrefs.Save();
    }

    public void AddToRunCount()
    {
        int currentRunCount = PlayerPrefs.GetInt(RUN_COUNT);
        PlayerPrefs.SetInt(RUN_COUNT, currentRunCount + 1);
        PlayerPrefs.Save();
    }
}
