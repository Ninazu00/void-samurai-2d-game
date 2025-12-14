using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Yuki : EnemyController
{
    public Transform target;
    private SpriteRenderer sr;
    Animator animator;
    protected override void Start()
    {
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
    void shellOfWhatWas()
    {
        if(currentHealth<= (maxHealth / 2))
        {
            
        }
    }
}