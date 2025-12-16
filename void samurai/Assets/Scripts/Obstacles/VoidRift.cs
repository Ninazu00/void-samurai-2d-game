using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VoidRift : MonoBehaviour
{
    [Header("Damage")]
    public int damage = 25;
    public float knockbackForce = 5f;

    [Header("Timing")]
    public float activeDuration = 1f;
    public float inactiveDuration = 1f;

    [Header("Audio (Prefab)")]
    public AudioClip riftActivateSFX;   // Assign per prefab

    private bool isActive = false;
    private Animator anim;
    private SpriteRenderer sr;

    private void Start()
    {
        anim = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();

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

        sr.enabled = true;
        anim.SetBool("IsActive", true);

        // 🔊 Play prefab-specific SFX
        if (AudioManager.Instance != null && riftActivateSFX != null)
        {
            AudioManager.Instance.PlayExplosion(riftActivateSFX);
        }
    }

    private void DeactivateRift()
    {
        isActive = false;

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