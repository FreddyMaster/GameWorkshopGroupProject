using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(BoxCollider2D))]

public class PlayerController : MonoBehaviour
{
    public float horizontalInput;
    public float maxSpeed = 5.0f;
    public float jumpHeight = 6.5f;
    public float gravityScale = 1.0f;
    public GameObject projectilePrefab;
    public float projectileSpeed = 150.0f;
    private bool isGrounded = false;
    private Rigidbody2D r2d;
    private BoxCollider2D mainCollider;
    private Transform t;
    private Vector2 shootDirection = Vector2.right;

    // Player identification
    public int playerNumber = 1; // 1 for Player 1, 2 for Player 2

    // Boundary
    public float leftBoundary = 0f; // Set this to the desired x position

    void Start()
    {
        t = transform;
        r2d = GetComponent<Rigidbody2D>();
        mainCollider = GetComponent<BoxCollider2D>();

        // Make sure to add a PhysicsMaterial2D with zero friction
        PhysicsMaterial2D noFrictionMaterial = new PhysicsMaterial2D { friction = 0f, bounciness = 0f };
        mainCollider.sharedMaterial = noFrictionMaterial;

        r2d.freezeRotation = true;
        r2d.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
        r2d.gravityScale = gravityScale;
    }

    void Update()
    {
        horizontalInput = 0;

        // Movement controls for Player 1 (WASD) and Player 2 (Arrow Keys)
        if (playerNumber == 1)
        {
            if (Input.GetKey(KeyCode.A))
            {
                horizontalInput = -1;
                shootDirection = Vector2.left;
            }
            else if (Input.GetKey(KeyCode.D))
            {
                horizontalInput = 1;
                shootDirection = Vector2.right;
            }

            // Jumping
            if (Input.GetKeyDown(KeyCode.W) && isGrounded)
            {
                r2d.velocity = new Vector2(r2d.velocity.x, jumpHeight);
            }

            // Shooting
            if (Input.GetKeyDown(KeyCode.Space))
            {
                ShootProjectile();
            }
        }
        else if (playerNumber == 2)
        {
            if (Input.GetKey(KeyCode.LeftArrow))
            {
                horizontalInput = -1;
                shootDirection = Vector2.left;
            }
            else if (Input.GetKey(KeyCode.RightArrow))
            {
                horizontalInput = 1;
                shootDirection = Vector2.right;
            }

            // Jumping
            if (Input.GetKeyDown(KeyCode.UpArrow) && isGrounded)
            {
                r2d.velocity = new Vector2(r2d.velocity.x, jumpHeight);
            }

            // Shooting
            if (Input.GetKeyDown(KeyCode.Return)) // Use Enter for Player 2 shooting
            {
                ShootProjectile();
            }
        }

        // Flip the sprite for sidescroller movement
        if (horizontalInput > 0)
        {
            // Face right (default orientation)
            transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }
        else if (horizontalInput < 0)
        {
            // Face left (flipped horizontally)
            transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }
    }

    void FixedUpdate()
    {
        // Check for left boundary
        Vector3 currentPosition = transform.position;
        if (currentPosition.x < leftBoundary && horizontalInput < 0)
        {
            horizontalInput = 0; // Prevent left movement
        }

        // Apply movement velocity
        float targetSpeed = horizontalInput * maxSpeed;
        r2d.velocity = new Vector2(targetSpeed, r2d.velocity.y);

        // Ground check
        Bounds colliderBounds = mainCollider.bounds;
        float colliderRadius = mainCollider.size.x * 0.4f * Mathf.Abs(transform.localScale.x);
        Vector3 groundCheckPos = colliderBounds.min + new Vector3(colliderBounds.size.x * 0.5f, colliderRadius * 0.9f, 0);

        Collider2D[] colliders = Physics2D.OverlapCircleAll(groundCheckPos, colliderRadius);
        isGrounded = false;
        foreach (Collider2D collider in colliders)
        {
            if (collider != mainCollider)
            {
                isGrounded = true;
                break;
            }
        }
    }

    private void ShootProjectile()
    {
        // Calculate the spawn position with an offset based on the shoot direction
        Vector3 spawnPosition = transform.position + (Vector3)shootDirection * 0.5f;

        // Instantiate the projectile at the offset position
        GameObject projectile = Instantiate(projectilePrefab, spawnPosition, Quaternion.identity);

        // Set the projectile's layer to Projectile
        projectile.layer = LayerMask.NameToLayer("Projectile");

        // Set projectile direction based on the last movement direction
        Rigidbody2D projectileRb = projectile.GetComponent<Rigidbody2D>();
        projectileRb.velocity = shootDirection * projectileSpeed;
    }
}