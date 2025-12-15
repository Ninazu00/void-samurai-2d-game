using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShadowWinds : MonoBehaviour
{
 public Vector2 pushDirection = Vector2.left;
    public float pushStrength = 5f;

    void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Rigidbody2D rb = other.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.AddForce(pushDirection.normalized * pushStrength);
            }
        }
    }
}
