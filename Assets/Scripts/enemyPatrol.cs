using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPatrol : MonoBehaviour
{
    public Transform[] patrolPoints; // Array of patrol points
    public float speed = 2f; // Movement speed of the enemy

    private int currentPointIndex = 0;
    private bool isFacingRight = true; // Track the current facing direction

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

            // Flip the sprite when the enemy changes direction
            Flip();
        }
    }

    // Flips the enemy's sprite by changing the localScale
    void Flip()
    {
        // Toggle the facing direction
        isFacingRight = !isFacingRight;

        // Flip the x-axis of the local scale to mirror the sprite
        Vector3 localScale = transform.localScale;
        localScale.x *= -1;
        transform.localScale = localScale;
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