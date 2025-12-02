using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]
public class EnemyMovement : MonoBehaviour
{
    private GameObject player;
    private PlayerLifecycle playerLifecycle;
    private EnemyLifecycle enemyLifecycle;
    private Rigidbody2D rb;
    private  float speed;
    private float stoppingDistance;

    [SerializeField] private EnemyConfig enemyConfig; // config source
    
    private bool playerWasDead = false;
    private bool currentlyRetreating = false;
    
    public bool IsMoving { get; private set; }
    public bool canMove = false;

    void Start()
    {
        gameObject.layer = LayerMask.NameToLayer("Enemy");
        player = GameObject.FindGameObjectWithTag("Player");
        
        if (player != null)
        {
            playerLifecycle = player.GetComponent<PlayerLifecycle>();
            
            // If not found on player, try to find it in children
            if (playerLifecycle == null)
            {
                playerLifecycle = player.GetComponentInChildren<PlayerLifecycle>();
            }
            
            // If still not found, search all objects (last resort)
            if (playerLifecycle == null)
            {
                playerLifecycle = FindObjectOfType<PlayerLifecycle>();
            }
        }
        
        // Get EnemyLifecycle from child (hitbox)
        enemyLifecycle = GetComponentInChildren<EnemyLifecycle>();
        
        // Pull movement settings from EnemyConfig (fallback to defaults if missing)
        if (enemyConfig != null)
        {
            speed = enemyConfig.moveSpeed;
            stoppingDistance = enemyConfig.stoppingDistance;
        }
        else {
            speed = 6f;
            stoppingDistance = 0.5f;            
        }

        rb = GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.gravityScale = 0f;
            rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        }
    }

    void FixedUpdate()
    {
        // Check if game is paused
        if (InGameUiManager.isPaused)
        {
            rb.velocity = Vector2.zero;
            IsMoving = false;
            return;
        }
        
        // Check if enemy is dead
        if (enemyLifecycle != null && enemyLifecycle.IsDead)
        {
            canMove = false;
        }
        
        // Don't move if not allowed
        if (!canMove)
        {
            rb.velocity = Vector2.zero;
            IsMoving = false;
            return;
        }
        
        if (player == null || rb == null)
        {
            IsMoving = false;
            return;
        }

        if (playerLifecycle == null)
        {
            float distanceToPlayer = Vector2.Distance(transform.position, player.transform.position);
            if (distanceToPlayer > stoppingDistance)
            {
                Vector2 chaseDirection = (player.transform.position - transform.position).normalized;
                rb.velocity = chaseDirection * speed;
                IsMoving = true;
            }
            else
            {
                rb.velocity = Vector2.zero;
                IsMoving = false;
            }
            return;
        }

        bool playerIsDeadNow = playerLifecycle.IsDead;

        if (playerIsDeadNow && !playerWasDead && !currentlyRetreating)
        {
            StartCoroutine(DoRetreat());
        }

        playerWasDead = playerIsDeadNow;

        // Stop all movement if retreating or player is dead
        if (currentlyRetreating || playerIsDeadNow)
        {
            rb.velocity = Vector2.zero;
            IsMoving = false;
            return;
        }

        float distance = Vector2.Distance(transform.position, player.transform.position);
        if (distance > stoppingDistance)
        {
            Vector2 direction = (player.transform.position - transform.position).normalized;
            rb.velocity = direction * speed;
            IsMoving = true;
        }
        else
        {
            rb.velocity = Vector2.zero;
            IsMoving = false;
        }
    }

    private IEnumerator DoRetreat()
    {
        currentlyRetreating = true;
        
        // Disable physics and collisions
        rb.isKinematic = true;
        rb.velocity = Vector2.zero;
        
        Collider2D[] colliders = GetComponentsInChildren<Collider2D>();
        foreach (var col in colliders)
        {
            col.enabled = false;
        }
        
        // Calculate retreat direction
        Vector2 awayDirection = ((Vector2)transform.position - (Vector2)player.transform.position).normalized;
        if (awayDirection.magnitude < 0.1f)
        {
            awayDirection = Random.insideUnitCircle.normalized;
        }
        
        // Add random angle variation for spread
        float randomAngle = Random.Range(-45f, 45f) * Mathf.Deg2Rad;
        awayDirection = new Vector2(
            awayDirection.x * Mathf.Cos(randomAngle) - awayDirection.y * Mathf.Sin(randomAngle),
            awayDirection.x * Mathf.Sin(randomAngle) + awayDirection.y * Mathf.Cos(randomAngle)
        );
        
        // Animate retreat
        Vector2 startPosition = transform.position;
        Vector2 targetPosition = startPosition + (awayDirection * Random.Range(2.5f, 3.5f));
        float elapsed = 0f;
        float duration = 0.5f;
        
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            transform.position = Vector2.Lerp(startPosition, targetPosition, elapsed / duration);
            yield return null;
        }
        
        transform.position = targetPosition;
        
        // Re-enable physics and collisions
        foreach (var col in colliders)
        {
            col.enabled = true;
        }
        
        yield return new WaitForFixedUpdate();
        
        rb.isKinematic = false;
        rb.velocity = Vector2.zero;
        currentlyRetreating = false;
    }
}
