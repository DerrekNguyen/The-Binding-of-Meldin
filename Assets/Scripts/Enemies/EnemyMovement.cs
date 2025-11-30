using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]
public class EnemyMovement : MonoBehaviour
{
    private GameObject player;
    private PlayerLifecycle playerLifecycle;
    private Rigidbody2D rb;
    public float speed = 8f;
    public float stoppingDistance = 0.5f;
    
    private bool playerWasDead = false;
    private bool currentlyRetreating = false;

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
        
        rb = GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.gravityScale = 0f;
            rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        }
    }

    void FixedUpdate()
    {
        if (player == null || rb == null)
            return;

        if (playerLifecycle == null)
        {
            float distanceToPlayer = Vector2.Distance(transform.position, player.transform.position);
            if (distanceToPlayer > stoppingDistance)
            {
                Vector2 chaseDirection = (player.transform.position - transform.position).normalized;
                rb.velocity = chaseDirection * speed;
            }
            else
            {
                rb.velocity = Vector2.zero;
            }
            return;
        }

        bool playerIsDeadNow = playerLifecycle.IsDead;

        if (playerIsDeadNow && !playerWasDead && !currentlyRetreating)
        {
            StartCoroutine(DoRetreat());
        }

        playerWasDead = playerIsDeadNow;

        if (currentlyRetreating || playerIsDeadNow)
        {
            return;
        }

        float distance = Vector2.Distance(transform.position, player.transform.position);
        if (distance > stoppingDistance)
        {
            Vector2 direction = (player.transform.position - transform.position).normalized;
            rb.velocity = direction * speed;
        }
        else
        {
            rb.velocity = Vector2.zero;
        }
    }

    private IEnumerator DoRetreat()
    {
        currentlyRetreating = true;
        rb.simulated = false;
        
        // Disable all colliders
        Collider2D[] colliders = GetComponents<Collider2D>();
        foreach (var col in colliders)
        {
            col.enabled = false;
        }
        
        Vector2 awayDirection = ((Vector2)transform.position - (Vector2)player.transform.position);
        if (awayDirection.magnitude < 0.1f)
        {
            awayDirection = Random.insideUnitCircle.normalized;
        }
        else
        {
            awayDirection = awayDirection.normalized;
        }
        
        Vector2 startPosition = transform.position;
        Vector2 targetPosition = startPosition + (awayDirection * 3f);
        
        float elapsed = 0f;
        float duration = 0.5f;
        
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float progress = elapsed / duration;
            transform.position = Vector2.Lerp(startPosition, targetPosition, progress);
            yield return null;
        }
        
        // Re-enable colliders
        foreach (var col in colliders)
        {
            col.enabled = true;
        }
        
        rb.simulated = true;
        rb.velocity = Vector2.zero;
        currentlyRetreating = false;
    }
}
