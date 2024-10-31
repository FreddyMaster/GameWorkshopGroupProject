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
    public Camera mainCamera;
    public GameObject projectilePrefab;
    public float projectileSpeed = 150.0f;
    private float moveDirection = 0;
    private bool isGrounded = false;
    private Vector3 cameraPos;
    private Rigidbody2D r2d;
    private BoxCollider2D mainCollider;
    private Transform t;
    private Vector2 shootDirection = Vector2.right;

    void Start()
    {
        t = transform;
        r2d = GetComponent<Rigidbody2D>();
        mainCollider = GetComponent<BoxCollider2D>();
        r2d.freezeRotation = true;
        r2d.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
        r2d.gravityScale = gravityScale;

        if (mainCamera)
        {
            cameraPos = mainCamera.transform.position;
        }
    }

    void Update()
    {
        horizontalInput = 0;

        // Movement controls
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


        // Move the player
        MovePlayer(new Vector2(horizontalInput, 0));

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

        // Camera follow
        if (mainCamera)
        {
            mainCamera.transform.position = new Vector3(t.position.x, cameraPos.y, cameraPos.z);
        }
    }

        void MovePlayer(Vector2 direction)
    {
        // Normalize the direction vector to ensure consistent movement speed
        if (direction.magnitude > 1)
        {
            direction.Normalize();
        }

        // Apply movement
        transform.Translate(direction * Time.deltaTime * maxSpeed, Space.World);
    }

    private void ShootProjectile()
    {
        // Instantiate the projectile at the player's position
        GameObject projectile = Instantiate(projectilePrefab, transform.position, Quaternion.identity);

        // Set projectile direction based on the last movement direction
        Rigidbody2D projectileRb = projectile.GetComponent<Rigidbody2D>();

        projectileRb.velocity = shootDirection * projectileSpeed;
    }

    void FixedUpdate()
    {
        Bounds colliderBounds = mainCollider.bounds;
        float colliderRadius = mainCollider.size.x * 0.4f * Mathf.Abs(transform.localScale.x);
        Vector3 groundCheckPos = colliderBounds.min + new Vector3(colliderBounds.size.x * 0.5f, colliderRadius * 0.9f, 0);

        // Check if player is grounded
        Collider2D[] colliders = Physics2D.OverlapCircleAll(groundCheckPos, colliderRadius);
        isGrounded = false;
        if (colliders.Length > 0)
        {
            for (int i = 0; i < colliders.Length; i++)
            {
                if (colliders[i] != mainCollider)
                {
                    isGrounded = true;
                    break;
                }
            }
        }

        // Apply movement velocity
        r2d.velocity = new Vector2((moveDirection) * maxSpeed, r2d.velocity.y);
    }
}
