using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(PlayerMovement))]
[RequireComponent(typeof(PlayerLifecycle))]
public class PlayerAnimationController : MonoBehaviour
{
    [Header("Animation Parameters")]
    [SerializeField] private RuntimeAnimatorController animatorController;
    [SerializeField] private string idleStateName = "Idle";
    [SerializeField] private string moveStateName = "Move";
    [SerializeField] private string dodgeStateName = "Dodge";
    [SerializeField] private string deathStateName = "Death";
    [SerializeField] private string reviveStateName = "Revive";

    private Animator _animator;
    private SpriteRenderer _spriteRenderer;
    private PlayerMovement _movement;
    private PlayerLifecycle _lifecycle;
    private bool _hasPlayedDeath = false;
    private bool _isPlayingRevive = false;

    public float DodgeAnimationLength { get; private set; }
    public bool IsDeathAnimationComplete { get; private set; }
    public bool IsReviveAnimationComplete { get; private set; }

    // Start is called before the first frame update
    void Start()
    {
        _animator = GetComponent<Animator>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _movement = GetComponent<PlayerMovement>();
        _lifecycle = GetComponent<PlayerLifecycle>();

        // Cache dodge animation length from animator controller
        DodgeAnimationLength = GetAnimationLength(dodgeStateName);
    }

    // Update is called once per frame
    void Update()
    {
        if (_movement == null || _lifecycle == null) return;

        // Death state - play death animation when dead and NOT reviving
        if (_lifecycle.IsDead && !_lifecycle.IsReviving)
        {
            if (!_hasPlayedDeath)
            {
                _animator.speed = 1f;
                _animator.Play(deathStateName);
                _hasPlayedDeath = true;
                _isPlayingRevive = false;
                IsReviveAnimationComplete = false;
                IsDeathAnimationComplete = false;
                StartCoroutine(WaitForAnimationEnd(deathStateName, () => IsDeathAnimationComplete = true, true));
            }
            return;
        }

        // Revive state - play revive animation when reviving (only once)
        if (_lifecycle.IsReviving && _hasPlayedDeath && !_isPlayingRevive && !IsReviveAnimationComplete)
        {
            _animator.speed = 1f;
            _animator.Play(reviveStateName);
            _hasPlayedDeath = false;
            _isPlayingRevive = true;
            StartCoroutine(WaitForAnimationEnd(reviveStateName, () => {
                IsReviveAnimationComplete = true;
                _isPlayingRevive = false;
            }));
            return;
        }

        // Stay in revive state until complete
        if (_lifecycle.IsReviving) return;

        // Reset for next cycle when fully alive
        if (!_lifecycle.IsDead && IsReviveAnimationComplete)
        {
            IsReviveAnimationComplete = false;
        }

        // Dodge state
        if (PlayerDodge.IsDodging)
        {
            _animator.Play(dodgeStateName);
            return;
        }

        // Movement states
        _animator.Play(_movement.IsMoving ? moveStateName : idleStateName);

        // Flip sprite based on last facing direction
        if (_movement.LastFacingDirection.x != 0)
        {
            _spriteRenderer.flipX = _movement.LastFacingDirection.x > 0;
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
