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
    public AudioClip riftActivateSFX; // Assign per prefab
    public float maxHearingDistance = 10f; // Max distance to hear the rift
    public float maxVolume = 1f;           // Volume at closest distance

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

        PlayRiftSFX(); // Play SFX based on distance
    }

    private void DeactivateRift()
    {
        isActive = false;

        sr.enabled = false;
        anim.SetBool("IsActive", false);
    }

    private void PlayRiftSFX()
    {
        if (AudioManager.Instance == null || riftActivateSFX == null)
            return;

        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player == null)
            return;

        float distance = Vector3.Distance(transform.position, player.transform.position);
        if (distance > maxHearingDistance)
            return; // too far to hear

        float volume = Mathf.Lerp(maxVolume, 0f, distance / maxHearingDistance);

        AudioManager.Instance.sfxSource.PlayOneShot(riftActivateSFX, volume);
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