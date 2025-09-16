using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomPlaySpace : MonoBehaviour
{
    public Transform roomCenter;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            CameraController.instance.MoveToRoom(roomCenter.position);
        }
    }
}
