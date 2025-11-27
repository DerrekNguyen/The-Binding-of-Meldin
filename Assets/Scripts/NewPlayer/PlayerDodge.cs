using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(PlayerAnimationController))]
[RequireComponent(typeof(PlayerMovement))]
public class PlayerDodge : MonoBehaviour
{
    [Header("Dodge Settings")]
    [SerializeField] private float dodgeSpeed = 15f;
    [SerializeField] private float dodgeCooldown = 0.5f;
    [SerializeField] private Collider2D hitboxCollider; // Assign the player's damage hitbox

    private Rigidbody2D _rb;
    private InputManager _input;
    private PlayerAnimationController _animController;
    private PlayerMovement _movement;
    private bool _canDodge = true;

    public static bool IsDodging { get; private set; }

    void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _animController = GetComponent<PlayerAnimationController>();
        _movement = GetComponent<PlayerMovement>();
    }

    void Start()
    {
        _input = InputManager.Instance;
    }

    void Update()
    {
        if (_input == null || !_canDodge || IsDodging) return;

        if (_input.DodgePressed)
        {
            Vector2 dodgeDirection = _input.Move;
            
            // If no input, dodge in last facing direction
            if (dodgeDirection.sqrMagnitude < 0.1f)
                dodgeDirection = _movement != null ? _movement.LastFacingDirection : Vector2.right;

            StartCoroutine(DodgeRoutine(dodgeDirection.normalized));
        }
    }

    private IEnumerator DodgeRoutine(Vector2 direction)
    {
        _canDodge = false;
        IsDodging = true;

        // Disable hitbox
        if (hitboxCollider != null)
            hitboxCollider.enabled = false;

        // Apply dodge velocity
        _rb.velocity = direction * dodgeSpeed;

        // Wait one frame for animation to start
        yield return null;

        // Get actual animation length from animator (accounts for speed multipliers)
        float dodgeDuration = 0.3f;
        if (_animController != null)
        {
            AnimatorStateInfo stateInfo = _animController.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0);
            dodgeDuration = stateInfo.length;
        }

        yield return new WaitForSeconds(dodgeDuration);

        // Stop dodge movement
        _rb.velocity = Vector2.zero;

        // Re-enable hitbox
        if (hitboxCollider != null)
            hitboxCollider.enabled = true;

        IsDodging = false;

        // Cooldown
        yield return new WaitForSeconds(dodgeCooldown);
        _canDodge = true;
    }
}