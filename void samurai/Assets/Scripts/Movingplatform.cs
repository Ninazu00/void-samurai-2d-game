using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movingplatform : MonoBehaviour
{
    public Transform PointA; // Right point
    public Transform PointB; // Left point

    public float speed;
    private bool AtoB;
    private Vector3 targetPosition;

    void Update()
    {
        if (AtoB)
        {
            targetPosition = PointA.position;
        }
        else
        {
            targetPosition = PointB.position;
        }

        Vector3 newPosition = transform.position;

        // Move on X axis instead of Y
        newPosition.x = Mathf.MoveTowards(
            transform.position.x,
            targetPosition.x,
            speed * Time.deltaTime
        );

        transform.position = newPosition;

        if (Mathf.Abs(transform.position.x - targetPosition.x) < 0.1f)
        {
            AtoB = !AtoB;
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.transform.SetParent(transform);
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.transform.SetParent(null);
        }
    }
}
