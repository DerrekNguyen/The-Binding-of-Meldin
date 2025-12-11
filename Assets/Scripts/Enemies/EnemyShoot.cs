using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Handles enemy shooting

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

        enemyLifecycle = GetComponent<EnemyLifecycle>();
        if (enemyLifecycle == null)
        {
            enemyLifecycle = GetComponentInChildren<EnemyLifecycle>();
        }

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
        if (enemyLifecycle != null && enemyLifecycle.IsDead)
        {
            if (shootCoroutine != null)
            {
                StopCoroutine(shootCoroutine);
                shootCoroutine = null;
            }
            return;
        }

        if (canShoot && shootCoroutine == null)
        {
            shootCoroutine = StartCoroutine(ShootRoutine());
        }
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
            if (enemyLifecycle != null && enemyLifecycle.IsDead)
            {
                break;
            }

            float waitTime = Random.Range(minShootInterval, maxShootInterval);
            yield return new WaitForSeconds(waitTime);
            
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
        
        Vector2 direction = (player.transform.position - transform.position).normalized;
        
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        
        GameObject projectile = Instantiate(projectilePrefab, transform.position, Quaternion.Euler(0, 0, angle));
        projectile.SetActive(true);

        int bulletDamage = enemyConfig != null
            ? enemyConfig.GetScaledDamage()
            : 1 + PlayerPrefs.GetInt("RunCount", 0) * 2;

        switch (projectileType)
        {
            case ProjConfig.ProjectileType.Follow:
                FollowProj followScript = projectile.AddComponent<FollowProj>();
                followScript.Initialize(player, projectileSpeed, bulletDamage, projectileLifetime, projConfig.bulletHitPlayerSoundName);
                if (SoundManager.Instance != null) SoundManager.Instance.PlaySound2D(projConfig.bulletShotSoundName);
                break;
            case ProjConfig.ProjectileType.Straight:
                StraightProj straightScript = projectile.AddComponent<StraightProj>();
                straightScript.Initialize(player, projectileSpeed, bulletDamage, projectileLifetime, projConfig.bulletHitPlayerSoundName);
                if (SoundManager.Instance != null) SoundManager.Instance.PlaySound2D(projConfig.bulletShotSoundName);
                break;
        }
    }
}
