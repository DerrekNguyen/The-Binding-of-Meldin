using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    public static InputManager Instance { get; private set; }

    private Controls _controls;

    // Cached values
    public Vector2 Move { get; private set; }
    public Vector2 MousePosition { get; private set; }

    // Button (down this frame) flags
    public bool SprintPressed { get; private set; }
    public bool DodgePressed { get; private set; }
    public bool PausePressed { get; private set; }
    public bool ShootPressed { get; private set; }
    public bool RevivePressed { get; private set; }
    public bool InteractPressed { get; private set; }

    // Button (held) flags
    public bool SprintHeld { get; private set; }
    public bool DodgeHeld { get; private set; }
    public bool PauseHeld { get; private set; }
    public bool ShootHeld { get; private set; }
    public bool ReviveHeld { get; private set; }
    public bool InteractHeld { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;

        _controls = new Controls();
        _controls.Player.Enable();
    }

    private void OnDestroy()
    {
        _controls?.Dispose();
    }

    private void Update()
    {
        // Check if player is dead
        PlayerLifecycle playerLifecycle = FindObjectOfType<PlayerLifecycle>();
        bool isPlayerDead = playerLifecycle != null && playerLifecycle.IsDead;

        // Mouse position (always available)
        MousePosition = Mouse.current != null ? Mouse.current.position.ReadValue() : Vector2.zero;

        // Pause (always available)
        PausePressed = _controls.Player.Pause.WasPressedThisFrame();
        PauseHeld = _controls.Player.Pause.IsPressed();

        // Revive (always available when dead)
        RevivePressed = _controls.Player.Revive.WasPressedThisFrame();
        ReviveHeld = _controls.Player.Revive.IsPressed();

        // If player is dead, don't process other inputs
        if (isPlayerDead)
        {
            Move = Vector2.zero;
            SprintPressed = false;
            DodgePressed = false;
            ShootPressed = false;
            InteractPressed = false;
            SprintHeld = false;
            DodgeHeld = false;
            ShootHeld = false;
            InteractHeld = false;
            return;
        }

        // Movement vector
        Move = _controls.Player.Move.ReadValue<Vector2>();

        // Pressed (this frame)
        SprintPressed = _controls.Player.Sprint.WasPressedThisFrame();
        DodgePressed  = _controls.Player.Dodge.WasPressedThisFrame();
        ShootPressed  = _controls.Player.Shoot.WasPressedThisFrame();
        InteractPressed = _controls.Player.Interact.WasPressedThisFrame();

        // Held
        SprintHeld = _controls.Player.Sprint.IsPressed();
        DodgeHeld  = _controls.Player.Dodge.IsPressed();
        ShootHeld  = _controls.Player.Shoot.IsPressed();
        InteractHeld = _controls.Player.Interact.IsPressed();
    }
}