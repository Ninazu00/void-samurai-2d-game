using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashHintAppearance : MonoBehaviour
{
    [SerializeField] private SpriteRenderer hintSprite;

    private void Start()
    {
        if (hintSprite != null)
            hintSprite.enabled = false;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            hintSprite.enabled = true;
        }
    }
}