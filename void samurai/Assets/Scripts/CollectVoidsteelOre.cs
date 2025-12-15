using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class CollectVoidsteelOre : MonoBehaviour
{
    public AudioClip voidOreSound1;
    public AudioClip voidOreSound2;
    //public AudioClip voidOreSound3;
    //public AudioClip voidOreSound4;
    public AudioClip voidOreSound3;
    //public AudioClip voidOreSound6;
    public AudioClip voidOreSound4;
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
            FindObjectOfType<PlayerStats>().score++;
            FindObjectOfType<AudioManager>().PlayRandomSFX(voidOreSound1, voidOreSound2, voidOreSound3, voidOreSound4);
            Destroy(gameObject);
        }
    }
}
