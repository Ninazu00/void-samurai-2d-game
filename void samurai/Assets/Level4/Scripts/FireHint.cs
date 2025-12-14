using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireHint : MonoBehaviour
{
    private SpriteRenderer sr;
    public float flickerSpeed;
    public float minAlpha;
    public float maxAlpha; 
    // Start is called before the first frame update
    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
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
