using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class PlayerStats : MonoBehaviour
{
    public int health = 100;
    public Slider slider;
    private float flickerTime = 0f;
    public float flickerDuration = 0.1f;

    private SpriteRenderer sr;

    public bool isImmune = false;
    private float immunityTime = 0f;
    public float immunityDuration = 1.5f;


    // Start is called before the first frame update
    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        slider.maxValue = health;
        slider.value = health;
    }

    // Update is called once per frame
    void Update()
    {
        if(isImmune == true)
        {
            SpriteFlicker();
            immunityTime+=Time.deltaTime;
            if (immunityTime >= immunityDuration)
            {
                isImmune = false;
                sr.enabled= true;
            }
        }
    }
    public void TakeDamage(int damage)
    {
        if (isImmune == false)
        {

        health -= damage;
        slider.value = health;
        if (health < 0)
        health = 0;
        if (health == 0)
        {
            FindObjectOfType<LevelManager>().RespawnPlayer();
        }
        Debug.Log("Player Health:" + health.ToString());;
        }
        isImmune=true;
        immunityTime=0f;
    }
    void SpriteFlicker()
    {
        if (flickerTime < flickerDuration)
        {
            flickerTime += Time.deltaTime;
        }
        else if (flickerTime >= flickerDuration)
        {
            sr.enabled = !(sr.enabled);
            flickerTime = 0;
        }
    }
}
