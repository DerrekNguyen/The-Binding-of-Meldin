using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Handles picking up a heart behavior

public class HeartBehavior : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (SoundManager.Instance != null) SoundManager.Instance.PlaySound2D("healthPickup");

            int maxHealth = PlayerPrefs.GetInt("PlayerHealth");
            int healAmount = Mathf.CeilToInt(maxHealth * 0.10f);

            if (!other.TryGetComponent<PlayerLifecycle>(out var lifecycle))
            {
                lifecycle = other.GetComponentInParent<PlayerLifecycle>();
            }
            if (lifecycle != null)
            {
                lifecycle.IncreaseHealth(healAmount);
            }

            Destroy(gameObject);
        }
    }
}
