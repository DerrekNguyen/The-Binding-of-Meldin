using UnityEngine;

// Handles player bullet movement
public class BulletMovement : MonoBehaviour
{
    private Vector2 _direction;
    private float _speed;
    private float lifetime = 3f;
    private float elapsedTime = 0f;

    public void Initialize(Vector2 direction, float speed)
    {
        _direction = direction.normalized;
        _speed = speed;
    }

    void Update()
    {
        if (InGameUiManager.isPaused) return;
        
        elapsedTime += Time.deltaTime;
        if (elapsedTime >= lifetime)
        {
            Destroy(gameObject);
            return;
        }
        
        float moveDistance = _speed * Time.deltaTime;
        
        RaycastHit2D hit = Physics2D.Raycast(transform.position, _direction, moveDistance);
        
        if (hit.collider != null && hit.collider.CompareTag("Collision"))
        {
            Destroy(gameObject);
            return;
        }

        transform.position += (Vector3)_direction * moveDistance;
    }
}
