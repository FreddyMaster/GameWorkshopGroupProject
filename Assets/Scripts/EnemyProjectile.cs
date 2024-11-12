using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectile : MonoBehaviour
{
    public float lifespan = 5f; // Time in seconds before the projectile is destroyed
    public int damage = 1; // Damage dealt to enemies upon collision

    private void Start()
    {
        // Destroy the projectile after a set lifespan to prevent it from existing indefinitely
        Destroy(gameObject, lifespan);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Check if the projectile hits an enemy
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("Collided with player");
            // Apply damage to the enemy, assuming it has an EnemyHealth component
            PlayerHealth playerHealth = collision.gameObject.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(damage); // Call TakeDamage on the enemy
            }

            // Destroy the projectile on impact with an enemy
            Destroy(gameObject);
        }
    }
}