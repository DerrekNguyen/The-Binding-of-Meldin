using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Enemy config scriptable object

[CreateAssetMenu(menuName = "Scriptables/EnemyScriptable")]
public class EnemyConfig : ScriptableObject
{
    [Header("Health (Multiplicative Scaling)")]
    public int baseHealth = 2;
    public float healthMultiplierPerRun = 1.15f;

    public int GetScaledHealth()
    {
        return Mathf.Max(1, Mathf.RoundToInt(baseHealth * Mathf.Pow(healthMultiplierPerRun, PlayerPrefs.GetInt("RunCount"))));
    }

    [Header("Damage")]
    public int baseDamage = 1;
    public float damageMultiplierPerRun = 1.1f;
    public float touchingDamageCooldown = 1f;

    public int GetScaledDamage()
    {
        return Mathf.Max(1, Mathf.RoundToInt(baseDamage * Mathf.Pow(damageMultiplierPerRun, PlayerPrefs.GetInt("RunCount"))));
    }  


    [Header("Moving")]
    public float moveSpeed = 6f;
    public float stoppingDistance = 0.5f;

    [Header("Sounds")]
    public string deathSoundName;
    public string takeDamageSoundName;
    public string hitPlayerSoundName;
}
