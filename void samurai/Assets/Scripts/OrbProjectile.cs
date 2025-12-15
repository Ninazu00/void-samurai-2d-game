using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbProjectile : MonoBehaviour
{
    public int damage = 20; // Damage of the projectile

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Check if it hit the player
        if (collision.CompareTag("Player"))
        {
            // Get the PlayerController to check parry state
            PlayerController player = collision.GetComponent<PlayerController>();

            if (player != null)
            {
                // ----------- PARRY CHECK -----------
                // If player is parrying, cancel the projectile
                if (player.IsParrying())
                {
                    Debug.Log("Projectile canceled by parry!");
                    Destroy(gameObject); // Destroy the projectile
                    return; // Stop execution
                }
            }

            // ----------- NORMAL DAMAGE -----------
            // Deal damage if not parrying
            collision.GetComponent<PlayerStats>()?.TakeDamage(damage);
            Destroy(gameObject); 
        }
    }
}
