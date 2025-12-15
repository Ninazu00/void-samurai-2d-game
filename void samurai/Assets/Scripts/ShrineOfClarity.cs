using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShrineOfClarity : MonoBehaviour
{
    public KeyCode interactKey = KeyCode.E;
    private bool playerInRange;
    private bool isActivated = false;
    private Animator animator;
    public CombatZone combatZone;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>(); // finds the Animator on this shrine
    }

    // Update is called once per frame
    void Update()
    {
       if (playerInRange && !isActivated && Input.GetKeyDown(interactKey)) // check if the player is trying to activate the shrine
        {
            TryActivateShrine();
        }
    }

    private void TryActivateShrine()
    {
        if (isActivated) // if the shrine already activated, do nothing
            return;

        if (combatZone != null && !combatZone.IsCleared)
            return;


        // heal player and set checkpoint
        PlayerStats playerStats = FindObjectOfType<PlayerStats>();
        playerStats.HealFull();

        LevelManager.Instance.SetCheckpoint(gameObject); // makes the shrine as a check point

        isActivated = true;

        // Play the animation
        if (animator != null)
            animator.SetTrigger("Activate");
    }

    public bool IsActivated
    {
        get { return isActivated; }
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
            playerInRange = true;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
            playerInRange = false;
    }
}
