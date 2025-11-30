using UnityEngine;

public class BulletMovement : MonoBehaviour
{
    private Vector2 _direction;
    private float _speed;

    public void Initialize(Vector2 direction, float speed)
    {
        _direction = direction.normalized;
        _speed = speed;
    }

    void Update()
    {
        // Calculate movement distance this frame
        float moveDistance = _speed * Time.deltaTime;
        
        // Raycast to check for walls before moving
        RaycastHit2D hit = Physics2D.Raycast(transform.position, _direction, moveDistance);
        
        if (hit.collider != null && hit.collider.CompareTag("Collision"))
        {
            Destroy(gameObject);
            return;
        }

        // Move arrow towards mouse direction
        transform.position += (Vector3)_direction * moveDistance;
    }
}
