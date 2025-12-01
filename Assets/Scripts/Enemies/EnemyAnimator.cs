using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(AIChasing))]
public class EnemyAnimator : MonoBehaviour
{
    [Header("Animation Parameters")]
    [SerializeField] private RuntimeAnimatorController animatorController;
    [SerializeField] private string idleStateName = "Idle";
    [SerializeField] private string moveStateName = "Move";
    [SerializeField] private string deathStateName = "Death";

    private Animator _animator;
    private SpriteRenderer _spriteRenderer;
    private AIChasing _movement;

    public bool IsDeathAnimationComplete { get; private set; }
    public bool IsReviveAnimationComplete { get; private set; }

    // Start is called before the first frame update
    void Start()
    {
        _animator = GetComponent<Animator>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _movement = GetComponent<AIChasing>();
    }

    // Update is called once per frame
    void Update()
    {
        if (_movement == null) return;

        // Movement states
        _animator.Play(_movement.isMoving ? moveStateName : idleStateName);

        // Flip sprite based on last facing direction
        if (_movement.LastFacingDirection != Vector2.zero)
        {
            _spriteRenderer.flipX = _movement.LastFacingDirection == Vector2.left;
        }
    }

    private IEnumerator WaitForAnimationEnd(string stateName, System.Action onComplete, bool lockOnEnd = false)
    {
        yield return null;
        AnimatorStateInfo stateInfo = _animator.GetCurrentAnimatorStateInfo(0);
        yield return new WaitForSeconds(stateInfo.length - (lockOnEnd ? 0.05f : 0));
        
        if (lockOnEnd) _animator.speed = 0f;
        onComplete?.Invoke();
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
