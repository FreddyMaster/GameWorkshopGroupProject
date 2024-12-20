using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    public Image[] hearts; // Array to hold heart images in the UI
    public int health = 2; // Starting health, same as number of hearts
    public SpriteRenderer playerSprite;
    public float invincibilityDurationSeconds = 1f;
    public float delayBetweenInvincibilityFlashes = 1f;
    public GameObject gameOverPanel; // Reference to the Game Over UI panel
    public float fallThreshold = -10f; // Y position threshold to detect falling off the map
    private bool isInvincible = false;
        private Vector2 shootDirection = Vector2.right;


    void Start()
    {
        Time.timeScale = 1f;
        gameOverPanel.SetActive(false); // Ensure Game Over panel is hidden at start
        UpdateHearts();
    }

    void Update()
    {
        // Check if the player has fallen below the threshold

    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("DeathTrigger"))
        {
            Die();
        }
    }
    // Call this function when the player is hit by an enemy
    public void TakeDamage(int damage)
    {
        if (isInvincible) return;

        if (health > 0)
        {
            health -= damage;
            UpdateHearts();
        }

        if (health <= 0)
        {
            Die();
        }
    }

    // Updates the heart display based on the current health
    private void UpdateHearts()
    {
        StartCoroutine(BecomeTemporarilyInvincible());

        for (int i = 0; i < hearts.Length; i++)
        {
            hearts[i].enabled = i < health; // Show heart if health allows
        }
    }

    // Handle player death
    private void Die()
    {
        Debug.Log("Player Died!");
        gameOverPanel.SetActive(true); // Show the Game Over panel
        Time.timeScale = 0; // Pause the game
    }

    // Detect collision with an enemy or potion
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy") || collision.gameObject.CompareTag("EnemyProjectile"))
        {
            TakeDamage(1);
        }
        else if (collision.gameObject.CompareTag("Potion"))
        {
            if (health < hearts.Length)
            {
                HealPlayer(collision.gameObject);
            }
            else
            {
                // Temporarily disable the potion collider to allow the player to phase through
                StartCoroutine(TemporarilyDisableCollider(collision.collider));
            }
        }
    }

    // Function to heal the player when they collect a potion
    private void HealPlayer(GameObject potion)
    {
        if (health < hearts.Length)
        {
            health++; // Increase health by 1
            UpdateHearts();
            Destroy(potion); // Destroy the potion object after healing
            Debug.Log("Player healed by potion!");
        }
    }

    // Coroutine to temporarily disable the collider of the potion
    private IEnumerator TemporarilyDisableCollider(Collider2D potionCollider)
    {
        potionCollider.enabled = false; // Disable the collider
        yield return new WaitForSeconds(0.5f); // Wait for half a second
        potionCollider.enabled = true; // Re-enable the collider
    }

    // Function to restart the game, linked to the Retry button in the Game Over panel
    public void RetryGame()
    {
        SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex); // Reload the current scene
    }

    private IEnumerator BecomeTemporarilyInvincible()
    {
        Debug.Log("Player turned invincible!");
        isInvincible = true;

        // Flash on and off for roughly invincibilityDurationSeconds seconds
        for (float i = 0; i < invincibilityDurationSeconds; i += delayBetweenInvincibilityFlashes)
        {
            playerSprite.color = Color.red;
            yield return new WaitForSeconds(delayBetweenInvincibilityFlashes);
            playerSprite.color = Color.white;
            yield return new WaitForSeconds(delayBetweenInvincibilityFlashes);
        }

        Debug.Log("Player is no longer invincible!");
        isInvincible = false;
    }
}