using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Projectile config scriptable object

[CreateAssetMenu(menuName = "Scriptables/ProjScriptable")]
public class ProjConfig : ScriptableObject
{
    public enum ProjectileType
    {
        Follow,
        Straight
    }

    [Header("Bullet Settings")]
    public GameObject projectilePrefab;
    public float projSpeed;
    public ProjectileType projectileType;
    public float lifetime;

    [Header("Interval")]
    public float min;
    public float max;

    [Header("Sounds")]
    public string bulletShotSoundName;
    public string bulletHitPlayerSoundName;
}
