using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptables/EnemyScriptable")]
public class EnemyConfig : ScriptableObject
{
    [Header("Health (Multiplicative Scaling)")]
    public int baseHealth = 2; // Health at run count 0
    public float healthMultiplierPerRun = 1.15f; // e.g., 15% harder per run
    // Effective health: baseHealth * Mathf.Pow(healthMultiplierPerRun, runCount)

    // Helper to compute scaled health (rounded to int)
    public int GetScaledHealth()
    {
        return Mathf.Max(1, Mathf.RoundToInt(baseHealth * Mathf.Pow(healthMultiplierPerRun, PlayerPrefs.GetInt("RunCount"))));
    }

    [Header("Damage")]
    public int baseDamage = 1; // Damage at run count 0
    public float damageMultiplierPerRun = 1.1f; // e.g., 15% harder per run
    // Effective damage: baseDamage * Mathf.Pow(healthMultiplierPerRun, runCount)
    public float touchingDamageCooldown = 1f;

    // Helper to compute scaled damage (rounded to int)
    public int GetScaledDamage()
    {
        return Mathf.Max(1, Mathf.RoundToInt(baseDamage * Mathf.Pow(damageMultiplierPerRun, PlayerPrefs.GetInt("RunCount"))));
    }  


    [Header("Moving")]
    public float moveSpeed = 6f;
    public float stoppingDistance = 0.5f;
}
