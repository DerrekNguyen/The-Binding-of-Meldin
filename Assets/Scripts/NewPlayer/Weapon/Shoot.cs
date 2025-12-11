using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

// Handles player shooting

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

    void LateUpdate()
    {
        if (_input == null || !_canShoot || _isShooting) return;

        if (!InGameUiManager.isPaused && !IsPointerOverUI() && _input.ShootPressed)
        {
            StartCoroutine(ShootRoutine());
        }
    }

    private bool IsPointerOverUI()
    {
        return EventSystem.current != null && EventSystem.current.IsPointerOverGameObject();
    }

    private IEnumerator ShootRoutine()
    {
        _isShooting = true;
        _canShoot = false;

        if (_animator != null && !string.IsNullOrEmpty(drawStateName))
        {
            _animator.Play(drawStateName);
            
            yield return null;
            
            AnimatorStateInfo stateInfo = _animator.GetCurrentAnimatorStateInfo(0);
            float animationLength = stateInfo.length;
            
            yield return new WaitForSeconds(animationLength * 0.1f);
            
            SpawnArrow();
            
            yield return new WaitForSeconds(animationLength * 0.5f);
            
            if (!string.IsNullOrEmpty(idleStateName))
            {
                _animator.Play(idleStateName);
            }
        }
        else
        {
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

        if (SoundManager.Instance != null) SoundManager.Instance.PlaySound2D("playerShoot");

        Transform spawnPoint = firePoint != null ? firePoint : transform;
        
        Vector3 mouseScreenPos = _input.MousePosition;
        mouseScreenPos.z = Mathf.Abs(_mainCamera.transform.position.z);
        Vector2 mouseWorldPos = _mainCamera.ScreenToWorldPoint(mouseScreenPos);
        
        Vector2 direction = (mouseWorldPos - (Vector2)spawnPoint.position).normalized;
        
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        
        GameObject arrow = Instantiate(arrowPrefab, spawnPoint.position, Quaternion.Euler(0, 0, angle));
        arrow.SetActive(true);
        
        Rigidbody2D rb = arrow.GetComponent<Rigidbody2D>();
        if (rb == null)
        {
            rb = arrow.AddComponent<Rigidbody2D>();
            rb.isKinematic = true;
            rb.gravityScale = 0f;
        }
        
        Collider2D col = arrow.GetComponent<Collider2D>();
        if (col == null)
        {
            CircleCollider2D circleCol = arrow.AddComponent<CircleCollider2D>();
            circleCol.radius = 0.1f;
            circleCol.isTrigger = true;
        }
        
        arrow.tag = "Bullet";
        
        BulletMovement arrowScript = arrow.AddComponent<BulletMovement>();
        arrowScript.Initialize(direction, arrowSpeed);
    }
}
