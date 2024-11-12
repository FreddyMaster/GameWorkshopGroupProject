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
    public GameObject player;
    private bool isGrounded = false;
    private Rigidbody2D r2d;
    private BoxCollider2D mainCollider;
    private Transform t;
    private float lastShotTime = 0f; // Time of the last shot
    private Vector2 shootDirection = Vector2.right;

    // Player identification
    public int playerNumber = 1; // 1 for Player 1, 2 for Player 2
    private static int playersInPortal = 0; // Counter for players in the portal
    
    #region Singleton

    public static PlayerController instance;

    private void Awake()
    {
        instance = this;
    }

    #endregion

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
        Cursor.visible = false;
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
            if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.V) || Input.GetKeyDown(KeyCode.B))
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
            if (Input.GetKeyDown(KeyCode.Alpha4) || Input.GetKeyDown(KeyCode.Alpha5) || Input.GetKeyDown(KeyCode.Alpha6)) 
            {
                ShootProjectile();
            }
        }

        // Flip the sprite for sidescroller movement
        if (horizontalInput > 0)
        {
            transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }
        else if (horizontalInput < 0)
        {
            transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }

        if (Input.GetKeyUp(KeyCode.Escape)) { 
	        Application.Quit();
        }  
    }

    void FixedUpdate()
    {
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
        if (Time.time >= lastShotTime + 1f / fireRate)
        {
            // Calculate the spawn position with an offset based on the shoot direction
            Vector3 spawnPosition = transform.position + (Vector3)shootDirection * 0.5f;

            // Instantiate the projectile at the offset position
            GameObject projectile = Instantiate(projectilePrefab, spawnPosition, Quaternion.identity);

            // Set the projectile's layer to Projectile
            projectile.layer = LayerMask.NameToLayer("Projectile");

            // Flip the projectile's scale if the player is facing left
            if (shootDirection == Vector2.left)
            {
                projectile.transform.localScale = new Vector3(-projectile.transform.localScale.x, projectile.transform.localScale.y, projectile.transform.localScale.z);
            }

            // Set projectile direction based on the last movement direction
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
            Destroy(gameObject);
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