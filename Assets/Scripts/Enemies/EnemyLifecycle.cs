using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Handles enemies lifecycle

public class EnemyLifecycle : MonoBehaviour
{
    private int maxHealth;
    [SerializeField] private int currentHealth;
    [SerializeField] private EnemyConfig enemyConfig;
    
    private EnemyAnimator enemyAnimator;
    private bool isDead = false;
    
    public bool IsDead => isDead;

    private const int START_HEALTH = 2;

    [System.Serializable]
    public struct DropItem
    {
        public GameObject prefab;
        [Range(0f, 1f)] public float chance;
    }

    [SerializeField] private DropItem[] drops;
    [SerializeField] private float dropPositionVariance = 0.5f;

    void Start()
    {
        int runCount = PlayerPrefs.GetInt("RunCount");
        
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
        
        if (transform.parent != null)
        {
            if (transform.parent.TryGetComponent<EnemyMovement>(out var movement))
            {
                movement.canMove = false;
            }
        }

        if (enemyAnimator != null)
        {
            enemyAnimator.PlayDeathAnimation();
            
            yield return null;
            
            float length = enemyAnimator.GetDeathAnimationLength();
            if (length > 0f)
            {
                yield return new WaitForSeconds(length);
            }
        }
        
        HandleDropsAndDestroy();
    }

    private void HandleDropsAndDestroy()
    {
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

        if (transform.parent != null)
        {
            Destroy(transform.parent.gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
