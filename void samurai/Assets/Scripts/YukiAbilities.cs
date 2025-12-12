using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YukiAbilities : MonoBehaviour
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
    void Update()
    {
        
    }
    
    void updateImages()
    {
        LspriteRenderer.sprite = LnewSprite;
        RspriteRenderer.sprite = RnewSprite;
    }
    void spawnSwords()
    {
        foreach (Transform sp in spawnPoints)
            {
                float randomZ = Random.Range(-20f, 20f);
                Quaternion randomRotation = Quaternion.Euler(0, 0, randomZ);
                Instantiate(swords, sp.position, randomRotation);
            }
    }
}
