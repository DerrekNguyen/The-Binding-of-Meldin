using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class Shoot : MonoBehaviour
{
    [Header("Shooting Settings")]
    [SerializeField] private GameObject arrowPrefab;
    [SerializeField] private Transform firePoint;
    [SerializeField] private float arrowSpeed = 20f;
    [SerializeField] private float spawnOffset = 5f;
    
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
            
            // Wait for 10% of animation before spawning arrow
            yield return new WaitForSeconds(animationLength * 0.1f);
            
            // Spawn and shoot arrow
            SpawnArrow();
            
            // Wait for another 50% of animation
            yield return new WaitForSeconds(animationLength * 0.5f);
            
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
        
        // Spawn arrow
        GameObject arrow = Instantiate(arrowPrefab, spawnPoint.position, Quaternion.Euler(0, 0, angle));
        arrow.SetActive(true);
        
        // Add Rigidbody2D if not present
        Rigidbody2D rb = arrow.GetComponent<Rigidbody2D>();
        if (rb == null)
        {
            rb = arrow.AddComponent<Rigidbody2D>();
            rb.isKinematic = true;
            rb.gravityScale = 0f;
        }
        
        // Add Collider2D if not present
        Collider2D col = arrow.GetComponent<Collider2D>();
        if (col == null)
        {
            CircleCollider2D circleCol = arrow.AddComponent<CircleCollider2D>();
            circleCol.radius = 0.1f;
            circleCol.isTrigger = true;
        }
        
        // Make sure bullet has the Bullet tag
        arrow.tag = "Bullet";
        
        // Add movement script
        BulletMovement arrowScript = arrow.AddComponent<BulletMovement>();
        arrowScript.Initialize(direction, arrowSpeed);
    }
}
