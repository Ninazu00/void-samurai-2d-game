using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DarknessFade : MonoBehaviour
{
    public float fadeDuration = 2f;

    private SpriteRenderer sr;

    void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    void OnEnable()
    {
        StartCoroutine(FadeOut());
    }

    System.Collections.IEnumerator FadeOut()
    {
        Color c = sr.color;
        float startAlpha = c.a;
        float time = 0f;

        while (time < fadeDuration)
        {
            time += Time.deltaTime;
            float t = time / fadeDuration;
            c.a = Mathf.Lerp(startAlpha, 0f, t);
            sr.color = c;
            yield return null;
        }

        c.a = 0f;
        sr.color = c;
    }
}

