using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbProjectile : MonoBehaviour
{
    public int damage = 20;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<BoxCollider2D>() != null)
        {
            if (collision.CompareTag("Player"))
            {
                PlayerController player = collision.GetComponent<PlayerController>();

                if (player != null && player.IsParrying())
                {
                    Debug.Log("Projectile canceled by parry!");
                    Destroy(gameObject);
                    return; 
                }

                collision.GetComponent<PlayerStats>()?.TakeDamage(damage);
            }

            Destroy(gameObject); 
        }
    }
}