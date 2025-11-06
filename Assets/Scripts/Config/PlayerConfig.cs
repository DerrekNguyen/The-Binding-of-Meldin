using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Configs/PlayerConfig")]
public class PlayerConfig : ScriptableObject
{
    [Header("Behavior")]
    public int health = 5;
    public int money = 0;
    public float moveSpeed = 3f;
    public float shootCooldown = 1f;
}
