using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YukiAbilities : MonoBehaviour
{
    public Transform Yuki;
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
    public Transform fireee;
    public Transform fireeeWarning;
    public float fallingSwordsCD;
    float fallingSwordsTimer = 0;
    public float voidBurstCD;
    float voidBurstTimer = 0;
    public float worldAblazeCD;
    private float worldAblazeWarningTimer = 0;
    private float worldAblazeWarningCD;
    float worldAblazeTimer = 0;
    public float voidlingChance;
    public Transform voidling;
    // Start is called before the first frame update
    void Start()
    {
        worldAblazeWarningCD = worldAblazeCD -5;
        //Phase one music
        FindObjectOfType<AudioManager>().playYukiOne();
        FindObjectOfType<AudioManager>().playVoidDrownYou();
    }

    // Update is called once per frame
    void Update()
    {
        fallingSwordsTimer += Time.deltaTime;
        voidBurstTimer += Time.deltaTime;
        worldAblazeTimer += Time.deltaTime;
        worldAblazeWarningTimer += Time.deltaTime;
        if (fallingSwordsTimer >= fallingSwordsCD)
        {
            spawnSwords();
            fallingSwordsTimer = 0;
        }
        if (voidBurstTimer >= voidBurstCD)
        {
            Yuki.position = transform.position;
            FindObjectOfType<Yuki>().freezeForVoidBurst();
            Invoke(nameof(spawnVoidBurst), 1f);
            voidBurstTimer = 0;
        }
        if (worldAblazeWarningTimer >= worldAblazeWarningCD)
        {
            worldAblazeWarning();
            worldAblazeWarningTimer = 0;
        }
        if (worldAblazeTimer >= worldAblazeCD)
        {
            worldAblaze();
            worldAblazeTimer = 0;
            worldAblazeWarningTimer = 0;
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
        FindObjectOfType<AudioManager>().playYukiLaugh();
        Instantiate(voidProjectile, spawnNovaDown.position, spawnNovaDown.transform.rotation);
        Instantiate(voidProjectile, spawnNovaLeft.position, spawnNovaLeft.transform.rotation);
        Instantiate(voidProjectile, spawnNovaRight.position, spawnNovaRight.transform.rotation);
        for (int i = 0; i < 5; i++)
        {
            float randomZ = Random.Range(-70f, -120f);
            Quaternion randomRotation = Quaternion.Euler(0, 0, randomZ);
            Instantiate(voidProjectile, spawnNovaLeft.position, randomRotation);
        }
        for (int i = 0; i < 5; i++)
        {
            float randomZ = Random.Range(70f, 120f);
            Quaternion randomRotation = Quaternion.Euler(0, 0, randomZ);
            Instantiate(voidProjectile, spawnNovaRight.position, randomRotation);
        }
        for (int i = 0; i < 5; i++)
        {
            float randomZ = Random.Range(-45f, 45f);
            Quaternion randomRotation = Quaternion.Euler(0, 0, randomZ);
            Instantiate(voidProjectile, spawnNovaDown.position, randomRotation);
        }
        for (int i = 0; i < 100; i+=20)
        {
            float randomZ = Random.Range(-20f-i, -40f-i);
            Quaternion randomRotation = Quaternion.Euler(0, 0, randomZ);
            Instantiate(voidProjectile, spawnNovaDL.position, randomRotation);
        }
        for (int i = 0; i < 100; i+=20)
        {
            float randomZ = Random.Range(20f+i, 40f+i);
            Quaternion randomRotation = Quaternion.Euler(0, 0, randomZ);
            Instantiate(voidProjectile, spawnNovaDR.position, randomRotation);
        }
    }
    public void worldAblaze()
    {
        FindObjectOfType<AudioManager>().playWorldAblaze();
        for (float i = -14.33f; i < 15f; i += 1f)
        {
            Quaternion noRotation = Quaternion.Euler(0, 0, 0);
            Vector3 spawnPosition = new Vector3(i, -2.5f, 0f);
            Instantiate(fireee, spawnPosition, noRotation);
            float roll = Random.Range(0f, 100f);
            if (roll < voidlingChance)
            {
                Instantiate(voidling, spawnPosition, noRotation);
            }
        }
    }
    public void worldAblazeWarning()
    {
        for (float i = -14.33f; i < 15f; i += 1f)
        {
            Quaternion noRotation = Quaternion.Euler(0, 0, 0);
            Vector3 spawnPosition = new Vector3(i, -2.5f, 0f);
            Instantiate(fireeeWarning, spawnPosition, noRotation);
        }
    }
    public void enterPhase2()
    {
        FindObjectOfType<AudioManager>().playYukiTwo();
        fallingSwordsCD = 25;
        fallingSwordsTimer = 0;
        voidBurstCD = 20;
        voidBurstTimer = 0;
        spawnSwords();
        updateImages();
        Yuki.position = transform.position;
        FindObjectOfType<Yuki>().freezeForVoidBurst();
        Invoke(nameof(spawnVoidBurst), 1f);
    }
}
