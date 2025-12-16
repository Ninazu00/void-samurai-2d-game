using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStats : MonoBehaviour
{
    public int health = 100;
    public Slider slider;

    private SpriteRenderer sr;
    private Animator anim;
    private PlayerController pc;

    public bool isImmune = false;
    private float immunityTime = 0f;
    public float immunityDuration = 1.5f;
    public static int score = 0;

    private float flickerTime = 0f;
    public float flickerDuration = 0.1f;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        pc = GetComponent<PlayerController>();

        if (slider != null)
        {
            slider.maxValue = health;
            slider.value = health;
        }
        else
        {
            Debug.LogError("Health Slider is NOT assigned in PlayerStats");
        }
    }

    void Update()
    {
        if (isImmune)
        {
            SpriteFlicker();
            immunityTime += Time.deltaTime;

            if (immunityTime >= immunityDuration)
            {
                isImmune = false;
                sr.enabled = true;
            }
        }
    }

    public void TakeDamage(int damage)
    {
        if (isImmune || pc == null || pc.IsDead()) return;

        health -= damage;
        slider.value = health;

        if (health < 0) health = 0;

        // HIT animation
        anim.SetTrigger("hit");
        anim.SetBool("isHit", true);
        Invoke(nameof(EndHit), 0.2f);

        if (health == 0)
            {
                FindObjectOfType<LevelManager>().RespawnPlayer();
            }

        Debug.Log("Player Health: " + health);

        isImmune = true;
        immunityTime = 0f;
        Debug.Log("Player Health: " + health);
    }

    void EndHit()
    {
        anim.SetBool("isHit", false);
    }

    void SpriteFlicker()
    {
        if (flickerTime < flickerDuration)
        {
            flickerTime += Time.deltaTime;
        }
        else
        {
            sr.enabled = !sr.enabled;
            flickerTime = 0f;
        }
    }

    public void HealFull()
    {
        health = 100;
        if (slider != null)
            slider.value = health;
    }
}
