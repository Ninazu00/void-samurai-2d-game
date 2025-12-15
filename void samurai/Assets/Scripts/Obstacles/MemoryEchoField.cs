using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MemoryEchoField : MonoBehaviour
{
    public int damagePerTick = 5;
    public float damageInterval = 1f;

    private bool inside = false;
    private float damageTimer = 0f;

    public float blurSpeed = 0.5f;
    public float maxBlur = 0.7f;

    float currentBlur = 0f;

    private PlayerStats playerStats;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            inside = true;
            playerStats = other.GetComponent<PlayerStats>();
        }
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlayVoiceLine(
                AudioManager.Instance.Memechoaudio
            );
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            inside = false;
            damageTimer = 0f;
        }
        if (AudioManager.Instance != null)
    {
        AudioManager.Instance.voiceLines.Stop();
    }

    }

    void Update()
    {

            // First if: only run logic when inside and player exists
            if (playerStats != null)
            {
                // Damage tick
                if (inside)
                {
                    damageTimer += Time.deltaTime;
                    currentBlur += blurSpeed * Time.deltaTime;

                    if (damageTimer >= damageInterval)
                    {
                        TakeDamageOverTime(damagePerTick);
                        damageTimer = 0f;
                    }
                }
                else
                {
                    // Reduce blur when outside
                    currentBlur -= blurSpeed * Time.deltaTime;
                    damageTimer = 0f; // reset timer when leaving
                }

                // Apply blur to screen
                currentBlur = Mathf.Clamp(currentBlur, 0f, maxBlur);
                if (SimpleScreenBlur.instance != null)
                    SimpleScreenBlur.instance.SetBlur(currentBlur);
            }

    }
        public void TakeDamageOverTime(int damage)
        {
            playerStats.health -= damage;
            if (playerStats.health < 0)
                playerStats.health = 0;

            playerStats.slider.value = playerStats.health;

            if (playerStats.health == 0)
            {
                FindObjectOfType<LevelManager>().RespawnPlayer();
            }
        }
}
