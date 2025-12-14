using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VoidRift : MonoBehaviour
{
    public int damage = 25;
    public float knockbackForce = 5f;
    public float activeDuration = 1f;
    public float inactiveDuration = 1f;

    private bool isActive = false;
    private Animator anim;
    private SpriteRenderer sr;

    private void Start()
    {
        anim = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();

        // Start invisible
        sr.enabled = false;

        StartCoroutine(RiftCycle());
    }

    private IEnumerator RiftCycle()
    {
        while (true)
        {
            DeactivateRift();
            yield return new WaitForSeconds(inactiveDuration);

            ActivateRift();
            yield return new WaitForSeconds(activeDuration);
        }
    }

    private void ActivateRift()
    {
        isActive = true;

        // Enable the sprite and set Animator Bool
        sr.enabled = true;
        anim.SetBool("IsActive", true); // Animator handles looping
    }

    private void DeactivateRift()
    {
        isActive = false;

        // Hide the sprite and reset Animator Bool
        sr.enabled = false;
        anim.SetBool("IsActive", false);
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
                    Vector2 knockbackDir =
                        (collision.transform.position - transform.position).normalized;
                    rb.AddForce(knockbackDir * knockbackForce, ForceMode2D.Impulse);
                }
            }
        }
    }
}