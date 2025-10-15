using System.Collections;
using UnityEngine;

public class NewPlayerCombat : MonoBehaviour
{
    [Header("Combat Settings")]
    public float knockbackForce = 15f;
    public float invincibilityDuration = 1f;
    public float knockbackDuration = 0.2f;

    private bool isInvincible;
    private SpriteRenderer spriteRenderer;
    private NewPlayerMovement playerMovement;
    private NewPlayerAnimator playerAnimator;

    void Start()
    {
        isInvincible = false;
        spriteRenderer = GetComponent<SpriteRenderer>();
        playerMovement = GetComponent<NewPlayerMovement>();
        playerAnimator = GetComponent<NewPlayerAnimator>();
    }

    // Handle taking damage
    void TakeDamage(int damage, Vector2 knockbackDirection)
    {
        NewPlayerManager.playerConfig.health -= damage;
        
        // Apply knockback using PlayerMovement
        Vector2 knockbackVelocity = knockbackDirection * knockbackForce;
        playerMovement.ApplyKnockback(knockbackVelocity, knockbackDuration);
        
        StartCoroutine(InvincibilityCoroutine());
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy") && !isInvincible)
        {
            Debug.Log("Player hit by enemy!");

            // Calculate knockback direction
            Vector2 knockbackDirection = (transform.position - other.transform.position).normalized;

            // Take damage
            TakeDamage(1, knockbackDirection);
        }
    }

    // Coroutine to handle invincibility frames
    private IEnumerator InvincibilityCoroutine()
    {
        isInvincible = true;
        float timer = 0f;

        // Wait for knockback duration
        yield return new WaitForSeconds(knockbackDuration);

        // Blinking effect during invincibility
        while (timer < invincibilityDuration)
        {
            spriteRenderer.enabled = !spriteRenderer.enabled;
            yield return new WaitForSeconds(0.1f);
            timer += 0.1f;
        }

        spriteRenderer.enabled = true;
        isInvincible = false;
    }

    // Public methods for other scripts to use
    public bool IsInvincible()
    {
        return isInvincible;
    }

    public void ForceKnockback(Vector2 direction, float force, float duration)
    {
        Vector2 knockbackVelocity = direction * force;
        playerMovement.ApplyKnockback(knockbackVelocity, duration);
    }
}