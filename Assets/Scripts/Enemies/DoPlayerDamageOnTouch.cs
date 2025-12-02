using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoPlayerDamageOnTouch : MonoBehaviour
{
    private int damageAmount = 1;
    [SerializeField] private float cooldownTime;
    [SerializeField] private EnemyConfig enemyConfig;

    private Coroutine damageCoroutine;

    void Start()
    {
        if (enemyConfig != null)
        {
            damageAmount = enemyConfig.GetScaledDamage();
            cooldownTime = enemyConfig.touchingDamageCooldown;
        }
        else
        {
            int runCount = PlayerPrefs.GetInt("RunCount", 0);
            damageAmount = 1 + (runCount * 2);
            cooldownTime = 1f;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && other.gameObject.name == "Hitbox")
        {
            PlayerLifecycle playerLifecycle = other.GetComponentInParent<PlayerLifecycle>();
            if (playerLifecycle != null && damageCoroutine == null)
            {
                damageCoroutine = StartCoroutine(ContinuousDamage(playerLifecycle));
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player") && other.gameObject.name == "Hitbox")
        {
            if (damageCoroutine != null)
            {
                StopCoroutine(damageCoroutine);
                damageCoroutine = null;
            }
        }
    }

    private IEnumerator ContinuousDamage(PlayerLifecycle playerLifecycle)
    {
        while (true)
        {
            if (playerLifecycle != null && !playerLifecycle.IsDead)
            {
                playerLifecycle.DecreaseHealth(damageAmount);
            }
            
            yield return new WaitForSeconds(cooldownTime);
        }
    }
}
