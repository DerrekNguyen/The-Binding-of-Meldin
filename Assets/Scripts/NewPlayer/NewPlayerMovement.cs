using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class NewPlayerMovement : MonoBehaviour
{
    public enum FacingDirection
    {
        Up,
        Down,
        Left,
        Right
    }

    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float sprintMultiplier = 1.5f;
    
    [Header("Dodge Settings")]
    [SerializeField] private float dodgeForce = 10f;
    [SerializeField] private float dodgeDuration = 0.3f;
    [SerializeField] private float dodgeCooldown = 1f;
    
    [Header("Knockback Settings")]
    [SerializeField] private float knockbackResistance = 0.5f;

    // State booleans
    public bool isSprinting { get; private set; } = false;
    public bool isDodging { get; private set; } = false;
    public bool isKnockbacked { get; private set; } = false;
    public bool justDied { get; private set; } = false;
    public bool isMoving { get; private set; } = false;
    public bool facingLeft { get; private set; } = true;
    public FacingDirection facing { get; private set; } = FacingDirection.Left;

    // Components
    private Rigidbody2D rb;
    private Controls controls;
    
    // Private variables
    private Vector2 moveInput;
    private Vector2 dodgeDirection;
    private float lastDodgeTime;
    private Coroutine dodgeCoroutine;
    private Coroutine knockbackCoroutine;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        controls = new Controls();
    }

    void OnEnable()
    {
        controls.Enable();
        controls.Player.Dodge.performed += OnDodgePerformed;
        controls.Player.Sprint.started += OnSprintStarted;
        controls.Player.Sprint.canceled += OnSprintCanceled;
    }

    void OnDisable()
    {
        controls.Player.Dodge.performed -= OnDodgePerformed;
        controls.Player.Sprint.started -= OnSprintStarted;
        controls.Player.Sprint.canceled -= OnSprintCanceled;
        controls.Disable();
    }

    void Update()
    {
        if (justDied) return;
        
        HandleInput();
        UpdateFacing();
    }

    void FixedUpdate()
    {
        if (justDied) return;
        
        HandleMovement();
    }

    private void HandleInput()
    {
        moveInput = controls.Player.Move.ReadValue<Vector2>();
        isMoving = moveInput.magnitude > 0.1f;
    }

    private void UpdateFacing()
    {
        if (moveInput.magnitude > 0.1f)
        {
            // Determine facing direction based on the strongest input axis
            if (Mathf.Abs(moveInput.x) > Mathf.Abs(moveInput.y))
            {
                // Horizontal movement is stronger
                facing = moveInput.x > 0 ? FacingDirection.Right : FacingDirection.Left;
            }
            else
            {
                // Vertical movement is stronger
                facing = moveInput.y > 0 ? FacingDirection.Up : FacingDirection.Down;
            }
        }
        
        if (moveInput.x > 0)
        {
            facingLeft = false;
        }
        if (moveInput.x < 0)
        {
            facingLeft = true;
        }

    }

    // Get the facing direction as a Vector2
    public Vector2 GetFacingVector()
    {
        return facing switch
        {
            FacingDirection.Up => Vector2.up,
            FacingDirection.Down => Vector2.down,
            FacingDirection.Left => Vector2.left,
            FacingDirection.Right => Vector2.right,
            _ => Vector2.right
        };
    }

    private void HandleMovement()
    {
        if (isDodging || isKnockbacked) return;
        
        float currentSpeed = isSprinting ? moveSpeed * sprintMultiplier : moveSpeed;
        Vector2 movement = moveInput * currentSpeed;
        
        rb.velocity = movement;
    }

    private void OnSprintStarted(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        if (!justDied && !isDodging && !isKnockbacked)
            isSprinting = true;
    }

    private void OnSprintCanceled(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        isSprinting = false;
    }

    private void OnDodgePerformed(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        if (justDied || isDodging || isKnockbacked) return;
        if (Time.time - lastDodgeTime < dodgeCooldown) return;
        
        StartDodge();
    }

    private void StartDodge()
    {
        isDodging = true;
        lastDodgeTime = Time.time;
        
        // Use current facing direction for dodge
        dodgeDirection = GetFacingVector();
        
        if (dodgeCoroutine != null)
            StopCoroutine(dodgeCoroutine);
        
        dodgeCoroutine = StartCoroutine(DodgeCoroutine());
    }

    private IEnumerator DodgeCoroutine()
    {
        rb.velocity = dodgeDirection * dodgeForce;
        
        yield return new WaitForSeconds(dodgeDuration);
        
        isDodging = false;
        rb.velocity = Vector2.zero;
    }

    public void ApplyKnockback(Vector2 knockbackForce, float duration = 0.5f)
    {
        if (justDied) return;
        
        // Apply resistance
        knockbackForce *= (1f - knockbackResistance);
        
        if (knockbackCoroutine != null)
            StopCoroutine(knockbackCoroutine);
        
        knockbackCoroutine = StartCoroutine(KnockbackCoroutine(knockbackForce, duration));
    }

    private IEnumerator KnockbackCoroutine(Vector2 knockbackForce, float duration)
    {
        isKnockbacked = true;
        rb.velocity = knockbackForce;
        
        yield return new WaitForSeconds(duration);
        
        isKnockbacked = false;
        rb.velocity = Vector2.zero;
    }

    public void SetPlayerDied(bool died)
    {
        justDied = died;
        
        if (died)
        {
            // Stop all movement
            rb.velocity = Vector2.zero;
            isSprinting = false;
            isDodging = false;
            isKnockbacked = false;
            isMoving = false;
            
            // Stop any ongoing coroutines
            if (dodgeCoroutine != null)
                StopCoroutine(dodgeCoroutine);
            if (knockbackCoroutine != null)
                StopCoroutine(knockbackCoroutine);
        }
    }
}