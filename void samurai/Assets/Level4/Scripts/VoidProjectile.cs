using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VoidProjectile : MonoBehaviour
{
    public int damage;
    public float speed;
    private Rigidbody2D rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        rb.velocity = -(transform.up * speed);
    }
    void OnTriggerEnter2D(Collider2D other){
    if(other.tag == "Player"){
        FindObjectOfType<PlayerStats>().TakeDamage(damage);
        FindObjectOfType<AudioManager>().playVoidBurst();
    }
    else if(other.tag == "SolidObject")
    {
        Destroy(gameObject);
    }
    }
}
