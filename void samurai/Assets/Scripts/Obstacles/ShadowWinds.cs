using UnityEngine;

public class ShadowWinds : MonoBehaviour
{
    public Vector2 pushDirection = Vector2.left;
    public float pushStrength = 4f;
    public float maxWindSpeed = 7f;

    private void OnTriggerStay2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        Rigidbody2D rb = other.GetComponent<Rigidbody2D>();
        if (rb == null) return;

        float wind = pushDirection.normalized.x * pushStrength;

        rb.velocity = new Vector2(
            Mathf.Clamp(rb.velocity.x + wind, -maxWindSpeed, maxWindSpeed),
            rb.velocity.y
        );
    }
}
