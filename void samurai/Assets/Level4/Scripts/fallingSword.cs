using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fallingSword : MonoBehaviour
{
    float timer = 0;
    public int damage;
    public Rigidbody2D rb; 
    // Start is called before the first frame update
    void Start()
    {
        
    }
    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= 0.5f)
        {
            rb.gravityScale = 15f;
        }

        if (timer >= 4f)
        {
            Destroy(gameObject);
        }
    }

    // Update is called once per frame
    void OnTriggerEnter2D(Collider2D other){
        if(other.tag == "Player"){
            FindObjectOfType<PlayerStats>().TakeDamage(damage);
        }
        else if(other.tag == "SolidObject")
        {
            rb.constraints = RigidbodyConstraints2D.FreezePositionY;
        }
    }
}
