using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; // Needed to check scene name

public enum Stance
{
    Regret,      // Default
    Resolve,     // After Ryo
    Purification // Final Boss
}

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed;
    public float jumpHeight;
    public KeyCode Spacebar;
    public KeyCode L;
    public KeyCode R;

    [Header("Combat")]
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
    public int heavyDamage = 25;
    public float perfectParryWindow = 0.3f;

    private bool grounded;
    private bool isParrying;
    private bool perfectParryActive;

    [Header("Dash")]
    public float dashDistance = 5f;
    public float dashDuration = 0.2f;
    public float dashCooldown = 1f;
    private bool canDash = true;
    private bool isDashing = false;

    [Header("Attack Cooldowns")]
    bool canHeavyAttack = true;
    bool canLightAttack = true;
    public float lightAttackCooldown = 0.25f;
    public float heavyAttackCooldown = 0.6f;

    private bool isDead = false;

    private Animator anim;
    private Rigidbody2D rb;
    private SpriteRenderer sr;
    private EnemyController lastDamagedEnemy;

    private float groundedBuffer = 0.05f;
    private float groundedTimer;

    [Header("Stance System")]
    public Stance currentStance = Stance.Regret;
    public ParticleSystem resolveAura;
    public ParticleSystem purificationAura;

    // Unlock flags
    [Header("Stance Unlocks")]
    public bool resolveUnlocked = false;       // After Ryo
    public bool purificationUnlocked = false;  // Final Boss

    private float lightAttackRangeModifier = 1f;
    private float heavyAttackDamageModifier = 1f;
    private bool ryoGuidanceActive = false;

    void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();

        // Automatically unlock Purification if Level 4 is loaded
        if (SceneManager.GetActiveScene().name == "Level4")
        {
            UnlockPurificationStance();
        }

        ApplyStanceModifiers();
    }

    void Update()
    {
        if (isDead) return;

        // --- STANCE SWITCH ---
        if (Input.GetKeyDown(KeyCode.Z))
            CycleStance();

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

            AudioManager.Instance.PlayParry();

            Invoke(nameof(EndPerfectParry), perfectParryWindow);
            Invoke(nameof(EndParry), 0.35f);
        }

        if (isParrying || isDashing) return;

        // JUMP
        if (Input.GetKeyDown(Spacebar) && grounded)
            Jump();

        // MOVE
        float xVelocity = 0f;
        if (Input.GetKey(L)) xVelocity = -moveSpeed;
        else if (Input.GetKey(R)) xVelocity = moveSpeed;
        rb.velocity = new Vector2(xVelocity, rb.velocity.y);

        sr.flipX = Input.GetKey(L);

        anim.SetBool("isRunning", Input.GetKey(L) || Input.GetKey(R));

        // Smooth yVelocity
        float smoothY = Mathf.Lerp(anim.GetFloat("yVelocity"), rb.velocity.y, Time.deltaTime * 10f);
        anim.SetFloat("yVelocity", smoothY);
        anim.SetBool("isGrounded", groundedTimer > 0f);

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
        groundedTimer = grounded ? groundedBuffer : groundedTimer - Time.fixedDeltaTime;
    }

    void Jump()
    {
        rb.velocity = new Vector2(rb.velocity.x, jumpHeight);
        AudioManager.Instance.PlayJump();
    }

    void EndPerfectParry() => perfectParryActive = false;
    void EndParry() { isParrying = false; anim.SetBool("isParrying", false); }

    public void LightAttack()
    {
        float modifiedRange = lightAttackPoint != null ? lightAttackRange * lightAttackRangeModifier : lightAttackRange;
        lastDamagedEnemy = null;
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(lightAttackPoint.position, modifiedRange, Enemy);
        foreach (Collider2D enemy in hitEnemies)
        {
            EnemyController ec = enemy.GetComponent<EnemyController>();
            if (ec != null && ec != lastDamagedEnemy)
            {
                float damage = lightDamage * heavyAttackDamageModifier;
                ec.TakeDamage((int)damage);
                lastDamagedEnemy = ec;
            }
        }
        anim.SetTrigger("lightAttack");
        AudioManager.Instance.PlayLightSlash();
    }

    public void HeavyAttack()
    {
        float modifiedRange = heavyAttackPoint != null ? heavyAttackRange * lightAttackRangeModifier : heavyAttackRange;
        lastDamagedEnemy = null;
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(heavyAttackPoint.position, modifiedRange, Enemy);
        foreach (Collider2D enemy in hitEnemies)
        {
            EnemyController ec = enemy.GetComponent<EnemyController>();
            if (ec != null && ec != lastDamagedEnemy)
            {
                float damage = heavyDamage * heavyAttackDamageModifier;
                ec.TakeDamage((int)damage);
                lastDamagedEnemy = ec;
            }
        }
        anim.SetTrigger("heavyAttack");
        AudioManager.Instance.PlayHeavySlash();
    }

    void ResetLightAttack() => canLightAttack = true;
    void ResetHeavyAttack() => canHeavyAttack = true;

    private IEnumerator PerformDash(float direction)
    {
        canDash = false;
        isDashing = true;
        float originalGravity = rb.gravityScale;
        rb.gravityScale = 0;
        rb.velocity = new Vector2(direction * dashDistance / dashDuration, 0f);
        AudioManager.Instance.PlayDash();
        yield return new WaitForSeconds(dashDuration);
        rb.velocity = Vector2.zero;
        rb.gravityScale = originalGravity;
        isDashing = false;
        yield return new WaitForSeconds(dashCooldown);
        canDash = true;
    }

    // ---------------- DEATH ----------------
    public void Die()
    {
        if (isDead) return;
        isDead = true;

        rb.velocity = Vector2.zero;
        rb.isKinematic = true; // freeze physics
        canLightAttack = false;
        canHeavyAttack = false;
        canDash = false;

        anim.Play("Death");
        GetComponent<Collider2D>().enabled = false;

        StartCoroutine(DeathAndRespawn());
    }

    private IEnumerator DeathAndRespawn()
    {
        float deathAnimLength = 1f;
        foreach (AnimationClip clip in anim.runtimeAnimatorController.animationClips)
            if (clip.name == "Death") deathAnimLength = clip.length;

        yield return new WaitForSeconds(deathAnimLength);

        LevelManager.Instance.RespawnPlayer();

        rb.isKinematic = false; // restore physics
        isDead = false;
        canLightAttack = true;
        canHeavyAttack = true;
        canDash = true;
        GetComponent<Collider2D>().enabled = true;

        anim.Rebind();
        anim.Update(0f);
    }

    private void OnDrawGizmos()
    {
        if (lightAttackPoint != null)
            Gizmos.DrawWireSphere(lightAttackPoint.position, lightAttackRange);
        if (heavyAttackPoint != null)
            Gizmos.DrawWireSphere(heavyAttackPoint.position, heavyAttackRange);
    }

    // ---------------- STANCE SYSTEM ----------------
    void CycleStance()
    {
        if (currentStance == Stance.Regret)
        {
            if (resolveUnlocked)
                currentStance = Stance.Resolve;
            else
                return;
        }
        else if (currentStance == Stance.Resolve)
        {
            if (purificationUnlocked)
                currentStance = Stance.Purification;
            else
                currentStance = Stance.Regret;
        }
        else // Purification
        {
            currentStance = Stance.Regret;
        }

        ApplyStanceModifiers();
        Debug.Log("Current Stance: " + currentStance);
    }

    void ApplyStanceModifiers()
    {
        if (resolveAura != null) resolveAura.Stop();
        if (purificationAura != null) purificationAura.Stop();

        switch (currentStance)
        {
            case Stance.Regret:
                sr.color = Color.white;
                lightAttackRangeModifier = 1.2f;
                heavyAttackDamageModifier = 1f;
                ryoGuidanceActive = false;
                break;

            case Stance.Resolve:
                sr.color = new Color(0.2f, 0.2f, 0.2f); // dark grey/black
                if (resolveAura != null) resolveAura.Play();
                lightAttackRangeModifier = 1f;
                heavyAttackDamageModifier = 1.2f;
                ryoGuidanceActive = true;
                break;

            case Stance.Purification:
                sr.color = new Color(1f, 0.85f, 0f); // gold
                if (purificationAura != null) purificationAura.Play();
                lightAttackRangeModifier = 1f;
                heavyAttackDamageModifier = 1.5f;
                ryoGuidanceActive = true;
                break;
        }
    }

    // ---------------- STANCE UNLOCK METHODS ----------------
    public void UnlockResolveStance()
    {
        resolveUnlocked = true;
        Debug.Log("Resolve Stance unlocked!");
    }

    public void UnlockPurificationStance()
    {
        purificationUnlocked = true;
        Debug.Log("Purification Stance unlocked!");
    }

    public void SetInputEnabled(bool enabled)
    {
        isDead = !enabled;
        if (!enabled) rb.velocity = Vector2.zero;
    }

    public bool IsParrying() => isParrying;
    public bool IsDead() => isDead;
    public bool IsPerfectParryActive() => perfectParryActive;
}
