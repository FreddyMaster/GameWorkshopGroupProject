using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPatrol : MonoBehaviour
{
    public Transform pointA; // The first patrol point
    public Transform pointB; // The second patrol point
    public float speed = 2f; // Movement speed of the enemy

    private Vector3 targetPosition;

    void Start()
    {
        // Set initial target position to pointA
        targetPosition = pointA.position;
    }

    void Update()
    {
        // Move the enemy towards the target position
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);

        // Check if the enemy has reached the target position
        if (Vector3.Distance(transform.position, targetPosition) < 0.1f)
        {
            // Switch to the other target position
            targetPosition = targetPosition == pointA.position ? pointB.position : pointA.position;
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
