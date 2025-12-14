using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VoidOrchids : MonoBehaviour
{
    [Header("Explosion Settings")]
    public int damage = 30;
    public float explosionRadius = 3f;
    public float delayBeforeExplosion = 2f;
    public GameObject explosionEffect;

    [Header("Animation")]
    public string triggerBoolName = "IsTriggered";

    private bool triggered = false;
    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (triggered) return;

        if (collision.CompareTag("Player") || collision.CompareTag("Enemy"))
        {
            triggered = true;
            StartCoroutine(ExplosionRoutine());
        }
    }

    private IEnumerator ExplosionRoutine()
    {
        // Play charging / armed animation (if animator exists)
        if (animator != null)
        {
            animator.SetBool(triggerBoolName, true);
        }

        yield return new WaitForSeconds(delayBeforeExplosion);

        Explode();
    }

    private void Explode()
    {
        // Spawn explosion animation
        if (explosionEffect != null)
        {
            Instantiate(explosionEffect, transform.position, Quaternion.identity);
        }

        Collider2D[] hitColliders =
            Physics2D.OverlapCircleAll(transform.position, explosionRadius);

        foreach (Collider2D hit in hitColliders)
        {
            if (hit.CompareTag("Player"))
            {
                PlayerStats player = hit.GetComponent<PlayerStats>();
                if (player != null)
                    player.TakeDamage(damage);
            }
            else if (hit.CompareTag("Enemy"))
            {
                EnemyController enemy = hit.GetComponent<EnemyController>();
                if (enemy != null)
                    enemy.TakeDamage(damage);
            }
        }

        // Remove plant after explosion
        Destroy(gameObject);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }
}