using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MemoryEchoField : MonoBehaviour
{
    public int damagePerTick = 5;
    public float damageInterval = 1f;

    private bool inside = false;
    private float damageTimer = 0f;

    private PlayerStats playerStats;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            inside = true;
            playerStats = other.GetComponent<PlayerStats>();
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            inside = false;
            damageTimer = 0f;
        }
    }

    void Update()
    {
        if (!inside || playerStats == null) return;

        damageTimer += Time.deltaTime;
        if (damageTimer >= damageInterval)
        {
           TakeDamageOverTime(damagePerTick);
            damageTimer = 0f;
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
