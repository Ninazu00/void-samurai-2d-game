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

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            inside = true;
            playerStats = other.GetComponent<PlayerStats>();
        }
    }

    void OnTriggerExit(Collider other)
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
            playerStats.TakeDamage(damagePerTick);
            damageTimer = 0f;
        }
    }
}
