using UnityEngine;
using UnityEngine.InputSystem;

// Gets inputs

public class InputManager : MonoBehaviour
{
    public static InputManager Instance { get; private set; }

    private Controls _controls;

    public Vector2 Move { get; private set; }
    public Vector2 MousePosition { get; private set; }

    public bool SprintPressed { get; private set; }
    public bool DodgePressed { get; private set; }
    public bool PausePressed { get; private set; }
    public bool ShootPressed { get; private set; }
    public bool RevivePressed { get; private set; }
    public bool InteractPressed { get; private set; }

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
        bool isGamePaused = InGameUiManager.isPaused;

        // Mouse position (always available)
        MousePosition = Mouse.current != null ? Mouse.current.position.ReadValue() : Vector2.zero;

        PausePressed = _controls.Player.Pause.WasPressedThisFrame();
        PauseHeld = _controls.Player.Pause.IsPressed();

        if (isGamePaused)
        {
            Move = Vector2.zero;
            SprintPressed = false;
            DodgePressed = false;
            ShootPressed = false;
            InteractPressed = false;
            RevivePressed = false;
            SprintHeld = false;
            DodgeHeld = false;
            ShootHeld = false;
            InteractHeld = false;
            ReviveHeld = false;
            return;
        }

        PlayerLifecycle playerLifecycle = FindObjectOfType<PlayerLifecycle>();
        bool isPlayerDead = playerLifecycle != null && playerLifecycle.IsDead;

        RevivePressed = _controls.Player.Revive.WasPressedThisFrame();
        ReviveHeld = _controls.Player.Revive.IsPressed();

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

        Move = _controls.Player.Move.ReadValue<Vector2>();

        SprintPressed = _controls.Player.Sprint.WasPressedThisFrame();
        DodgePressed  = _controls.Player.Dodge.WasPressedThisFrame();
        ShootPressed  = _controls.Player.Shoot.WasPressedThisFrame();
        InteractPressed = _controls.Player.Interact.WasPressedThisFrame();

        SprintHeld = _controls.Player.Sprint.IsPressed();
        DodgeHeld  = _controls.Player.Dodge.IsPressed();
        ShootHeld  = _controls.Player.Shoot.IsPressed();
        InteractHeld = _controls.Player.Interact.IsPressed();
    }
}