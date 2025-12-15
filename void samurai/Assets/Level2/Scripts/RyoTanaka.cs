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
    
    // Phase tracking
    private bool isLowHealth = false;
    
    protected override void Start()
    {
        // Set Ryo's stats from the design document
        moveSpeed = 2f; // Use the custom speed variable
        maxHealth = 150;
        damage = 0; // No base damage - only skill-based damage
        attackRange = 2f; // Melee range
        
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
                SacrificeMirror();
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
            // Calculate horizontal-only dash direction
            Vector2 directionToPlayer = (player.position - transform.position).normalized;
            dashDirection = new Vector2(directionToPlayer.x, 0).normalized;
            isDashing = true;
            dashTimer = 0f;
            phantomDashTimer = phantomDashCooldown;
            
            // Flip sprite for dash direction
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
        
        // Move toward player (only horizontal movement to stay on ground)
        Vector2 dir = (player.position - transform.position).normalized;
        rb.velocity = new Vector2(dir.x * moveSpeed, rb.velocity.y); // Preserve vertical velocity
        
        // Flip sprite based on movement direction
        // If moving right (positive direction), DON'T flip. If moving left (negative), flip
        if (sr != null && dir.x != 0)
            sr.flipX = dir.x > 0;
    }

    void PhantomDash()
    {
        if (isDashing)
        {
            dashTimer += Time.deltaTime;
            // Dash horizontally while preserving gravity
            float dashVelocityX = dashDirection.x * phantomDashSpeed;
            rb.velocity = new Vector2(dashVelocityX, rb.velocity.y);
            
            if (dashTimer >= dashDuration)
            {
                isDashing = false;
                currentState = 1; // Return to chase state
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
                
                // Check if player is in range for hit
                float dist = Vector2.Distance(transform.position, player.position);
                if (dist <= attackRange && playerStats != null)
                {
                    playerStats.TakeDamage(mirroringGhostDamage);
                }
            }
        }
        else
        {
            currentState = 1; // Return to chase state
        }
    }

    void SacrificeMirror()
    {
        // Final desperate attack - mirrors his sacrifice in the war
        float dist = Vector2.Distance(transform.position, player.position);
        
        // Charge toward player with doubled speed (horizontal only)
        Vector2 dir = (player.position - transform.position).normalized;
        rb.velocity = new Vector2(dir.x * (moveSpeed * 2f), rb.velocity.y);
        
        // If close enough, deal massive damage and transform
        if (dist <= attackRange && playerStats != null)
        {
            playerStats.TakeDamage(sacrificeMirrorDamage);
            TransformToSpirit();
        }
    }

    void TransformToSpirit()
    {
        // Ryo transforms from void version into his true old self as a spirit
        Debug.Log("Ryo transforms into spirit form...");
        
        // Stop all movement
        rb.velocity = Vector2.zero;
        currentState = 0; // Set to idle
        
        // Disable combat
        enabled = false;
        
        // Visual transformation effect - blue ethereal color
        if (sr != null)
        {
            sr.color = new Color(0.5f, 0.8f, 1f, 0.7f);
        }
        
        // Could trigger dialogue here
        // DialogueManager.ShowDialogue("Ryo Spirit", "Jin... you've grown stronger...");
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && playerStats != null)
        {
            // Only deal damage during skill states
            int dmg = 0;
            
            if (currentState == 2) // Phantom Dash
                dmg = phantomDashDamage;
            else if (currentState == 4) // Sacrifice Mirror
                dmg = sacrificeMirrorDamage;
            // No damage during idle (0) or chase (1) states
            // Mirroring Ghost (3) handles damage internally
            
            if (dmg > 0)
                playerStats.TakeDamage(dmg);
        }
    }

    public override void TakeDamage(int dmg)
    {
        currentHealth -= dmg;
        
        // Flash effect when damaged
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
        // Transform into spirit before destroying
        TransformToSpirit();
        
        // Wait before cleanup to allow spirit transformation to be visible
        Invoke("DestroyRyo", 5f);
    }
    
    void DestroyRyo()
    {
        // Call base Die to handle combat zone unregistration and cleanup
        base.Die();
    }
}