using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoPlayerDamageOnTouch : MonoBehaviour
{
    private int damageAmount = 1;
    [SerializeField] private float cooldownTime = 1f;

    public bool IsOnCooldown { get; private set; }
    private float cooldownTimer = 0f;

    void Start()
    {
        int runCount = PlayerPrefs.GetInt("RunCount");
        damageAmount = 1 + (runCount * 2);
    }

    void Update()
    {
        if (IsOnCooldown)
        {
            cooldownTimer -= Time.deltaTime;
            if (cooldownTimer <= 0f)
            {
                IsOnCooldown = false;
            }
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (IsOnCooldown) return;

        if (other.CompareTag("Player") && other.gameObject.name == "Hitbox")
        {
            PlayerLifecycle playerLifecycle = other.GetComponentInParent<PlayerLifecycle>();
            if (playerLifecycle != null)
            {
                playerLifecycle.DecreaseHealth(damageAmount);
                IsOnCooldown = true;
                cooldownTimer = cooldownTime;
            }
        }
    }
}
