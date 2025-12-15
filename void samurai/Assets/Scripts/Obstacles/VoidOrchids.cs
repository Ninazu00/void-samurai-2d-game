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

    [Header("Shake Settings")]
    public float shakeAmount = 0.1f;
    public float shakeSpeed = 0.05f;

    private bool triggered = false;
    private Animator animator;
    private Vector3 originalPosition;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        originalPosition = transform.position;
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
     
        StartCoroutine(Shake());

        yield return new WaitForSeconds(delayBeforeExplosion);

        Explode();
    }

    private IEnumerator Shake()
    {
        float elapsed = 0f;

        while (elapsed < delayBeforeExplosion)
        {
            Vector2 randomOffset = Random.insideUnitCircle * shakeAmount;
            transform.position = originalPosition + (Vector3)randomOffset;

            elapsed += shakeSpeed;
            yield return new WaitForSeconds(shakeSpeed);
        }

        // Reset position before exploding
        transform.position = originalPosition;
    }

    private void Explode()
    {
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

        Destroy(gameObject);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }
}