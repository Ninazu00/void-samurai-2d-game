using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fallingSword : MonoBehaviour
{
    public int damage;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void OnTriggerEnter2D(Collider2D other){
    if(other.tag == "Player"){
        FindObjectOfType<PlayerStats>().TakeDamage(damage);
    }
    }
}
