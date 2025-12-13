using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestingSpawnTrigger : MonoBehaviour
{
    public Transform swords;
    public Transform[] spawnPoints;
    public SpriteRenderer LspriteRenderer;
    public Sprite LnewSprite;  
    public SpriteRenderer RspriteRenderer;
    public Sprite RnewSprite;  
    // Start is called before the first frame update
    void Start()
    {
        
    }
    // Update is called once per frame
    void OnTriggerEnter2D(Collider2D other){
        if(other.tag == "Player")
        {
            FindObjectOfType<YukiAbilities>().updateImages();
            FindObjectOfType<YukiAbilities>().spawnSwords();
        }
    }
}
