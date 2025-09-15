using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehavior : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // Destroy the enemy
            Destroy(gameObject);

            // TODO: Reduce player health and check for game over (down the line)
        }
        else if (other.CompareTag("Bullet"))
        {
            // Destroy the enemy
            Destroy(gameObject);
        }
    }
}
