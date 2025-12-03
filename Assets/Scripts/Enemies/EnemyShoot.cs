using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShoot : MonoBehaviour
{
    [Header("Shooting Settings")]
    [SerializeField] private ProjConfig projConfig;
    [SerializeField] private EnemyConfig enemyConfig;
    
    public bool canShoot = false;
    
    private GameObject player;
    private PlayerLifecycle playerLifecycle;
    private Coroutine shootCoroutine;
    
    private GameObject projectilePrefab;
    private float projectileSpeed;
    private ProjConfig.ProjectileType projectileType;
    private float minShootInterval;
    private float maxShootInterval;
    private float projectileLifetime;
    private EnemyLifecycle enemyLifecycle;

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

        // Cache enemy lifecycle from this object or children
        enemyLifecycle = GetComponent<EnemyLifecycle>();
        if (enemyLifecycle == null)
        {
            enemyLifecycle = GetComponentInChildren<EnemyLifecycle>();
        }

        // Pull all settings from ProjConfig
        if (projConfig != null)
        {
            projectilePrefab = projConfig.projectilePrefab;
            projectileSpeed = projConfig.projSpeed;
            projectileType = projConfig.projectileType;
            minShootInterval = projConfig.min;
            maxShootInterval = projConfig.max;
            projectileLifetime = projConfig.lifetime;
        }
    }

    void Update()
    {
        // If enemy is dead, ensure shooting stops
        if (enemyLifecycle != null && enemyLifecycle.IsDead)
        {
            if (shootCoroutine != null)
            {
                StopCoroutine(shootCoroutine);
                shootCoroutine = null;
            }
            return;
        }

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
            // If enemy died during wait, stop
            if (enemyLifecycle != null && enemyLifecycle.IsDead)
            {
                break;
            }

            // Wait random interval
            float waitTime = Random.Range(minShootInterval, maxShootInterval);
            yield return new WaitForSeconds(waitTime);
            
            // Check if still allowed to shoot, game isn't paused, player exists and isn't dead, and enemy isn't dead
            if (canShoot 
                && (enemyLifecycle == null || !enemyLifecycle.IsDead)
                && !InGameUiManager.isPaused 
                && player != null 
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

        // Get bullet damage from config (fallback to simple scaling if missing)
        int bulletDamage = enemyConfig != null
            ? enemyConfig.GetScaledDamage()
            : 1 + PlayerPrefs.GetInt("RunCount", 0) * 2;

        // Add the appropriate projectile script based on type
        switch (projectileType)
        {
            case ProjConfig.ProjectileType.Follow:
                FollowProj followScript = projectile.AddComponent<FollowProj>();
                followScript.Initialize(player, projectileSpeed, bulletDamage, projectileLifetime);
                break;
            case ProjConfig.ProjectileType.Straight:
                StraightProj straightScript = projectile.AddComponent<StraightProj>();
                straightScript.Initialize(player, projectileSpeed, bulletDamage, projectileLifetime);
                break;
        }
    }
}
