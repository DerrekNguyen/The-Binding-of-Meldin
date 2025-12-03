using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EnemyCard : MonoBehaviour
{
    [Header("Run")]
    [SerializeField] private TextMeshProUGUI run;
    [SerializeField] private string runCountPlayerPref = "RunCount";
    [SerializeField] private string runPrefix = "RUN: ";

    [Header("HP Tab")]
    [SerializeField] private TextMeshProUGUI enemyHP;
    [SerializeField] private TextMeshProUGUI bossHP;
    [SerializeField] private string enemyHPPrefix = "ENEMY HP: ";
    [SerializeField] private string bossHPPrefix = "BOSS HP: ";

    [Header("DMG Tab")]
    [SerializeField] private TextMeshProUGUI enemyDMG;
    [SerializeField] private TextMeshProUGUI bossDMG;
    [SerializeField] private string enemyDMGPrefix = "ENEMY DMG: ";
    [SerializeField] private string bossDMGPrefix = "BOSS DMG: ";

    [Header("Scriptables")]
    [SerializeField] private EnemyConfig enemyConfig;
    [SerializeField] private EnemyConfig bossConfig;

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
        // Update run count
        if (run != null)
        {
            int runCount = PlayerPrefs.GetInt(runCountPlayerPref, 0);
            run.text = runPrefix + runCount;
        }

        // Update enemy stats
        if (enemyConfig != null)
        {
            if (enemyHP != null)
            {
                enemyHP.text = enemyHPPrefix + enemyConfig.GetScaledHealth();
            }

            if (enemyDMG != null)
            {
                enemyDMG.text = enemyDMGPrefix + enemyConfig.GetScaledDamage();
            }
        }

        // Update boss stats
        if (bossConfig != null)
        {
            if (bossHP != null)
            {
                bossHP.text = bossHPPrefix + bossConfig.GetScaledHealth();
            }

            if (bossDMG != null)
            {
                bossDMG.text = bossDMGPrefix + bossConfig.GetScaledDamage();
            }
        }
    }
}
