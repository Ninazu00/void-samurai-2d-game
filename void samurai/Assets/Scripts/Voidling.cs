using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Voidling : EnemyController
{

    public float detectionRange = 8f;
    public float chargeDistance = 5f;
    public float chargeDelay = 0.8f;
    public float chargeDashSpeed = 15f;
    public int chargeDamage = 15;
    private int currentState = 0;//0: idle/1: following/2: charging/3: dashing
    private Vector2 dashDirection;
    private float chargeTimer;
    private PlayerStats playerStats;
    
    // Flash effect
    private float flashTimer;
    private bool flashOn;
    private Color originalColor;
    private SpriteRenderer sr;
    private Animator anim;
    protected override void Start()
    {
        base.Start();
        moveSpeed = 2f;
        maxHealth = 30;
        currentHealth = maxHealth;
        
        if (player != null)
            playerStats = player.GetComponent<PlayerStats>();
        
        sr = GetComponent<SpriteRenderer>();
        if (sr != null)
            originalColor = sr.color;
        anim = GetComponent<Animator>();
    }
    protected override void EnemyBehavior()
    {
        if (player == null) return;
        
        float dist = Vector2.Distance(transform.position, player.position);
        Vector2 directionToPlayer = (player.position - transform.position).normalized;
        
        // Flip sprite to face player
        if (sr != null)
            sr.flipX = directionToPlayer.x > 0;
        
        if (currentState == 0)
        {
            if (dist <= detectionRange)
                currentState = 1;
        }
        
        else if (currentState == 1)
        {
            if (dist <= chargeDistance)
            {
                currentState = 2;
                chargeTimer = 0f;
                flashTimer = 0f;
            }
            else
            {
                Vector2 dir = (player.position - transform.position).normalized;
                rb.velocity = dir * moveSpeed;
            }
        }
        
        else if (currentState == 2)
        {
            rb.velocity = Vector2.zero;
            chargeTimer += Time.deltaTime;
            
            flashTimer += Time.deltaTime;
            if (flashTimer >= 0.15f)
            {
                flashTimer = 0f;
                flashOn = !flashOn;
                if (sr != null)
                {
                    if (flashOn)
                        sr.color = Color.red;
                    else
                        sr.color = originalColor;
                }
            }
            
            if (chargeTimer >= chargeDelay)
            {
                dashDirection = (player.position - transform.position).normalized;
                currentState = 3;
                if (sr != null)
                    sr.color = originalColor;
            }
        }
        
        else if (currentState == 3)
        {
            rb.velocity = dashDirection * chargeDashSpeed;
            
            if (dist > 30f)
                Destroy(gameObject);
        }
        if (anim != null)
        {
            anim.SetFloat("Speed", Mathf.Abs(GetComponent<Rigidbody2D>().velocity.x));
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && playerStats != null)
        {
            playerStats.TakeDamage(chargeDamage);
        }
        Destroy(gameObject);
    }
    public override void TakeDamage(int dmg)
    {
        if (currentState != 3)
        {
            currentHealth -= dmg;
            if (currentHealth <= 0)
                Destroy(gameObject);
        }
    }
}
