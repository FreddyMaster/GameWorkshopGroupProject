using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    public Image[] hearts; // Array to hold heart images in the UI
    public int health = 2; // Starting health, same as number of hearts

    // Call this function when the player is hit by an enemy
    public void TakeDamage()
    {
        if (health > 0)
        {
            health--; // Reduce health by 1
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
        for (int i = 0; i < hearts.Length; i++)
        {
            hearts[i].enabled = i < health; // Show heart if health allows
        }
    }

    // Handle player death
    private void Die()
    {
        // Add your death logic here, like disabling the player or showing a game over screen
        Debug.Log("Player Died!");
    }

    // Detect collision with an enemy
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            TakeDamage();
        }
    }
}
