using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(BoxCollider2D))]

public class PlayerController : MonoBehaviour
{
    public float horizontalInput;
    public float maxSpeed = 5.0f;
    public float jumpHeight = 6.5f;
    public float gravityScale = 1.0f;
    public float fireRate = 5f;
    public GameObject projectilePrefab;
    public float projectileSpeed = 150.0f;
    private bool isGrounded = false;
    private Rigidbody2D r2d;
    private BoxCollider2D mainCollider;
    private Transform t;
    private float lastShotTime = 0f; // Time of the last shot
    private Vector2 shootDirection = Vector2.right;

    public int playerNumber = 1; // 1 for Player 1, 2 for Player 2
    private static int playersInPortal = 0; // Counter for players in the portal

    void Start()
    {
        t = transform;
        r2d = GetComponent<Rigidbody2D>();
        mainCollider = GetComponent<BoxCollider2D>();

        PhysicsMaterial2D noFrictionMaterial = new PhysicsMaterial2D { friction = 0f, bounciness = 0f };
        mainCollider.sharedMaterial = noFrictionMaterial;

        r2d.freezeRotation = true;
        r2d.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
        r2d.gravityScale = gravityScale;
    }

    void Update()
    {
        horizontalInput = 0;

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

            if (Input.GetKeyDown(KeyCode.W) && isGrounded)
            {
                r2d.velocity = new Vector2(r2d.velocity.x, jumpHeight);
            }

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

            if (Input.GetKeyDown(KeyCode.UpArrow) && isGrounded)
            {
                r2d.velocity = new Vector2(r2d.velocity.x, jumpHeight);
            }

            if (Input.GetKeyDown(KeyCode.Return))
            {
                ShootProjectile();
            }
        }

        if (horizontalInput > 0)
        {
            transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }
        else if (horizontalInput < 0)
        {
            transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }
    }

    void FixedUpdate()
    {
        float targetSpeed = horizontalInput * maxSpeed;
        r2d.velocity = new Vector2(targetSpeed, r2d.velocity.y);

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
        if (Time.time >= lastShotTime + 1f / fireRate)
        {
            Vector3 spawnPosition = transform.position + (Vector3)shootDirection * 0.5f;

            GameObject projectile = Instantiate(projectilePrefab, spawnPosition, Quaternion.identity);
            projectile.layer = LayerMask.NameToLayer("Projectile");

            Rigidbody2D projectileRb = projectile.GetComponent<Rigidbody2D>();
            projectileRb.velocity = shootDirection * projectileSpeed;
            lastShotTime = Time.time;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Portal"))
        {
            playersInPortal++;
            CheckPortal();
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Portal"))
        {
            playersInPortal--;
        }
    }

    private void CheckPortal()
    {
        string currentScene = SceneManager.GetActiveScene().name;

        if (playersInPortal >= 2)
        {
            if (currentScene == "Level1")
            {
                SceneManager.LoadScene("Level2");
            }
            else if (currentScene == "Level2")
            {
                Debug.Log("Game Over!");
                Application.Quit(); // Quits the game for standalone
            }
        }
    }
}