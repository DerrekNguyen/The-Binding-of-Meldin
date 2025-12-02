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

    void Start()
    {
        // Ensure barrier starts disabled
        if (roomBarrier != null)
        {
            roomBarrier.SetActive(false);
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
        if (other.CompareTag("Player") && !roomActive)
        {
            OnPlayerEnterRoom();
        }
    }

    private void OnPlayerEnterRoom()
    {
        roomActive = true;
        
        // Enable room barrier immediately
        if (roomBarrier != null)
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
        // Enable enemy shooting
        if (enemy.TryGetComponent<EnemyShoot>(out EnemyShoot shootScript))
        {
            shootScript.canShoot = true;
        }

        // Enable enemy movement
        if (enemy.TryGetComponent<EnemyMovement>(out EnemyMovement moveScript))
        {
            moveScript.canMove = true;
        }
    }

    private bool AreAllEnemiesDead()
    {
        foreach (GameObject enemy in enemies)
        {
            // If any enemy still exists, room is not cleared
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

        if (roomBarrier != null)
        {
            roomBarrier.SetActive(false);
        }
    }

    // Public method to check room status
    public bool IsRoomCleared()
    {
        return roomCleared;
    }
}
