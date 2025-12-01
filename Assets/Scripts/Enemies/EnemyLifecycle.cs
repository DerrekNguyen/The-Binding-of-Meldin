using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyLifecycle : MonoBehaviour
{
    [SerializeField] private int maxHealth;
    [SerializeField] private int currentHealth;
    [SerializeField] private GameObject coinPrefab;
    
    private EnemyAnimator enemyAnimator;
    private bool isDead = false;
    
    public bool IsDead => isDead;

    private const int START_HEALTH = 2;

    void Start()
    {
        int runCount = PlayerPrefs.GetInt("RunCount", 0);
        maxHealth = START_HEALTH + (runCount * 2);
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
            int damage = PlayerPrefs.GetInt("PlayerWeaponDamage", 1);
            currentHealth -= damage;

            if (currentHealth <= 0)
            {
                Die();
            }

            Destroy(other.gameObject);
        }
    }

    private void Die()
    {
        if (isDead) return;
        isDead = true;
        
        // Stop parent movement immediately
        if (transform.parent != null)
        {
            EnemyMovement movement = transform.parent.GetComponent<EnemyMovement>();
            if (movement != null)
            {
                movement.canMove = false;
            }
        }
        
        // Destroy hitbox immediately so no more damage can be taken
        Destroy(gameObject);
        
        // Trigger death animation if animator exists
        if (enemyAnimator != null)
        {
            enemyAnimator.PlayDeathAnimation(coinPrefab, transform.position);
        }
        else
        {
            // No animator, spawn coin immediately and destroy parent
            if (coinPrefab != null)
            {
                GameObject coin = Instantiate(coinPrefab, transform.position, Quaternion.identity);
                coin.SetActive(true);
            }
            
            if (transform.parent != null)
            {
                Destroy(transform.parent.gameObject);
            }
        }
    }
}
