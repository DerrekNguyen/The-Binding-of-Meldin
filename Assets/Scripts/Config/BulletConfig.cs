using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Configs/BulletConfig")]
public class BulletConfig : ScriptableObject
{
    [Header("Visuals")]
    public float scale = 0.3f;
    public Color color = Color.white;

    [Header("Behavior")]
    public float speed = 5f;
    public float damage = 1f;
}
