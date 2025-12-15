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
    protected CombatZone combatZone;

    protected virtual void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            player = playerObj.transform;
        }
        currentHealth = maxHealth;

        // Register to combat zone if it exists
        combatZone = GetComponentInParent<CombatZone>();
        if (combatZone != null)
            combatZone.RegisterEnemy(this);
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
        if (combatZone != null)
            combatZone.UnregisterEnemy(this);
            
        Destroy(gameObject);
    }

    protected bool PlayerInRange(float range)
    {
        return Vector2.Distance(transform.position, player.position) <= range;
    }
}
