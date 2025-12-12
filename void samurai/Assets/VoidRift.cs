using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Collections;
using UnityEngine;

public class VoidRift : MonoBehaviour
{
    public int damage = 25;
    public float knockbackForce = 5f;
    public float activeDuration = 1.5f;
    public float inactiveDuration = 3f;
    public GameObject voidlingPrefab;
    public Transform spawnPoint;

    private bool isActive = false;

    private void Start()
    {
        StartCoroutine(RiftCycle());
    }

    private IEnumerator RiftCycle()
    {
        while (true)
        {
            yield return new WaitForSeconds(inactiveDuration);
            ActivateRift();
            yield return new WaitForSeconds(activeDuration);
            DeactivateRift();
        }
    }

    private void ActivateRift()
    {
        isActive = true;
        if (voidlingPrefab != null && spawnPoint != null)
        {
            Instantiate(voidlingPrefab, spawnPoint.position, Quaternion.identity);
        }
        // Optional: trigger visual/sound effects here
    }

    private void DeactivateRift()
    {
        isActive = false;
        // Optional: deactivate visual effects
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!isActive)
            return;

        if (collision.CompareTag("Player"))
        {
            PlayerStats playerStats = collision.GetComponent<PlayerStats>();
            if (playerStats != null)
            {
                playerStats.TakeDamage(damage);

                Rigidbody2D rb = collision.GetComponent<Rigidbody2D>();
                if (rb != null)
                {
                    Vector2 knockbackDir = (collision.transform.position - transform.position).normalized;
                    rb.AddForce(knockbackDir * knockbackForce, ForceMode2D.Impulse);
                }
            }
        }
    }
}
