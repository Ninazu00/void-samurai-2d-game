using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlacksmithNPC : MonoBehaviour
{
    public float interactionRange = 2f;
    public int requiredVoidsteelOre = 3; // Require 3 ore per upgrade
    public int lightDamageIncrease = 2; // Increase light attack by 2
    public int heavyDamageIncrease = 5; // Increase heavy attack by 5
    public int healthIncrease = 25; // Increase health by 25
    public int maxUpgrades = 3; // Maximum of 3 upgrades allowed
    
    private Transform player;
    private PlayerStats playerStats;
    private PlayerController playerController;
    private int upgradesUsed = 0; // Track number of upgrades already used

    void Start()
    {
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            player = playerObj.transform;
            playerStats = player.GetComponent<PlayerStats>();
            playerController = player.GetComponent<PlayerController>();
        }
        
        // Blacksmith is always active (normal NPC)
        gameObject.SetActive(true);
    }

    void Update()
    {
        if (player == null) return;
        
        // Check if player is in interaction range
        float dist = Vector2.Distance(transform.position, player.position);
        
        // Check all upgrade conditions
        if (dist <= interactionRange && Input.GetKeyDown(KeyCode.E))
        {
            if (upgradesUsed >= maxUpgrades)
            {
                Debug.Log($"Maximum upgrades ({maxUpgrades}) already used!");
            }
            else if (PlayerStats.score < requiredVoidsteelOre)
            {
                Debug.Log($"Need {requiredVoidsteelOre - PlayerStats.score} more Voidsteel Ore to upgrade!");
            }
            else
            {
                Upgrade();
            }
        }
    }

    void Upgrade()
    {
        if (playerStats != null && playerController != null && upgradesUsed < maxUpgrades)
        {
            // Deduct the ore cost
            PlayerStats.score -= requiredVoidsteelOre;
            
            // Increase health
            playerStats.health += healthIncrease;
            
            // Update health slider UI
            if (playerStats.slider != null)
            {
                playerStats.slider.maxValue = playerStats.health;
                playerStats.slider.value = playerStats.health;
            }
            
            // Increase attack damage
            playerController.lightDamage += lightDamageIncrease;
            playerController.heavyDamage += heavyDamageIncrease;
            
            upgradesUsed++;
            
            Debug.Log($"Upgraded! Health: {playerStats.health}, Light Damage: {playerController.lightDamage}, Heavy Damage: {playerController.heavyDamage}");
            Debug.Log($"Upgrades remaining: {maxUpgrades - upgradesUsed}");
            
            // Notify when all upgrades are used
            if (upgradesUsed >= maxUpgrades)
            {
                Debug.Log("All upgrades have been used!");
            }
        }
    }
}

