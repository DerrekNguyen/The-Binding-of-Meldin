using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(PlayerAnimationController))]
public class PlayerLifecycle : MonoBehaviour
{
    [Header("Health Settings")]
    [SerializeField] private int maxHealth = 0;
    [SerializeField] private int currentHealth;

    [Header("Revive Settings")]
    [SerializeField] private int maxRevives = 0;
    private int currentRevives;

    [SerializeField] private Collider2D hitboxCollider; // Assign the player's damage hitbox

    public bool IsDead { get; private set; }
    public bool IsReviving => _isReviving;
    public int CurrentRevives => currentRevives;
    public int MaxRevives => maxRevives;
    public int CurrentHealth => currentHealth;

    private InputManager _input;
    private PlayerAnimationController _animController;
    private bool _isReviving = false;
    private bool _gameOverTriggered = false;

    // Start is called before the first frame update
    void Start()
    {
        maxHealth = PlayerPrefs.GetInt("PlayerHealth");
        maxRevives = PlayerPrefs.GetInt("PlayerRevives");
        
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

        // Check if dead with no revives - trigger game over
        if (IsDead && !_gameOverTriggered && currentRevives <= 0)
        {
            if (_animController != null && _animController.IsDeathAnimationComplete)
            {
                StartCoroutine(GameOverSequence());
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

    private IEnumerator GameOverSequence()
    {
        _gameOverTriggered = true;

        // Optional: wait a moment before transitioning
        yield return new WaitForSeconds(3f);

        // Reset progression
        Progession progression = FindObjectOfType<Progession>();
        if (progression != null)
        {
            progression.HardResetProgression();
        }
        else
        {
            // Fallback if Progression script not in scene
            PlayerPrefs.DeleteAll();
            PlayerPrefs.Save();
        }

        // Load PreRun scene
        SceneManager.LoadScene("PreRun");
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
