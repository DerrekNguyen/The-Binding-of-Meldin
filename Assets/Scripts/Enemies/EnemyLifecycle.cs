using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyLifecycle : MonoBehaviour
{
    private int maxHealth;
    [SerializeField] private int currentHealth;
    [SerializeField] private EnemyConfig enemyConfig; // config source
    
    private EnemyAnimator enemyAnimator;
    private bool isDead = false;
    
    public bool IsDead => isDead;

    private const int START_HEALTH = 2;

    [System.Serializable]
    public struct DropItem
    {
        public GameObject prefab;   // item to spawn
        [Range(0f, 1f)] public float chance; // probability (0..1)
    }

    [SerializeField] private DropItem[] drops; // vector of structs for death drops
    [SerializeField] private float dropPositionVariance = 0.5f; // random offset radius

    void Start()
    {
        int runCount = PlayerPrefs.GetInt("RunCount");
        
        // Use config if available, otherwise fallback
        if (enemyConfig != null)
        {
            maxHealth = enemyConfig.GetScaledHealth();
        }
        else
        {
            maxHealth = START_HEALTH + (runCount * 2);
        }
        currentHealth = maxHealth;
        
        if (!TryGetComponent<Rigidbody2D>(out var rb))
        {
            rb = gameObject.AddComponent<Rigidbody2D>();
            rb.isKinematic = true;
            rb.gravityScale = 0f;
        }
        
        // Find animator on parent
        if (transform.parent != null)
        {
            enemyAnimator = transform.parent.GetComponent<EnemyAnimator>();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (isDead) return;
        
        if (other.CompareTag("Bullet"))
        {
            int damage = PlayerPrefs.GetInt("PlayerWeaponDamage");
            currentHealth -= damage;
            
            if(currentHealth > 0)
            {
                if (SoundManager.Instance != null) SoundManager.Instance.PlaySound2D(enemyConfig.takeDamageSoundName);
            }

            if (currentHealth <= 0)
            {
                if (SoundManager.Instance != null) SoundManager.Instance.PlaySound2D(enemyConfig.deathSoundName);
                StartCoroutine(Die());
            }

            Destroy(other.gameObject);
        }
    }

    private IEnumerator Die()
    {
        if (isDead) yield break;
        isDead = true;
        
        // Stop parent movement immediately
        if (transform.parent != null)
        {
            if (transform.parent.TryGetComponent<EnemyMovement>(out var movement))
            {
                movement.canMove = false;
            }
        }

        // Trigger death animation if animator exists
        if (enemyAnimator != null)
        {
            enemyAnimator.PlayDeathAnimation();
            
            // Wait one frame for animator to transition to death state
            yield return null;
            
            // Now get the actual animation length from the playing state
            float length = enemyAnimator.GetDeathAnimationLength();
            if (length > 0f)
            {
                yield return new WaitForSeconds(length);
            }
        }
        
        // After animation (or immediately if no animator), handle drops and destroy
        HandleDropsAndDestroy();
    }

    private void HandleDropsAndDestroy()
    {
        // Spawn configured drops by chance with position variance
        if (drops != null && drops.Length > 0)
        {
            Vector2 basePos = transform.parent != null ? (Vector2)transform.parent.position : (Vector2)transform.position;
            foreach (var item in drops)
            {
                if (item.prefab == null) continue;
                if (Random.value <= item.chance)
                {
                    Vector2 offset = Random.insideUnitCircle * dropPositionVariance;
                    GameObject spawned = Instantiate(item.prefab, basePos + offset, Quaternion.identity);
                    spawned.SetActive(true);
                }
            }
        }

        // Finally, destroy the parent enemy object (this will also remove the hitbox)
        if (transform.parent != null)
        {
            Destroy(transform.parent.gameObject);
        }
        else
        {
            // If no parent, destroy this hitbox as a fallback
            Destroy(gameObject);
        }
    }
}
