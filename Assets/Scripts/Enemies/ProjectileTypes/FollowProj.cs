using UnityEngine;

// Handles a follow projectile

public class FollowProj : MonoBehaviour
{
    private GameObject target;
    private float speed;
    private int damageAmount;
    private float lifetime;
    private float elapsedTime = 0f;
    private string soundName;

    public void Initialize(GameObject playerTarget, float projectileSpeed, int damage, float projectileLifetime, string bulletHitPlayerSoundName)
    {
        target = playerTarget;
        speed = projectileSpeed;
        damageAmount = damage;
        lifetime = projectileLifetime;
        soundName = bulletHitPlayerSoundName;

        if (!TryGetComponent<Rigidbody2D>(out var rb))
        {
            rb = gameObject.AddComponent<Rigidbody2D>();
            rb.isKinematic = true;
            rb.gravityScale = 0f;
        }
        
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
        
        elapsedTime += Time.deltaTime;
        if (elapsedTime >= lifetime)
        {
            Destroy(gameObject);
            return;
        }
        
        if (target != null)
        {
            Vector2 direction = (target.transform.position - transform.position).normalized;
            
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0, 0, angle);
            
            transform.position += speed * Time.deltaTime * (Vector3)direction;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && other.gameObject.name == "Hitbox")
        {
            PlayerLifecycle playerLifecycle = other.GetComponent<PlayerLifecycle>();
            if (playerLifecycle == null)
            {
                playerLifecycle = other.GetComponentInParent<PlayerLifecycle>();
            }
            
            if (playerLifecycle != null)
            {
                playerLifecycle.DecreaseHealth(damageAmount);
                if (SoundManager.Instance != null) SoundManager.Instance.PlaySound2D(soundName);
            }
            
            Destroy(gameObject);
        }
        else if (other.CompareTag("Collision"))
        {
            Destroy(gameObject);
        }
    }
}
