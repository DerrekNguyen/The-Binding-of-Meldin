using UnityEngine;

// Handles moving camera to new room

public class CameraController : MonoBehaviour
{
    private Transform targetRoom;
    public float moveSpeed = 5f;

    void Update()
    {
        if (targetRoom != null)
        {
            Vector3 targetPos = new Vector3(targetRoom.position.x, targetRoom.position.y, transform.position.z);
            transform.position = Vector3.Lerp(transform.position, targetPos, Time.deltaTime * moveSpeed);
        }
    }

    public void MoveToRoom(Transform newRoom)
    {
        targetRoom = newRoom;
    }
}

