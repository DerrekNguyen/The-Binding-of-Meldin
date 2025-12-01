using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIChasing : MonoBehaviour
{
    public GameObject player;
    public float speed;
    public bool isMoving;
    public Vector2 LastFacingDirection = Vector2.right;

    private float distance;
    
    private void ChasePlayer()
    {
        Vector2 direction = player.transform.position - transform.position;
        direction.Normalize();
        LastFacingDirection = direction.x < 0 ? Vector2.left : Vector2.right;
        transform.position = Vector2.MoveTowards(this.transform.position, player.transform.position, speed * Time.deltaTime);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        distance = Vector2.Distance(transform.position, player.transform.position);
        isMoving = distance > 0.05f;
        if (isMoving)
        {
            ChasePlayer();
        }
    }
}
