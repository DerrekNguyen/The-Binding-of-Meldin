using UnityEngine;

public class ArrowMovement : MonoBehaviour
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
        // Raycast to check for walls before moving
        RaycastHit2D hit = Physics2D.Raycast(transform.position, _direction, _speed * Time.deltaTime);
        
        if (hit.collider != null && hit.collider.CompareTag("Wall"))
        {
            Destroy(gameObject);
            return;
        }

        // Move arrow in straight line
        transform.position += _speed * Time.deltaTime * (Vector3)_direction;
    }
}
