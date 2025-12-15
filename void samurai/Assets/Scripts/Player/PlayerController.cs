using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    public float moveSpeed; //how fast the character moves
    public float jumpHeight; //how high the character jumps
    public KeyCode Spacebar; //Jump key
    public KeyCode L; //Left movement key
    public KeyCode R; //Right movement key
    public KeyCode LightAttackKey; //Light attack key
    public KeyCode HeavyAttackKey; //Heavy attack key
    public KeyCode ParryKey; //Parry key
    public Transform groundCheck; //ground check position
    public float groundCheckRadius; //ground check radius
    public LayerMask whatIsGround; //what is considered ground

    public Transform lightAttackPoint; //point for light attack
    public float lightAttackRange = 0.5f; //light attack radius
    public Transform heavyAttackPoint; //point for heavy attack
    public float heavyAttackRange = 0.7f; //heavy attack radius
    public LayerMask Enemy; //layers considered as enemies (capital E)
    public int lightDamage = 10; //light attack damage
    public int heavyDamage = 25; //heavy attack damage

    private bool grounded; //is player on ground
    private bool isParrying; //is player parrying
    private bool isHit; //is player taking damage
    private Animator anim;
    private Rigidbody2D rb; //reference to Rigidbody2D for velocity access

    void Start () {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>(); //get Rigidbody2D component
    }

    void Update () {

        // Prevent movement and actions while taking hit
        if (isHit)
        {
            rb.velocity = Vector2.zero;
            return;
        }

        // ----------- PARRY -----------
        if (Input.GetKeyDown(ParryKey) && grounded && !isParrying)
        {
            isParrying = true; //lock player state
            rb.velocity = Vector2.zero; //stop movement instantly
            anim.SetBool("isParrying", true); //set parry state
            anim.SetTrigger("parry"); //play parry animation
            Invoke(nameof(EndParry), 0.25f); //end parry after animation
        }

        // Prevent movement and actions while parrying
        if (isParrying)
        {
            return;
        }

        // ----------- JUMP -----------
        if(Input.GetKeyDown(Spacebar) && grounded)
        {
            Jump();  
        }

        // ----------- MOVE LEFT -----------
        if (Input.GetKey(L))
        {
            rb.velocity = new Vector2(-moveSpeed, rb.velocity.y);
            GetComponent<SpriteRenderer>().flipX = true;
        }

        // ----------- MOVE RIGHT -----------
        if (Input.GetKey(R))
        {
            rb.velocity = new Vector2(moveSpeed, rb.velocity.y);
            GetComponent<SpriteRenderer>().flipX = false;
        }

        // Stop sliding when no movement key is pressed
        if (!Input.GetKey(L) && !Input.GetKey(R))
        {
            rb.velocity = new Vector2(0f, rb.velocity.y);
        }

        // ----------- ANIMATION (Idle <-> Run) -----------
        anim.SetBool("isRunning", Input.GetKey(L) || Input.GetKey(R));

        // ----------- ANIMATION (Jump & Fall Blend Tree) -----------
        anim.SetFloat("yVelocity", rb.velocity.y); //send Y velocity to blend tree
        anim.SetBool("isGrounded", grounded); //tell animator if player is on ground

        // ----------- ANIMATION (Light & Heavy Attacks) -----------
        if (Input.GetKeyDown(LightAttackKey))
            anim.SetTrigger("lightAttack"); //trigger light slash animation

        if (Input.GetKeyDown(HeavyAttackKey))
            anim.SetTrigger("heavyAttack"); //trigger heavy slash animation
    }

    void FixedUpdate()
    {
        grounded = Physics2D.OverlapCircle(
            groundCheck.position,
            groundCheckRadius,
            whatIsGround
        ); //this statement calculates when exactly the character is considered to be standing on the ground
    }

    void Jump()
    {
        rb.velocity = new Vector2(rb.velocity.x, jumpHeight); //player jumps vertically
    }

    void EndParry()
    {
        isParrying = false; //unlock player state
        anim.SetBool("isParrying", false); //return to idle/run
    }

    // ----------- ATTACK FUNCTIONS -----------

    // Called from LightAttack animation event
    public void LightAttack()
    {
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(
            lightAttackPoint.position,
            lightAttackRange,
            Enemy
        );

        foreach (Collider2D enemy in hitEnemies)
        {
            EnemyController ec = enemy.GetComponent<EnemyController>();
            if (ec != null)
                ec.TakeDamage(lightDamage);
        }
    }

    // Called from HeavyAttack animation event
    public void HeavyAttack()
    {
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(
            heavyAttackPoint.position,
            heavyAttackRange,
            Enemy
        );

        foreach (Collider2D enemy in hitEnemies)
        {
            EnemyController ec = enemy.GetComponent<EnemyController>();
            if (ec != null)
                ec.TakeDamage(heavyDamage);
        }
    }

    // ----------- HIT CONTROL -----------

    public void SetHitState(bool value)
    {
        isHit = value;
    }

    // Visualize the attack ranges in the Scene view
    private void OnDrawGizmos()
    {
        if (lightAttackPoint != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(lightAttackPoint.position, lightAttackRange);
        }

        if (heavyAttackPoint != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(heavyAttackPoint.position, heavyAttackRange);
        }
    }
}
