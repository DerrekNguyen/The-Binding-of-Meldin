using UnityEngine;

// Handles calling MoveToRoom on room trigger enter

public class RoomPlaySpace : MonoBehaviour
{
    public Transform roomCenter;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player entered room: " + gameObject.name);
            Camera.main.GetComponent<CameraController>().MoveToRoom(transform);
        }
    }
}
