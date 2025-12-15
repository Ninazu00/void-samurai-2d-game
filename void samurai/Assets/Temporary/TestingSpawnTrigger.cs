using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestingSpawnTrigger : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void OnTriggerEnter2D(Collider2D other){
        if(other.tag == "Player")
        {
            FindObjectOfType<YukiAbilities>().worldAblaze();
            //FindObjectOfType<Yuki>().TakeDamage(60);
        }
    }
}
