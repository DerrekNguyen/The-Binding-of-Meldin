using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShoot : MonoBehaviour
{
    public enum ProjectileType
    {
        Slime,
        Skele
    }

    [Header("Shooting Settings")]
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private float projectileSpeed = 5f;
    [SerializeField] private ProjectileType projectileType = ProjectileType.Slime;
    
    [Header("Timing")]
    [SerializeField] private float minShootInterval = 1f;
    [SerializeField] private float maxShootInterval = 3f;
    
    public bool canShoot = false;
    
    private GameObject player;
    private PlayerLifecycle playerLifecycle;
    private Coroutine shootCoroutine;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        
        if (player != null)
        {
            playerLifecycle = player.GetComponent<PlayerLifecycle>();
            if (playerLifecycle == null)
            {
                playerLifecycle = player.GetComponentInChildren<PlayerLifecycle>();
            }
            if (playerLifecycle == null)
            {
                playerLifecycle = FindObjectOfType<PlayerLifecycle>();
            }
        }
    }

    void Update()
    {
        // Start shooting coroutine if canShoot is true and not already running
        if (canShoot && shootCoroutine == null)
        {
            shootCoroutine = StartCoroutine(ShootRoutine());
        }
        // Stop shooting if canShoot becomes false
        else if (!canShoot && shootCoroutine != null)
        {
            StopCoroutine(shootCoroutine);
            shootCoroutine = null;
        }
    }

    private IEnumerator ShootRoutine()
    {
        while (canShoot)
        {
            // Wait random interval
            float waitTime = Random.Range(minShootInterval, maxShootInterval);
            yield return new WaitForSeconds(waitTime);
            
            // Check if still allowed to shoot, game isn't paused, player exists and isn't dead
            if (canShoot && !InGameUiManager.isPaused && player != null 
                && (playerLifecycle == null || !playerLifecycle.IsDead))
            {
                ShootProjectile();
            }
        }
        
        shootCoroutine = null;
    }

    private void ShootProjectile()
    {
        if (projectilePrefab == null)
        {
            Debug.LogWarning("EnemyShoot: Projectile prefab not assigned!");
            return;
        }
        
        // Calculate direction to player
        Vector2 direction = (player.transform.position - transform.position).normalized;
        
        // Calculate rotation to face player
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        
        // Spawn projectile at enemy position
        GameObject projectile = Instantiate(projectilePrefab, transform.position, Quaternion.Euler(0, 0, angle));
        projectile.SetActive(true);
        
        // Add the appropriate projectile script based on type
        switch (projectileType)
        {
            case ProjectileType.Slime:
                SlimeProj slimeScript = projectile.AddComponent<SlimeProj>();
                slimeScript.Initialize(player, projectileSpeed);
                break;
            case ProjectileType.Skele:
                SkeleProj skeleScript = projectile.AddComponent<SkeleProj>();
                skeleScript.Initialize(player, projectileSpeed);
                break;
        }
    }
}
