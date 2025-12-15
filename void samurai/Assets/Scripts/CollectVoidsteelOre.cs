using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class CollectVoidsteelOre : MonoBehaviour
{
    //public AudioClip voidOreSound;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    // Item Pickup
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            PlayerStats.score++;
            //AudioManager.Instance.PlayMusicSFX(voidOreSound);
            Destroy(gameObject);
        }
    }
}
