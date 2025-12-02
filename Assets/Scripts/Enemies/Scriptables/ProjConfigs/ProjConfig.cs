using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptables/ProjScriptable")]
public class ProjConfig : ScriptableObject
{
    public enum ProjectileType
    {
        Slime,
        Skele
    }

    [Header("Bullet Settings")]
    public GameObject projectilePrefab;
    public float projSpeed;
    public ProjectileType projectileType;
    public float lifetime;

    [Header("Interval")]
    public float min;
    public float max;
}
