using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPatrol : MonoBehaviour
{
    public Transform[] patrolPoints; // Array of patrol points
    public float speed = 2f; // Movement speed of the enemy

    private int currentPointIndex = 0;

    void Start()
    {
        // Start by moving towards the first patrol point if any are set
        if (patrolPoints.Length > 0)
        {
            transform.position = patrolPoints[0].position;
            currentPointIndex = 1;
        }
    }

    void Update()
    {
        // Ensure there are patrol points set
        if (patrolPoints.Length == 0) return;

        // Move the enemy towards the current target patrol point
        transform.position = Vector3.MoveTowards(transform.position, patrolPoints[currentPointIndex].position, speed * Time.deltaTime);

        // Check if the enemy has reached the current target patrol point
        if (Vector3.Distance(transform.position, patrolPoints[currentPointIndex].position) < 0.1f)
        {
            // Move to the next patrol point
            currentPointIndex = (currentPointIndex + 1) % patrolPoints.Length;
        }
    }

    // Detect collision with projectiles
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Projectile"))
        {
            // Destroy the enemy
            Destroy(gameObject);

            // Optionally destroy the projectile as well
            Destroy(collision.gameObject);
        }
    }
}