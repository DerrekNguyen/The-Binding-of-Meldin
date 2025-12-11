using UnityEngine;

// Handles player movement

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(PlayerAnimationController))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float walkSpeed = 8f;
    [SerializeField] private float sprintSpeed = 12f;

    private Rigidbody2D _rb;
    private InputManager _input;
    private PlayerAnimationController _animController;

    public bool IsMoving { get; private set; }
    public Vector2 LastFacingDirection { get; private set; } = Vector2.right;

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
        if (_input == null || PlayerDodge.IsDodging) return;

        Vector2 moveInput = _input.Move;
        float currentSpeed = _input.SprintHeld ? sprintSpeed : walkSpeed;
        _rb.velocity = moveInput.normalized * currentSpeed;

        IsMoving = moveInput.sqrMagnitude > 0;

        if (IsMoving)
        {
            LastFacingDirection = moveInput.normalized;
        }
    }
}