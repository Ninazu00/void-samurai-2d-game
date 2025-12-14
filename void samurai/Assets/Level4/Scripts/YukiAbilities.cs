using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YukiAbilities : MonoBehaviour
{

    public Transform swords;
    public Transform[] swordSpawnPoints;
    public SpriteRenderer LspriteRenderer;
    public Sprite LnewSprite;  
    public SpriteRenderer RspriteRenderer;
    public Sprite RnewSprite;
    public Transform voidProjectile;
    public Transform spawnNovaDown;
    public Transform spawnNovaLeft;
    public Transform spawnNovaRight;
    public Transform spawnNovaDL;
    public Transform spawnNovaDR;
    public float fallingSwordsCD;
    float fallingSwordsTimer = 0;
    public float voidBurstCD;
    float voidBurstTimer = 0;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        fallingSwordsTimer += Time.deltaTime;
        voidBurstTimer += Time.deltaTime;
        if (fallingSwordsTimer >= fallingSwordsCD)
        {
            spawnSwords();
            fallingSwordsTimer = 0;
        }
        if (voidBurstTimer >= voidBurstCD)
        {
            spawnVoidBurst();
            voidBurstTimer = 0;
        }
    }
    
    public void updateImages()
    {
        LspriteRenderer.sprite = LnewSprite;
        RspriteRenderer.sprite = RnewSprite;
    }
    public void spawnSwords()
    {
        foreach (Transform sp in swordSpawnPoints)
            {
                float randomZ = Random.Range(-20f, 20f);
                Quaternion randomRotation = Quaternion.Euler(0, 0, randomZ);
                Instantiate(swords, sp.position, randomRotation);
            }
    }
    public void spawnVoidBurst()
    {
        
        Instantiate(voidProjectile, spawnNovaDown.position, spawnNovaDown.transform.rotation);
        Instantiate(voidProjectile, spawnNovaLeft.position, spawnNovaLeft.transform.rotation);
        Instantiate(voidProjectile, spawnNovaRight.position, spawnNovaRight.transform.rotation);
        for (int i = 0; i < 3; i++)
        {
            float randomZ = Random.Range(-70f, -120f);
            Quaternion randomRotation = Quaternion.Euler(0, 0, randomZ);
            Instantiate(voidProjectile, spawnNovaLeft.position, randomRotation);
        }
        for (int i = 0; i < 3; i++)
        {
            float randomZ = Random.Range(70f, 120f);
            Quaternion randomRotation = Quaternion.Euler(0, 0, randomZ);
            Instantiate(voidProjectile, spawnNovaRight.position, randomRotation);
        }
        for (int i = 0; i < 3; i++)
        {
            float randomZ = Random.Range(-45f, 45f);
            Quaternion randomRotation = Quaternion.Euler(0, 0, randomZ);
            Instantiate(voidProjectile, spawnNovaDown.position, randomRotation);
        }
        for (int i = 0; i < 50; i+=20)
        {
            float randomZ = Random.Range(-20f-i, -40f-i);
            Quaternion randomRotation = Quaternion.Euler(0, 0, randomZ);
            Instantiate(voidProjectile, spawnNovaDL.position, randomRotation);
        }
        for (int i = 0; i < 50; i+=20)
        {
            float randomZ = Random.Range(20f+i, 40f+i);
            Quaternion randomRotation = Quaternion.Euler(0, 0, randomZ);
            Instantiate(voidProjectile, spawnNovaDR.position, randomRotation);
        }

    }
}
