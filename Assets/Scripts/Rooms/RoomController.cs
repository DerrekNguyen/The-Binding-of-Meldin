using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomController : MonoBehaviour
{
    [Header("Enemy Setup")]
    [SerializeField] private List<GameObject> enemies = new();
    
    [Header("Room Barriers")]
    [SerializeField] private GameObject roomBarrier;

    [Header("Timing")]
    [SerializeField, Min(0f)] private float activationDelay = 1f;

    private bool roomActive = false;
    private bool roomCleared = false;
    private bool replacedWithBossController = false;

    void Start()
    {
        if (roomBarrier == gameObject)
        {
            roomBarrier = null;
        }

        if (roomBarrier != null)
        {
            roomBarrier.SetActive(false);
        }
        
        foreach (GameObject enemy in enemies)
        {
            if (enemy != null)
            {
                DeactivateEnemy(enemy);
            }
        }
    }

    void Update()
    {
        // Check if all enemies are dead
        if (roomActive && !roomCleared && AreAllEnemiesDead())
        {
            OnRoomCleared();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Boss detection and immediate replacement
        if (!replacedWithBossController && other.CompareTag("Boss"))
        {
            ReplaceWithBossController(other.gameObject);
            return;
        }

        if (other.CompareTag("Player") && !roomActive)
        {
            OnPlayerEnterRoom();
        }
    }

    private void ReplaceWithBossController(GameObject boss)
    {
        if (replacedWithBossController) return;
        replacedWithBossController = true;

        // Delete the detected boss
        if (boss != null)
        {
            Destroy(boss);
        }

        // Add BossRoomController to the same GameObject
        if (gameObject.GetComponent<BossRoomController>() == null)
        {
            gameObject.AddComponent<BossRoomController>();
        }

        // Delete all enemies in the list
        foreach (GameObject enemy in enemies)
        {
            if (enemy != null)
            {
                Destroy(enemy);
            }
        }

        // Remove this RoomController component
        Destroy(this);
    }

    private void OnPlayerEnterRoom()
    {
        roomActive = true;
        
        // Enable room barrier immediately
        if (roomBarrier != null && roomBarrier != gameObject)
        {
            roomBarrier.SetActive(true);
        }
        
        // Activate all enemies after a short delay
        StartCoroutine(ActivateEnemiesWithDelay());
    }

    private IEnumerator ActivateEnemiesWithDelay()
    {
        yield return new WaitForSeconds(activationDelay);

        foreach (GameObject enemy in enemies)
        {
            if (enemy != null)
            {
                ActivateEnemy(enemy);
            }
        }
    }

    private void ActivateEnemy(GameObject enemy)
    {
        if (enemy.TryGetComponent<EnemyShoot>(out EnemyShoot shootScript))
        {
            shootScript.canShoot = true;
        }

        if (enemy.TryGetComponent<EnemyMovement>(out EnemyMovement moveScript))
        {
            moveScript.canMove = true;
        }
    }

    private void DeactivateEnemy(GameObject enemy)
    {
        if (enemy.TryGetComponent<EnemyShoot>(out EnemyShoot shootScript))
        {
            shootScript.canShoot = false;
        }

        if (enemy.TryGetComponent<EnemyMovement>(out EnemyMovement moveScript))
        {
            moveScript.canMove = false;
        }
    }

    private bool AreAllEnemiesDead()
    {
        foreach (GameObject enemy in enemies)
        {
            if (enemy != null)
            {
                return false;
            }
        }
        
        return true;
    }

    private void OnRoomCleared()
    {
        roomCleared = true;
        Debug.Log("Room Cleared!");
        StartCoroutine(UnlockBarrierWithDelay());
    }

    private IEnumerator UnlockBarrierWithDelay()
    {
        yield return new WaitForSeconds(activationDelay);

        if (roomBarrier != null && roomBarrier != gameObject)
        {
            roomBarrier.SetActive(false);
        }
    }

    public bool IsRoomCleared()
    {
        return roomCleared;
    }
}
