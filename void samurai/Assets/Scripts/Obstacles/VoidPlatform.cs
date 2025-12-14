using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VoidPlatform : MonoBehaviour
{
    public float visibleTime = 2f;
    public float hiddenTime = 2f;

    private SpriteRenderer sr;
    private Collider2D col;
    private float timer;
    private bool isVisible = true;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        col = GetComponent<Collider2D>();
        timer = visibleTime;
    }

    void Update()
    {
        timer -= Time.deltaTime;

        if (timer <= 0f)
        {
            isVisible = !isVisible;

            sr.enabled = isVisible;
            col.enabled = isVisible;

            timer = isVisible ? visibleTime : hiddenTime;
        }
    }
}