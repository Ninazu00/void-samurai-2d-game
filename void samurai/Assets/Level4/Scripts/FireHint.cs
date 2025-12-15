using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireHint : MonoBehaviour
{
    private SpriteRenderer sr;
    public float flickerSpeed;
    public float minAlpha;
    public float maxAlpha; 
    float timer = 0;
    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= 5f)
        {
            Destroy(gameObject);
        }
        SpriteFlicker();
    }
    void SpriteFlicker()
    {
        float alpha = Mathf.Lerp(minAlpha, maxAlpha, (Mathf.Sin(Time.time * flickerSpeed) + 1f) / 2f);

        Color c = sr.color;
        c.a = alpha;
        sr.color = c;
    }
}
