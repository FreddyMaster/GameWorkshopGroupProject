using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public int health = 2; // Set the initial health to 2 for two-hit kill

    public void TakeDamage(int damage)
    {
        // Reduce health by the amount of damage taken
        health -= damage;

        // Check if health has reached 0
        if (health <= 0)
        {
            Destroy(gameObject); // Destroy the enemy if health is 0 or less
        }
    }
}
