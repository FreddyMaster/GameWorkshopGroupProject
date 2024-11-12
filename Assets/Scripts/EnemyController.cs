using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public Transform[] patrolPoints; // Array of patrol points
    public GameObject projectilePrefab;
    public float speed = 2f; // Movement speed of the enemy
    public Transform firePoint;
    private float attackRange = 10f;
    public float projectileSpeed = 150.0f;
    private int currentPointIndex = 0;
    private bool isFacingRight = true; // Track the current facing direction
    private float lastShotTime = 0f;
    public float fireRate = 1f;

    Transform target;
    private bool isPlayerInRange = false;

    void Start()
    {
        // Start by moving towards the first patrol point if any are set
        if (patrolPoints.Length > 0)
        {
            transform.position = patrolPoints[0].position;
            currentPointIndex = 1;
        }
        target = PlayerController.instance.player.transform;
    }

    void Update()
    {
        // Ensure there are patrol points set
        if (patrolPoints.Length == 0) return;

        // Calculate the distance to the player
        float distance = Vector3.Distance(target.position, transform.position);

        // Check if player is in range
        if (distance <= attackRange)
        {
            isPlayerInRange = true;
            StopAndFacePlayer();
            AttackTarget();
        }
        else
        {
            isPlayerInRange = false;
            Patrol();
        }
    }

    // Handles the patrolling movement
    void Patrol()
    {
        // If the player is in range, stop patrolling
        if (isPlayerInRange) return;

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

    // Faces the enemy towards the player
    void StopAndFacePlayer()
    {
        // Calculate the direction to the player
        Vector3 directionToPlayer = target.position - transform.position;

        // Flip the enemy to face the player based on their relative position
        if (directionToPlayer.x > 0 && !isFacingRight)
        {
            Flip();
        }
        else if (directionToPlayer.x < 0 && isFacingRight)
        {
            Flip();
        }
    }

    // Handles attacking the target by shooting projectiles
    void AttackTarget()
    {
        // Check if enough time has passed since the last shot
        if (Time.time >= lastShotTime + 1f / fireRate)
        {
            ShootProjectile();
            lastShotTime = Time.time; // Update the time of the last shot
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

private void ShootProjectile()
{
    // Determine shoot direction based on facing direction
    Vector2 shootDirection = isFacingRight ? Vector2.right : Vector2.left;

    // Calculate the spawn position with an offset based on the shoot direction
    Vector3 spawnPosition = transform.position + (Vector3)shootDirection * 0.5f;

    // Instantiate the projectile at the offset position
    GameObject projectile = Instantiate(projectilePrefab, spawnPosition, Quaternion.identity);

    // Rotate the projectile to face the correct direction
    float angle = isFacingRight ? 0f : 180f;
    projectile.transform.rotation = Quaternion.Euler(0, angle, 0);

    // Set projectile velocity based on the shoot direction
    Rigidbody2D projectileRb = projectile.GetComponent<Rigidbody2D>();
    projectileRb.velocity = shootDirection * projectileSpeed;
}

}
