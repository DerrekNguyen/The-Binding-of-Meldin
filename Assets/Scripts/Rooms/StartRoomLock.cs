using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class StartRoomLock : MonoBehaviour
{
    [Header("Room Barriers")]
    [SerializeField] private GameObject roomBarrier;

    // Start is called before the first frame update
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
        // First half: wait 3 seconds
        yield return new WaitForSeconds(3f);

        if (SoundManager.Instance != null) SoundManager.Instance.PlaySound2D("countdown");

        yield return new WaitForSeconds(3f);

        // Unlock (disable barrier)
        if (roomBarrier != null)
        {
            roomBarrier.SetActive(false);
        }
    }
}
