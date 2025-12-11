using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

// Handles locking start room at start of game

public class StartRoomLock : MonoBehaviour
{
    [Header("Room Barriers")]
    [SerializeField] private GameObject roomBarrier;

    void Start()
    {
        if (roomBarrier != null)
        {
            roomBarrier.SetActive(true);
        }
        StartCoroutine(UnlockAfterDelay());
    }

    private IEnumerator UnlockAfterDelay()
    {
        yield return new WaitForSeconds(3f);

        if (SoundManager.Instance != null) SoundManager.Instance.PlaySound2D("countdown");

        yield return new WaitForSeconds(3f);

        if (roomBarrier != null)
        {
            roomBarrier.SetActive(false);
        }
    }
}
