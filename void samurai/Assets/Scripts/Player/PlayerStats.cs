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
    public static int score = 0; // The score of the collected Void Steel Ore, it's equal to 0 now because this is the start.

    private float flickerTime = 0f;
    public float flickerDuration = 0.1f;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        pc = GetComponent<PlayerController>();

        slider.maxValue = health;
        slider.value = health;

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
        if (isImmune) return;

        health -= damage;
        slider.value = health;

        if (health < 0)
            health = 0;

        // -------- HIT ANIMATION --------
        anim.SetTrigger("hit");
        anim.SetBool("isHit", true);
        //pc.SetHitState(true);

        Invoke(nameof(EndHit), 0.2f);

        if (health == 0)
            {
                FindObjectOfType<DeathScreenUI>().ShowDeath();
            }

        Debug.Log("Player Health: " + health);

        isImmune = true;
        immunityTime = 0f;
    }

    void EndHit()
    {
        anim.SetBool("isHit", false);
        //pc.SetHitState(false);
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
