using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Yuki : EnemyController
{
    public Transform target;
    private SpriteRenderer sr;
    Animator animator;
    bool Phase1 = true;
    float tempMoveSpeed;
    protected override void Start()
    {
        tempMoveSpeed = moveSpeed;
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        currentHealth = maxHealth;
    }
    protected override void  EnemyBehavior()
    {
        transform.position = new Vector3(Mathf.MoveTowards(transform.position.x, target.position.x, moveSpeed * Time.deltaTime), transform.position.y, 0f);
        sr.flipX = (target.position.x < transform.position.x);
    }
    void OnTriggerEnter2D(Collider2D other){
    if(other.tag == "Player"){
        attack();
    }
    }
    void attack()
    {
        animator.SetTrigger("mATK");
        FindObjectOfType<PlayerStats>().TakeDamage(damage);
    }
    public void shellOfWhatWas()
    {
        moveSpeed *= 1.1f;
        tempMoveSpeed = moveSpeed;
        FindObjectOfType<YukiAbilities>().enterPhase2();
        if(currentHealth<= (maxHealth / 2) && Phase1)
        {
            Phase1 = false;
            moveSpeed *= 1.1f;
            FindObjectOfType<YukiAbilities>().enterPhase2();
        }
        else
            return;
    }
    public void freezeForVoidBurst()
    {
        animator.SetTrigger("cATK");
        moveSpeed = 0f;
        rb.gravityScale = 0;
        rb.velocity = Vector2.zero;
        Invoke(nameof(unFreezeForVoidBurst), 1f);
    }
    void unFreezeForVoidBurst()
    {
        moveSpeed = tempMoveSpeed;
        rb.gravityScale = 1f;
    }
}