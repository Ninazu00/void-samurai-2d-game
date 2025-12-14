using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class WeaponEcho : MonoBehaviour
{
    public float moveDistance = 3f;
    public float moveSpeed = 4f;
    public int damage = 20;

    private Vector3 startPos;
    private Vector3 targetPos;
    private bool movingForward = true;

    private void Start()
    {
        startPos = transform.position;
        targetPos = startPos + transform.right * moveDistance;
    }

    private void Update()
    {
        MoveWeapon();
    }

    private void MoveWeapon()
    {
        if (movingForward)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPos, moveSpeed * Time.deltaTime);
            if (Vector3.Distance(transform.position, targetPos) < 0.01f)
                movingForward = false;
        }
        else
        {
            transform.position = Vector3.MoveTowards(transform.position, startPos, moveSpeed * Time.deltaTime);
            if (Vector3.Distance(transform.position, startPos) < 0.01f)
                movingForward = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayerStats playerStats = collision.GetComponent<PlayerStats>();
            if (playerStats != null)
                playerStats.TakeDamage(damage);
        }
        else if (collision.CompareTag("PlayerWeapon"))
        {
            Debug.Log("Weapon Echo parried the attack!");
        }
    }
}
