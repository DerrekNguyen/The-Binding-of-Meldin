using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(EnemyMovement))]
[RequireComponent(typeof(Rigidbody2D))]
public class EnemyAnimator : MonoBehaviour
{
    [Header("Animation Parameters")]
    [SerializeField] private RuntimeAnimatorController animatorController;
    [SerializeField] private string idleStateName = "Idle";
    [SerializeField] private string moveStateName = "Move";
    [SerializeField] private string deathStateName = "Death";

    private Animator _animator;
    private SpriteRenderer _spriteRenderer;
    private EnemyMovement _movement;
    
    private bool isPlayingDeathAnimation = false;

    // Start is called before the first frame update
    void Start()
    {
        _animator = GetComponent<Animator>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _movement = GetComponent<EnemyMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        if (_movement == null || isPlayingDeathAnimation) return;

        // Movement states
        _animator.Play(_movement.IsMoving ? moveStateName : idleStateName);

        // Flip sprite based on velocity direction
        if (_movement.IsMoving)
        {
            Rigidbody2D rb = GetComponent<Rigidbody2D>();
            if (rb != null && rb.velocity.x != 0)
            {
                _spriteRenderer.flipX = rb.velocity.x < 0;
            }
        }
    }

    public void PlayDeathAnimation()
    {
        isPlayingDeathAnimation = true;
        
        // Stop movement
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.velocity = Vector2.zero;
        }
        
        // Play death animation
        _animator.Play(deathStateName);
    }

    // Expose death animation length - must be called after PlayDeathAnimation
    public float GetDeathAnimationLength()
    {
        // If death animation is playing, get the actual state info length
        if (isPlayingDeathAnimation && _animator != null)
        {
            AnimatorStateInfo stateInfo = _animator.GetCurrentAnimatorStateInfo(0);
            // Check if we're in the death state
            if (stateInfo.IsName(deathStateName))
            {
                return stateInfo.length;
            }
        }
        
        // Fallback: search clips by name
        return GetAnimationLength(deathStateName);
    }

    private float GetAnimationLength(string stateName)
    {
        if (_animator.runtimeAnimatorController == null) return 0f;

        foreach (AnimationClip clip in _animator.runtimeAnimatorController.animationClips)
        {
            if (clip.name == stateName)
                return clip.length;
        }

        return 0f;
    }
}
