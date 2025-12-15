
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mirage : EnemyController
{
    [Header("Detection & Attack")]
    public float detectionRange = 7f;
    public float visionAngle = 50f;
    public float attackCooldown = 2f;
    public float meleeAttackRange = 1.5f;
    public float orbAttackRange = 8f;

    [Header("Ranged Attack")]
    public GameObject orbProjectile;
    public Transform orbSpawnPoint;
    public int orbCount = 3;
    public float orbDelay = 0.5f;
    public float orbSpeed = 6f;

    [Header("Floating Motion")]
    public float floatAmplitude = 0.3f;
    public float floatFrequency = 2f;

    [Header("Patrol Settings")]
    public Transform patrolPointA;
    public Transform patrolPointB;
    public float patrolSpeed = 2f;

    private Animator animator;
    private SpriteRenderer sr;
    private Vector3 currentTarget;
    private float nextAttackTime;
    private Vector3 floatStartPos;
    private bool isAttacking;

    protected override void Start()
    {
        base.Start();

        animator = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();

        currentHealth = maxHealth;
        floatStartPos = transform.position;

        if (patrolPointA != null)
            currentTarget = patrolPointA.position;
    }

    protected override void EnemyBehavior()
    {
        FloatingMotion();

        if (player == null)
        {
            Patrol();
            animator.SetBool("IsIdle", true);
            return;
        }

        float distance = Vector2.Distance(transform.position, player.position);

        if (distance <= orbAttackRange)
        {
            FacePlayer();

            if (Time.time >= nextAttackTime && !isAttacking)
            {
                if (distance <= meleeAttackRange)
                    StartCoroutine(MeleeAttackRoutine());
                else
                    StartCoroutine(RangedAttackRoutine());
            }
            else if (!isAttacking)
            {
                animator.SetBool("IsIdle", true);
            }
        }
        else
        {
            Patrol();
            animator.SetBool("IsIdle", true);
        }
    }

    IEnumerator MeleeAttackRoutine()
    {
        isAttacking = true;
        nextAttackTime = Time.time + attackCooldown;

        animator.SetBool("IsIdle", false);
        animator.SetTrigger("MeleeAttack");

        yield return new WaitForSeconds(0.25f);

        if (player != null &&
            Vector2.Distance(transform.position, player.position) <= meleeAttackRange)
        {
            player.GetComponent<PlayerStats>()?.TakeDamage(damage);
        }

        yield return new WaitForSeconds(0.35f);

        isAttacking = false;
        animator.SetBool("IsIdle", true);

    }

    IEnumerator RangedAttackRoutine()
    {
        isAttacking = true;
        nextAttackTime = Time.time + attackCooldown;

        animator.SetBool("IsIdle", false);
        animator.SetBool("IsRangedActive", true);

        yield return new WaitForSeconds(0.3f);

        for (int i = 0; i < orbCount; i++)
        {
            if (orbProjectile != null && orbSpawnPoint != null && player != null)
            {
                GameObject orb = Instantiate(
                    orbProjectile,
                    orbSpawnPoint.position,
                    Quaternion.identity
                );

                Vector2 dir = (player.position - orbSpawnPoint.position).normalized;
                orb.GetComponent<Rigidbody2D>().velocity = dir * orbSpeed;
            }

            yield return new WaitForSeconds(orbDelay);
        }

        animator.SetBool("IsRangedActive", false);

        yield return new WaitForSeconds(0.4f);

        isAttacking = false;
        animator.SetBool("IsIdle", true);

    }

    private void FloatingMotion()
    {
        float yOffset = Mathf.Sin(Time.time * floatFrequency) * floatAmplitude;
        transform.position = new Vector3(
            transform.position.x,
            floatStartPos.y + yOffset,
            transform.position.z
        );
    }

    private void Patrol()
    {
        if (patrolPointA == null || patrolPointB == null)
            return;

        float targetX = currentTarget.x;
        float newX = Mathf.MoveTowards(transform.position.x, targetX, patrolSpeed * Time.deltaTime);

        transform.position = new Vector3(newX, transform.position.y, transform.position.z);

        sr.flipX = !(currentTarget.x < transform.position.x);

        if (Mathf.Abs(transform.position.x - currentTarget.x) < 0.05f)
        {
            currentTarget = currentTarget == patrolPointA.position
                ? patrolPointB.position
                : patrolPointA.position;
        }
    }

    private void FacePlayer()
    {
        if (player != null)
            sr.flipX = !(player.position.x < transform.position.x);
    }
}