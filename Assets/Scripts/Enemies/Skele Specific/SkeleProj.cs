using UnityEngine;

public class SkeleProj : MonoBehaviour
{
    private Vector2 direction;
    private float speed;
    private int damageAmount;
    private float lifetime = 3f;
    private float elapsedTime = 0f;

    public void Initialize(GameObject playerTarget, float projectileSpeed)
    {
        speed = projectileSpeed;
        
        // Calculate damage based on run count
        int runCount = PlayerPrefs.GetInt("RunCount", 0);
        damageAmount = 1 + (runCount * 2);
        
        // Calculate direction to player ONCE at spawn
        if (playerTarget != null)
        {
            direction = (playerTarget.transform.position - transform.position).normalized;
            
            // Set initial rotation
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0, 0, angle);
        }
        else
        {
            direction = Vector2.right; // Default direction if no target
        }
        
        // Add Rigidbody2D if not present
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if (rb == null)
        {
            rb = gameObject.AddComponent<Rigidbody2D>();
            rb.isKinematic = true;
            rb.gravityScale = 0f;
        }
        
        // Add Collider2D if not present
        if (!TryGetComponent<Collider2D>(out var col))
        {
            CircleCollider2D circleCol = gameObject.AddComponent<CircleCollider2D>();
            circleCol.radius = 0.1f;
            circleCol.isTrigger = true;
        }
    }

    void Update()
    {
        if (InGameUiManager.isPaused) return;
        
        // Update lifetime timer (only when not paused)
        elapsedTime += Time.deltaTime;
        if (elapsedTime >= lifetime)
        {
            Destroy(gameObject);
            return;
        }
        
        // Move in the fixed direction
        transform.position += (Vector3)direction * speed * Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && other.gameObject.name == "Hitbox")
        {
            // Damage the player
            PlayerLifecycle playerLifecycle = other.GetComponentInParent<PlayerLifecycle>();
            
            if (playerLifecycle != null)
            {
                playerLifecycle.DecreaseHealth(damageAmount);
            }
            
            Destroy(gameObject);
        }
        else if (other.CompareTag("Collision"))
        {
            Destroy(gameObject);
        }
    }
}
