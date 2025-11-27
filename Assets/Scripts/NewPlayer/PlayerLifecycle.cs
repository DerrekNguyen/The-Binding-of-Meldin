using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLifecycle : MonoBehaviour
{
    [Header("Health Settings")]
    [SerializeField] private int maxHealth = 20;
    [SerializeField] private int currentHealth;

    [Header("Revive Settings")]
    [SerializeField] private int maxRevives = 3;
    private int currentRevives;

    [SerializeField] private Collider2D hitboxCollider; // Assign the player's damage hitbox

    public bool IsDead { get; private set; }
    public bool IsReviving => _isReviving;
    public int CurrentRevives => currentRevives;
    public int MaxRevives => maxRevives;

    private InputManager _input;
    private PlayerAnimationController _animController;
    private bool _isReviving = false;

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
        currentRevives = maxRevives;
        IsDead = false;
        _input = InputManager.Instance;
        _animController = GetComponent<PlayerAnimationController>();
    }

    // Update is called once per frame
    void Update()
    {
        // Check for revive input when dead (only after death animation completes)
        if (IsDead && !_isReviving && _input != null && _input.RevivePressed && currentRevives > 0)
        {
            if (_animController != null && _animController.IsDeathAnimationComplete)
            {
                StartCoroutine(ReviveSequence());
            }
        }

        // Check health - set IsDead when health reaches zero
        if (currentHealth <= 0 && !IsDead)
        {
            currentHealth = 0;
            IsDead = true;
            _isReviving = false;
            
            if (hitboxCollider != null)
                hitboxCollider.enabled = false;
        }        
    }

    public void IncreaseHealth(int amount)
    {
        if (IsDead) return;

        currentHealth += amount;
        
        // Cap health at max
        if (currentHealth > maxHealth)
            currentHealth = maxHealth;
    }

    public void DecreaseHealth(int amount)
    {
        if (IsDead) return;

        currentHealth -= amount;

        // Check for death
        if (currentHealth <= 0)
        {
            currentHealth = 0;
            IsDead = true;
        }
    }

    private IEnumerator ReviveSequence()
    {
        _isReviving = true;
        
        // Wait for revive animation to complete
        while (_animController != null && !_animController.IsReviveAnimationComplete)
        {
            yield return null;
        }
        
        // Apply revive changes
        IsDead = false;
        _isReviving = false;
        currentHealth = maxHealth;
        currentRevives--;
        
        if (hitboxCollider != null)
            hitboxCollider.enabled = true;
    }

    public void Revive()
    {
        if (currentRevives <= 0) return;
        StartCoroutine(ReviveSequence());
    }

    public void AddRevive(int amount)
    {
        currentRevives += amount;
        if (currentRevives > maxRevives)
            currentRevives = maxRevives;
    }
}
