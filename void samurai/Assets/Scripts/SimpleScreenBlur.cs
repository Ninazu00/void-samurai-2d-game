using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SimpleScreenBlur : MonoBehaviour
{
    public static SimpleScreenBlur instance;

    Image img;

    void Awake()
    {
        instance = this;
        img = GetComponent<Image>();
    }

    public void SetBlur(float amount)
    {
        Color c = img.color;
        c.a = amount;
        img.color = c;
    }
}
