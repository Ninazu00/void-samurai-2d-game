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

    // ----------- STAGGER SETTINGS -----------
    public float staggerDuration = 0.6f;
    protected bool isStaggered;

    protected Transform player;
    protected Rigidbody2D rb;
    protected CombatZone combatZone;

    protected virtual void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
            player = playerObj.transform;

        currentHealth = maxHealth;

        combatZone = GetComponentInParent<CombatZone>();
        if (combatZone != null)
            combatZone.RegisterEnemy(this);
    }

    protected virtual void Update()
    {
        if (isStaggered)
            return; // enemy frozen during stagger

        EnemyBehavior();
    }

    protected abstract void EnemyBehavior();

    public virtual void TakeDamage(int dmg)
    {
        currentHealth -= dmg;
        Debug.Log("Enemy Took Damage " + dmg);

        if (currentHealth <= 0)
            Die();
    }

    public virtual void ApplyStagger()
    {
        if (isStaggered)
            return;

        isStaggered = true;
        rb.velocity = Vector2.zero;
        Debug.Log("Enemy Staggered!");

        Invoke(nameof(EndStagger), staggerDuration);
    }

    void EndStagger()
    {
        isStaggered = false;
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
