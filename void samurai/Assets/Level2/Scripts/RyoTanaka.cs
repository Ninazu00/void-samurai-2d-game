using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RyoTanaka : EnemyController
{
    // Combat parameters
    [Header("Combat Settings")]
    public float aggroRange = 12f;
    public float phantomDashSpeed = 12f;
    public float phantomDashRange = 6f;
    public int phantomDashDamage = 25;
    public int mirroringGhostHits = 5;
    public float mirroringGhostDelay = 0.2f;
    public int mirroringGhostDamage = 15;
    public int sacrificeMirrorDamage = 40;
    public float sacrificeMirrorRange = 8f;
    public float phantomDashCooldown = 5f;
    public float mirroringGhostCooldown = 8f;
    
    // State machine
    private int currentState = 0; // 0: idle, 1: chase, 2: phantom dash, 3: mirroring ghost, 4: sacrifice mirror
    private PlayerStats playerStats;
    private SpriteRenderer sr;
    private Animator anim;
    
    // Attack tracking
    private float phantomDashTimer = 0f;
    private float mirroringGhostTimer = 0f;
    private int ghostHitsRemaining = 0;
    private float ghostHitTimer = 0f;
    
    // Phantom dash
    private Vector2 dashDirection;
    private bool isDashing = false;
    private float dashTimer = 0f;
    private float dashDuration = 0.5f;
    
    // Sacrifice Mirror (enhanced phantom dash)
    private bool isSacrificeDashing = false;
    private float sacrificeDashTimer = 0f;
    private float sacrificeDashDuration = 0.7f; // Slightly longer than normal dash
    
    // Phase tracking
    private bool isLowHealth = false;
    private bool hasDamagedWithSacrifice = false;
    
    protected override void Start()
    {
        // Set Ryo's stats from the design document
        moveSpeed = 2f;
        maxHealth = 150;
        damage = 0;
        attackRange = 2f;
        
        base.Start();
        
        if (player != null)
            playerStats = player.GetComponent<PlayerStats>();
        
        sr = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
    }

    protected override void EnemyBehavior()
    {
        if (player == null) return;
        
        float dist = Vector2.Distance(transform.position, player.position);
        
        // Update cooldown timers
        if (phantomDashTimer > 0)
            phantomDashTimer -= Time.deltaTime;
        if (mirroringGhostTimer > 0)
            mirroringGhostTimer -= Time.deltaTime;
        
        // Check if low health for Sacrifice Mirror (20% health threshold)
        if (currentHealth <= maxHealth * 0.2f && !isLowHealth)
        {
            isLowHealth = true;
            currentState = 4; // Trigger Sacrifice Mirror
            hasDamagedWithSacrifice = false;
            
            // Set up the sacrifice dash (like phantom dash but stronger)
            Vector2 directionToPlayer = (player.position - transform.position).normalized;
            dashDirection = new Vector2(directionToPlayer.x, 0).normalized;
            isSacrificeDashing = true;
            sacrificeDashTimer = 0f;
            
            // Flip sprite for dash direction
            if (sr != null && dashDirection.x != 0)
                sr.flipX = dashDirection.x > 0;
            
            Debug.Log("Ryo Tanaka enters Sacrifice Mirror mode!");
            return;
        }
        
        // State machine
        switch (currentState)
        {
            case 0: // Idle
                if (dist <= aggroRange)
                    currentState = 1;
                break;
            case 1: // Chase
                ChasePlayer(dist);
                break;
            case 2: // Phantom Dash
                PhantomDash();
                break;
            case 3: // Mirroring Ghost
                MirroringGhost();
                break;
            case 4: // Sacrifice Mirror
                SacrificeMirror(dist);
                break;
        }
        
        // Update animator
        if (anim != null)
        {
            anim.SetFloat("Speed", Mathf.Abs(rb.velocity.x));
            anim.SetInteger("State", currentState);
        }
    }

    void ChasePlayer(float dist)
    {
        // Check if can use Phantom Dash ability
        if (dist <= phantomDashRange && phantomDashTimer <= 0)
        {
            currentState = 2;
            Vector2 directionToPlayer = (player.position - transform.position).normalized;
            dashDirection = new Vector2(directionToPlayer.x, 0).normalized;
            isDashing = true;
            dashTimer = 0f;
            phantomDashTimer = phantomDashCooldown;
            
            if (sr != null && dashDirection.x != 0)
                sr.flipX = dashDirection.x > 0;
            return;
        }
        
        // Check if can use Mirroring Ghost ability
        if (dist <= 3f && mirroringGhostTimer <= 0)
        {
            currentState = 3;
            ghostHitsRemaining = mirroringGhostHits;
            ghostHitTimer = 0f;
            mirroringGhostTimer = mirroringGhostCooldown;
            rb.velocity = Vector2.zero;
            return;
        }
        
        // Move toward player
        Vector2 dir = (player.position - transform.position).normalized;
        rb.velocity = new Vector2(dir.x * moveSpeed, rb.velocity.y);
        
        if (sr != null && dir.x != 0)
            sr.flipX = dir.x > 0;
    }

    void PhantomDash()
    {
        if (isDashing)
        {
            dashTimer += Time.deltaTime;
            float dashVelocityX = dashDirection.x * phantomDashSpeed;
            rb.velocity = new Vector2(dashVelocityX, rb.velocity.y);
            
            if (dashTimer >= dashDuration)
            {
                isDashing = false;
                currentState = 1;
            }
        }
    }

    void MirroringGhost()
    {
        if (ghostHitsRemaining > 0)
        {
            ghostHitTimer += Time.deltaTime;
            
            if (ghostHitTimer >= mirroringGhostDelay)
            {
                ghostHitTimer = 0f;
                ghostHitsRemaining--;
                
                float dist = Vector2.Distance(transform.position, player.position);
                if (dist <= attackRange && playerStats != null)
                {
                    playerStats.TakeDamage(mirroringGhostDamage);
                    Debug.Log($"Mirroring Ghost hit! {ghostHitsRemaining} hits remaining");
                }
            }
        }
        else
        {
            currentState = 1;
        }
    }

    void SacrificeMirror(float dist)
    {
        // Enhanced phantom dash - slower but double damage
        if (isSacrificeDashing)
        {
            sacrificeDashTimer += Time.deltaTime;
            
            // Turn red ONLY during the Sacrifice Mirror dash
            if (sr != null)
                sr.color = new Color(1f, 0.2f, 0.2f); // Bright intense red
            
            // Dash at 80% of normal phantom dash speed (slower)
            float sacrificeDashSpeed = phantomDashSpeed * 0.8f;
            float dashVelocityX = dashDirection.x * sacrificeDashSpeed;
            rb.velocity = new Vector2(dashVelocityX, rb.velocity.y);
            
            if (sacrificeDashTimer >= sacrificeDashDuration)
            {
                isSacrificeDashing = false;
                currentState = 1; // Return to chase state
                
                // Return to normal white color after dash
                if (sr != null)
                    sr.color = Color.white;
                
                Debug.Log("Sacrifice Mirror dash ended, returning to chase");
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && playerStats != null)
        {
            int dmg = 0;
            
            if (currentState == 2) // Phantom Dash
            {
                dmg = phantomDashDamage;
                Debug.Log("Phantom Dash collision damage: " + dmg);
            }
            else if (currentState == 4 && !hasDamagedWithSacrifice) // Sacrifice Mirror
            {
                // Double damage of phantom dash
                dmg = phantomDashDamage * 2;
                hasDamagedWithSacrifice = true;
                Debug.Log("Sacrifice Mirror collision damage: " + dmg + " (DOUBLE DAMAGE!)");
            }
            
            if (dmg > 0)
                playerStats.TakeDamage(dmg);
        }
    }

    public override void TakeDamage(int dmg)
    {
        currentHealth -= dmg;
        
        Debug.Log($"Ryo Tanaka took {dmg} damage. Health: {currentHealth}/{maxHealth}");
        
        // Flash red when damaged
        if (sr != null)
        {
            sr.color = Color.red;
            Invoke("ResetColor", 0.1f);
        }
        
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void ResetColor()
    {
        if (sr != null)
            sr.color = Color.white;
    }

    protected override void Die()
    {
        Debug.Log("Ryo Tanaka defeated!");

        // --- ADDITION: Unlock Resolve Stance ---
        PlayerController player = FindObjectOfType<PlayerController>();
        if (player != null)
        {
            player.UnlockResolveStance();
        }

        base.Die();
    }
}
