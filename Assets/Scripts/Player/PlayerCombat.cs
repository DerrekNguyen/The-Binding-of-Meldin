using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    public float knockbackForce = 15f; // Force of the knockback when the player takes damage
    public float invincibilityDuration = 1f; // Duration of invincibility after taking damage
    public float knockbackDuration = 0.2f; // How long knockback lasts

    private Color hurtColor;
    private Color originalColor;
    private bool isInvincible;
    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;

    // Start is called before the first frame update
    void Start()
    {
        isInvincible = false;
        hurtColor = Color.red;
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        originalColor = spriteRenderer.color;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Handle taking damage
    void TakeDamage(int damage) {
        PlayerManager.playerConfig.health -= damage;
        // StartCoroutine(invincibilityCoroutine());
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            Debug.Log("Player hit by enemy!");

            // Apply knockback
            Vector2 knockbackDirection = (transform.position - other.transform.position).normalized;
            rb.AddForce(knockbackDirection * knockbackForce, ForceMode2D.Impulse);

            // Damage calculation
            TakeDamage(1);
        }
    }

    // Coroutine to handle invincibility frames
    private IEnumerator invincibilityCoroutine() {
        isInvincible = true;
        float timer = 0f;

        // Turn red when hurt.
        // TODO: Make this a hurt animation (down the line)
        spriteRenderer.color = hurtColor;
        yield return new WaitForSeconds(knockbackDuration);
        spriteRenderer.color = originalColor;

        // Calculate invincibility duration and blinking effect
        while (timer < invincibilityDuration) {
            spriteRenderer.enabled = !spriteRenderer.enabled;
            yield return new WaitForSeconds(0.1f);
            timer += 0.1f;
        }

        spriteRenderer.enabled = true;
        isInvincible = false;
    }
}
