using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class Shoot : MonoBehaviour
{
    [Header("Shooting Settings")]
    [SerializeField] private GameObject arrowPrefab;
    [SerializeField] private Transform firePoint;
    [SerializeField] private float arrowSpeed = 20f;
    
    [Header("Animation Parameters")]
    [SerializeField] private RuntimeAnimatorController animatorController;
    [SerializeField] private string drawStateName = "Draw";
    [SerializeField] private string idleStateName = "Idle";
    
    private InputManager _input;
    private Animator _animator;
    private Camera _mainCamera;
    private bool _canShoot = true;
    private bool _isShooting = false;

    void Start()
    {
        _input = InputManager.Instance;
        _animator = GetComponent<Animator>();
        _mainCamera = Camera.main;
        
        if (_mainCamera == null)
        {
            _mainCamera = FindObjectOfType<Camera>();
        }
    }

    void Update()
    {
        if (_input == null || !_canShoot || _isShooting) return;

        if (_input.ShootPressed)
        {
            StartCoroutine(ShootRoutine());
        }
    }

    private IEnumerator ShootRoutine()
    {
        _isShooting = true;
        _canShoot = false;

        // Play shooting animation
        if (_animator != null && !string.IsNullOrEmpty(drawStateName))
        {
            _animator.Play(drawStateName);
            
            // Wait one frame for animation to start
            yield return null;
            
            // Get actual animation length from animator state
            AnimatorStateInfo stateInfo = _animator.GetCurrentAnimatorStateInfo(0);
            float animationLength = stateInfo.length;
            
            // Wait for 70% of animation before spawning arrow
            yield return new WaitForSeconds(animationLength * 0.7f);
            
            // Spawn and shoot arrow
            SpawnArrow();
            
            // Wait for remaining 30% of animation
            yield return new WaitForSeconds(animationLength * 0.3f);
            
            // Return to idle state
            if (!string.IsNullOrEmpty(idleStateName))
            {
                _animator.Play(idleStateName);
            }
        }
        else
        {
            // If no animation, just shoot immediately
            SpawnArrow();
        }

        _isShooting = false;
        _canShoot = true;
    }

    private void SpawnArrow()
    {
        if (arrowPrefab == null)
        {
            Debug.LogError("Shoot: Arrow prefab not assigned!");
            return;
        }

        Transform spawnPoint = firePoint != null ? firePoint : transform;
        
        // Get mouse world position
        Vector3 mouseScreenPos = _input.MousePosition;
        mouseScreenPos.z = Mathf.Abs(_mainCamera.transform.position.z);
        Vector2 mouseWorldPos = _mainCamera.ScreenToWorldPoint(mouseScreenPos);
        
        // Calculate direction from spawn point to mouse
        Vector2 direction = (mouseWorldPos - (Vector2)spawnPoint.position).normalized;
        
        // Calculate rotation to face mouse
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        
        // Instantiate arrow at fire point
        GameObject arrow = Instantiate(arrowPrefab, spawnPoint.position, Quaternion.Euler(0, 0, angle));
        
        // Deparent arrow so it's independent
        arrow.transform.SetParent(null);
        
        // Activate arrow in case prefab is inactive
        arrow.SetActive(true);
        
        // Initialize arrow movement
        ArrowMovement movement = arrow.GetComponent<ArrowMovement>();
        if (movement == null)
        {
            movement = arrow.AddComponent<ArrowMovement>();
        }
        movement.Initialize(direction, arrowSpeed);
    }
}
