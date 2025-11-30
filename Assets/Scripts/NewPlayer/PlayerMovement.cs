using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float walkSpeed = 8f;
    [SerializeField] private float sprintSpeed = 12f;

    private Rigidbody2D _rb;
    private InputManager _input;
    private PlayerAnimationController _animController;

    // Movement direction tracking
    public bool IsMoving { get; private set; }
    public Vector2 LastFacingDirection { get; private set; } = Vector2.right; // Default facing right

    void Awake()
    {
        gameObject.layer = LayerMask.NameToLayer("Player");
        
        _rb = GetComponent<Rigidbody2D>();
        _rb.gravityScale = 0f;
        _rb.constraints = RigidbodyConstraints2D.FreezeRotation;
    }

    void Start()
    {
        _input = InputManager.Instance;
        _animController = GetComponent<PlayerAnimationController>();
    }

    void FixedUpdate()
    {
        // Don't move if dodging
        if (_input == null || PlayerDodge.IsDodging) return;

        Vector2 moveInput = _input.Move;
        float currentSpeed = _input.SprintHeld ? sprintSpeed : walkSpeed;
        _rb.velocity = moveInput.normalized * currentSpeed;

        // Track horizontal movement direction
        IsMoving = moveInput.sqrMagnitude > 0;

        // Update last facing direction when moving in any direction
        if (IsMoving)
        {
            LastFacingDirection = moveInput.normalized;
        }
    }
}