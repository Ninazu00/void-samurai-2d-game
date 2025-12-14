using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Yuki : EnemyController
{
    public Transform target;
    private SpriteRenderer sr;
    protected override void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        currentHealth = maxHealth;
    }
    protected override void  EnemyBehavior()
    {
        transform.position = Vector3.MoveTowards(transform.position,target.transform.position,moveSpeed*Time.deltaTime);
        sr.flipX = (target.position.x < transform.position.x);
    }
}