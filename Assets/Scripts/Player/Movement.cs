using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    private Animator animator;
    public bool canMoveDiagonally = true; // Controls whether the player can move diagonally

    private Rigidbody2D rb; // Reference to the Rigidbody2D component attached to the player
    private Vector2 movement; // Stores the direction of player movement
    private Vector2 knockbackVelocity; // Direction of knockback when the player takes damage
    private bool isMovingHorizontally = true; // Flag to track if the player is moving horizontally
    private SpriteRenderer spriteRenderer;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        // Initialize the SpriteRenderer component
        spriteRenderer = GetComponent<SpriteRenderer>();
        // Initialize the Rigidbody2D componentaw
        rb = GetComponent<Rigidbody2D>();
        // Prevent the player from rotating
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        // Initialize and knockback velocity to zero
        knockbackVelocity = Vector2.zero;
    }

    // Update is called once per frame
    void Update()
    {
        // Get player input from keyboard or controller
        float horizontalInput = Input.GetAxisRaw("Horizontal");
        float verticalInput = Input.GetAxisRaw("Vertical");

        // Check if diagonal movement is allowed
        if (canMoveDiagonally)
        {
            // Set movement direction based on input
            movement = new Vector2(horizontalInput, verticalInput);
            // Optionally rotate the player based on movement direction
            RotatePlayer(horizontalInput, verticalInput);
        }
        else
        {
            // Determine the priority of movement based on input
            if (horizontalInput != 0)
            {
                isMovingHorizontally = true;
            }
            else if (verticalInput != 0)
            {
                isMovingHorizontally = false;
            }

            // Set movement direction and optionally rotate the player
            if (isMovingHorizontally)
            {
                movement = new Vector2(horizontalInput, 0);
                RotatePlayer(horizontalInput, 0);
            }
            else
            {
                movement = new Vector2(0, verticalInput);
                RotatePlayer(0, verticalInput);
            }
        }

        bool isMoving = movement.sqrMagnitude > 0;
        animator.SetBool("isMoving", isMoving);
    }

    void FixedUpdate()
    {
        // Apply movement to the player in FixedUpdate for physics consistency
        rb.velocity = (movement * PlayerManager.playerConfig.moveSpeed) + knockbackVelocity;
    }

    void RotatePlayer(float x, float y)
    {
        // If there is no input, do not rotate the player
        if (x == 0 && y == 0) return;

        // Calculate the rotation angle based on input direction
        float angle = Mathf.Atan2(y, x) * Mathf.Rad2Deg;
        // Apply the rotation to the player
        transform.rotation = Quaternion.Euler(0, 0, angle);
    }

    public IEnumerator KnockbackCoroutine(Vector2 direction, float force, float duration)
    {
        // Apply knockback in the specified direction with given force
        knockbackVelocity = direction.normalized * force;
        yield return new WaitForSeconds(duration);
        // Reset knockback velocity after the duration
        knockbackVelocity = Vector2.zero;
    }
}
