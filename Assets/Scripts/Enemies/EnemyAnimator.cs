using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Handles enemy animations

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

    void Start()
    {
        _animator = GetComponent<Animator>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _movement = GetComponent<EnemyMovement>();
    }

    void Update()
    {
        if (_movement == null || isPlayingDeathAnimation) return;

        _animator.Play(_movement.IsMoving ? moveStateName : idleStateName);

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
        
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.velocity = Vector2.zero;
        }
        
        _animator.Play(deathStateName);
    }

    public float GetDeathAnimationLength()
    {
        if (isPlayingDeathAnimation && _animator != null)
        {
            AnimatorStateInfo stateInfo = _animator.GetCurrentAnimatorStateInfo(0);
            if (stateInfo.IsName(deathStateName))
            {
                return stateInfo.length;
            }
        }
        
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
