using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlacksmithNPC : MonoBehaviour
{
    public float interactionRange = 2f;
    public int healthUpgrade = 20;
    public int levelNumber = 1;
    private Transform player;
    private PlayerStats playerStats;
    private bool playerInRange = false;
    private bool hasUpgraded = false;
    
    public static int checkpointsVisited = 0;

    void Start()
    {
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            player = playerObj.transform;
            playerStats = player.GetComponent<PlayerStats>();
        }
        gameObject.SetActive(false);
    }

    void Update()
    {
        if (player == null) return;

        if (!gameObject.activeSelf)
        {
            if (checkpointsVisited >= levelNumber)
            {
                gameObject.SetActive(true);
            }
            return;
        }
        
        float dist = Vector2.Distance(transform.position, player.position);
        playerInRange = dist <= interactionRange;
        
        if (playerInRange && Input.GetKeyDown(KeyCode.E))
        {
            Upgrade();
        }
    }
    void Upgrade()
    {
        if (playerStats != null && !hasUpgraded)
        {
            playerStats.health += healthUpgrade;
            hasUpgraded = true;
            Debug.Log("Upgraded! New health: " + playerStats.health);
        }
        else if (hasUpgraded)
        {
            Debug.Log("Already upgraded in this level!");
        }
    }
}
