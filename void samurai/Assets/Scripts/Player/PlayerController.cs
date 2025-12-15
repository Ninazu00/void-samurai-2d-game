using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed;
    public float jumpHeight;
    public KeyCode Spacebar;
    public KeyCode L;
    public KeyCode R;
    public KeyCode LightAttackKey;
    public KeyCode HeavyAttackKey;
    public KeyCode ParryKey;
    public KeyCode DashKey = KeyCode.Q;
    public Transform groundCheck;
    public float groundCheckRadius;
    public LayerMask whatIsGround;

    public Transform lightAttackPoint; 
    public float lightAttackRange = 0.5f; 
    public Transform heavyAttackPoint; 
    public float heavyAttackRange = 0.7f; 
    public LayerMask Enemy; 
    public LayerMask Barrel; 
    public int lightDamage = 10; 
    public int heavyDamage = 15;
    public float lightAttackCooldown = 0.4f;
    public float heavyAttackCooldown = 0.8f;
    private bool canLightAttack = true;
    private bool canHeavyAttack = true;

    public float perfectParryWindow = 0.2f;
    private bool grounded;
    private bool isParrying;
    private bool perfectParryActive;

    public float dashDistance = 5f;
    public float dashDuration = 0.2f;
    public float dashCooldown = 1f;
    private bool canDash = true;
    private bool isDashing = false;

    private bool isDead = false;

    private Animator anim;
    private Rigidbody2D rb;

    private EnemyController lastDamagedEnemy;

    void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (isDead) return;

        // DASH
        if (Input.GetKeyDown(DashKey) && canDash && !isDashing)
        {
            float dashDirection = Input.GetKey(L) ? -1f : Input.GetKey(R) ? 1f : 0f;
            if (dashDirection != 0f)
                StartCoroutine(PerformDash(dashDirection));
        }

        // PARRY
        if (Input.GetKeyDown(ParryKey) && grounded && !isParrying && !isDashing)
        {
            isParrying = true;
            perfectParryActive = true;
            rb.velocity = Vector2.zero;
            anim.SetBool("isParrying", true);
            anim.SetTrigger("parry");

            Invoke(nameof(EndPerfectParry), perfectParryWindow);
            Invoke(nameof(EndParry), 0.35f);
        }

        if (isParrying || isDashing) return;

        // JUMP
        if (Input.GetKeyDown(Spacebar) && grounded)
            Jump();

        // MOVE
        if (Input.GetKey(L))
        {
            rb.velocity = new Vector2(-moveSpeed, rb.velocity.y);
            if (GetComponent<SpriteRenderer>() != null)
                GetComponent<SpriteRenderer>().flipX = true;
        }
        else if (Input.GetKey(R))
        {
            rb.velocity = new Vector2(moveSpeed, rb.velocity.y);
            if (GetComponent<SpriteRenderer>() != null)
                GetComponent<SpriteRenderer>().flipX = false;
        }
        else
        {
            rb.velocity = new Vector2(0f, rb.velocity.y);
        }

        anim.SetBool("isRunning", Input.GetKey(L) || Input.GetKey(R));
        anim.SetFloat("yVelocity", rb.velocity.y);
        anim.SetBool("isGrounded", grounded);

        // ATTACK INPUTS
        if (Input.GetKeyDown(LightAttackKey) && canLightAttack)
        {
            canLightAttack = false;
            LightAttack();
            Invoke(nameof(ResetLightAttack), lightAttackCooldown);
        }

        if (Input.GetKeyDown(HeavyAttackKey) && canHeavyAttack)
        {
            canHeavyAttack = false;
            HeavyAttack();
            Invoke(nameof(ResetHeavyAttack), heavyAttackCooldown);
        }
    }

    void FixedUpdate()
    {
        grounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, whatIsGround);
    }

    void Jump()
    {
        rb.velocity = new Vector2(rb.velocity.x, jumpHeight);
    }

    void EndPerfectParry() => perfectParryActive = false;
    void EndParry() { isParrying = false; anim.SetBool("isParrying", false); }

    // ATTACK FUNCTIONS
    public void LightAttack()
    {
        lastDamagedEnemy = null;
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(lightAttackPoint.position, lightAttackRange, Enemy);

        foreach (Collider2D enemy in hitEnemies)
        {
            EnemyController ec = enemy.GetComponent<EnemyController>();
            if (ec != null && ec != lastDamagedEnemy)
            {
                ec.TakeDamage(lightDamage);
                lastDamagedEnemy = ec;
            }
        }
        Collider2D[] hitBarrels = Physics2D.OverlapCircleAll(
            lightAttackPoint.position,
            lightAttackRange,
            Barrel
        );

        foreach (Collider2D barrelCol in hitBarrels)
        {
            BarrelDestroyer barrel = barrelCol.GetComponent<BarrelDestroyer>();
            if (barrel != null)
                barrel.BarrelDamage();
        }


        anim.SetTrigger("lightAttack");
    }

    public void HeavyAttack()
    {
        lastDamagedEnemy = null;
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(heavyAttackPoint.position, heavyAttackRange, Enemy);

        foreach (Collider2D enemy in hitEnemies)
        {
            EnemyController ec = enemy.GetComponent<EnemyController>();
            if (ec != null && ec != lastDamagedEnemy)
            {
                ec.TakeDamage(heavyDamage);
                lastDamagedEnemy = ec;
            }
        }
        Collider2D[] hitBarrels = Physics2D.OverlapCircleAll(
            heavyAttackPoint.position,
            heavyAttackRange,
            Barrel
        );

        foreach (Collider2D barrelCol in hitBarrels)
        {
            BarrelDestroyer barrel = barrelCol.GetComponent<BarrelDestroyer>();
            if (barrel != null)
                barrel.BarrelDamage();
        }


        anim.SetTrigger("heavyAttack");
    }

    void ResetLightAttack() => canLightAttack = true;
    void ResetHeavyAttack() => canHeavyAttack = true;

    // DASH
    private IEnumerator PerformDash(float direction)
    {
        canDash = false;
        isDashing = true;
        float originalGravity = rb.gravityScale;
        rb.gravityScale = 0;
        rb.velocity = new Vector2(direction * dashDistance / dashDuration, 0f);

        yield return new WaitForSeconds(dashDuration);

        rb.velocity = Vector2.zero;
        rb.gravityScale = originalGravity;
        isDashing = false;

        yield return new WaitForSeconds(dashCooldown);
        canDash = true;
    }

    // DEATH
    public void Die()
    {
        if (isDead) return;
        isDead = true;

        rb.velocity = Vector2.zero;
        canLightAttack = false;
        canHeavyAttack = false;
        canDash = false;

        anim.SetTrigger("death");
        GetComponent<Collider2D>().enabled = false;

        Invoke(nameof(GameOver), 2f);
    }

    private void GameOver()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(
            UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex
        );
    }

    private void OnDrawGizmos()
    {
        if (lightAttackPoint != null)
            Gizmos.DrawWireSphere(lightAttackPoint.position, lightAttackRange);
        if (heavyAttackPoint != null)
            Gizmos.DrawWireSphere(heavyAttackPoint.position, heavyAttackRange);
    }

    public bool IsParrying() => isParrying;
    public bool IsPerfectParryActive() => perfectParryActive;
}