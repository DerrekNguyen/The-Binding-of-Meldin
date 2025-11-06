using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BulletBehavior : MonoBehaviour
{
    public BulletConfig config;

    private Rigidbody2D rb;
    private BoxCollider2D bc;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void Init(BulletConfig config) {
        this.config = config;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    // Use FixedUpdate for physics updates
    void FixedUpdate()
    {
        // Move the bullet forward based on its speed
        rb.velocity = transform.right * config.speed;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Wall"))
        {
            // Destroy the bullet if it hits a wall
            Destroy(this.gameObject);
        } else if (collision.CompareTag("Enemy"))
        {
            // Destroy the bullet if it hits an enemy
            Destroy(this.gameObject);
        }
    }
    
}
