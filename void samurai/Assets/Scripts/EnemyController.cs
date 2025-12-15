using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyController : MonoBehaviour
{
    public int maxHealth;
    public int currentHealth;
    public float moveSpeed;

    public int damage;
    public float attackRange;

    protected Transform player;
    protected Rigidbody2D rb;

    protected virtual void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            player = playerObj.transform;
        }
        currentHealth = maxHealth;
    }

    protected virtual void Update()
    {
        EnemyBehavior();
    }

    protected abstract void EnemyBehavior();

    public virtual void TakeDamage(int dmg)
    {
        currentHealth -= dmg;
        if (currentHealth <= 0)
            Die();
    }

    protected virtual void Die()
    {
        Destroy(gameObject);
    }

    protected bool PlayerInRange(float range)
    {
        return Vector2.Distance(transform.position, player.position) <= range;
    }
}
