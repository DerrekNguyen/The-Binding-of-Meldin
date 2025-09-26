using UnityEngine;

public class CameraController : MonoBehaviour
{
    private Transform targetRoom;   // Current room the camera should lock to
    public float moveSpeed = 5f;    // Smooth speed when snapping

    void Update()
    {
        if (targetRoom != null)
        {
            // Smoothly move camera to the target room’s center
            Vector3 targetPos = new Vector3(targetRoom.position.x, targetRoom.position.y, transform.position.z);
            transform.position = Vector3.Lerp(transform.position, targetPos, Time.deltaTime * moveSpeed);
        }
    }

    public void MoveToRoom(Transform newRoom)
    {
        targetRoom = newRoom;
    }
}

