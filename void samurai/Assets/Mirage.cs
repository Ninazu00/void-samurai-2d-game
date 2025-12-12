using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Collections;
using UnityEngine;

public class MirageEnemyController : EnemyController
{
    public float detectionRange = 7f;
    public float visionAngle = 50f;
    public float attackCooldown = 2f;

    public GameObject orbProjectile;
    public Transform orbSpawnPoint;
    public float orbAttackRange = 8f;

    public float floatAmplitude = 0.3f;
    public float floatFrequency = 2f;

    private float nextAttackTime;
    private Vector3 floatStartPos;

    protected override void Start()
    {
        base.Start();
        maxHealth = 150;
        currentHealth = maxHealth;

        floatStartPos = transform.position;
    }

    protected override void EnemyBehavior()
    {
        FloatingMotion();

        if (!PlayerInVisionCone())
            return;

        float distance = Vector2.Distance(transform.position, player.position);

        if (distance > attackRange)
        {
            MoveTowardPlayer();
        }
        else if (Time.time >= nextAttackTime)
        {
            StartCoroutine(MeleeScytheAttack());
            nextAttackTime = Time.time + attackCooldown;
        }
        else if (distance <= orbAttackRange && Time.time >= nextAttackTime)
        {
            StartCoroutine(RangedOrbAttack());
            nextAttackTime = Time.time + attackCooldown;
        }
    }

    private void FloatingMotion()
    {
        float yOffset = Mathf.Sin(Time.time * floatFrequency) * floatAmplitude;
        transform.position = new Vector3(transform.position.x, floatStartPos.y + yOffset, transform.position.z);
    }

    private bool PlayerInVisionCone()
    {
        Vector2 dirToPlayer = player.position - transform.position;

        if (dirToPlayer.magnitude > detectionRange)
            return false;

        float angle = Vector2.Angle(transform.right, dirToPlayer);
        return angle < visionAngle;
    }

    private void MoveTowardPlayer()
    {
        Vector2 direction = (player.position - transform.position).normalized;
        direction += new Vector2(Random.Range(-0.1f, 0.1f), Random.Range(-0.1f, 0.1f));
        rb.MovePosition(rb.position + direction * moveSpeed * Time.deltaTime);

        if (direction.x > 0)
            transform.localScale = new Vector3(1, 1, 1);
        else
            transform.localScale = new Vector3(-1, 1, 1);
    }

    IEnumerator MeleeAttack()
    {
        yield return new WaitForSeconds(0.15f);

        if (PlayerInRange(attackRange))
        {
            player.GetComponent<PlayerHealth>()?.TakeDamage(damage);
        }
    }

    IEnumerator RangedOrbAttack()
    {
        yield return new WaitForSeconds(0.2f);

        if (orbProjectile != null && orbSpawnPoint != null)
        {
            GameObject orb = Instantiate(orbProjectile, orbSpawnPoint.position, Quaternion.identity);
            Vector2 dir = (player.position - orbSpawnPoint.position).normalized;
            orb.GetComponent<Rigidbody2D>().velocity = dir * 6f;
        }
    }
}

