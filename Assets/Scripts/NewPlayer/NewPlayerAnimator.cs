using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(SpriteRenderer))]
public class NewPlayerAnimator : MonoBehaviour
{
    [Header("Animation Controllers")]
    [SerializeField] private RuntimeAnimatorController idleController;
    [SerializeField] private RuntimeAnimatorController movingController;
    [SerializeField] private RuntimeAnimatorController dodgeController;
    [SerializeField] private RuntimeAnimatorController knockbackController;
    [SerializeField] private RuntimeAnimatorController deathController;

    // Animation state flags (using fields instead of properties)
    private bool _isIdleAnimPlaying = true;
    private bool _isMovingAnimPlaying = false;
    private bool _isDodgeAnimPlaying = false;
    private bool _isKnockbackAnimPlaying = false;
    private bool _isDeathAnimPlaying = false;
    
    // Components
    private Animator animator;
    private SpriteRenderer spriteRenderer;
    private NewPlayerMovement playerMovement;

    void Awake()
    {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        playerMovement = GetComponent<NewPlayerMovement>();
    }

    void Update()
    {
        UpdateAnimations();
        UpdateSpriteFlip();
    }

    private void UpdateAnimations()
    {
        // Death has highest priority
        if (playerMovement.justDied)
        {
            SetAnimationState(deathController, ref _isDeathAnimPlaying);
            return;
        }

        // Knockback has second highest priority
        if (playerMovement.isKnockbacked)
        {
            SetAnimationState(knockbackController, ref _isKnockbackAnimPlaying);
            return;
        }

        // Dodge has third priority
        if (playerMovement.isDodging)
        {
            SetAnimationState(dodgeController, ref _isDodgeAnimPlaying);
            return;
        }

        // Moving vs Idle
        if (playerMovement.isMoving)
        {
            SetAnimationState(movingController, ref _isMovingAnimPlaying);
        }
        else
        {
            SetAnimationState(idleController, ref _isIdleAnimPlaying);
        }
    }

    private void UpdateSpriteFlip()
    {
        spriteRenderer.flipX = playerMovement.facingLeft;
    }

    private void SetAnimationState(RuntimeAnimatorController controller, ref bool currentFlag)
    {
        // Reset all flags
        _isIdleAnimPlaying = false;
        _isMovingAnimPlaying = false;
        _isDodgeAnimPlaying = false;
        _isKnockbackAnimPlaying = false;
        _isDeathAnimPlaying = false;

        // Set the current flag and controller
        currentFlag = true;
        
        if (animator.runtimeAnimatorController != controller)
        {
            animator.runtimeAnimatorController = controller;
        }
    }

    // Public methods to check animation states
    public bool IsPlayingHighPriorityAnimation()
    {
        return _isDeathAnimPlaying || _isKnockbackAnimPlaying || _isDodgeAnimPlaying;
    }

    public bool IsPlayingMovementAnimation()
    {
        return _isMovingAnimPlaying || _isIdleAnimPlaying;
    }
}