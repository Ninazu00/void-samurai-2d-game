using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance;
    public GameObject CurrentCheckpoint;
    public Transform Player;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public void SetCheckpoint(GameObject checkpoint)
    {
        CurrentCheckpoint = checkpoint;
    }

    public void RespawnPlayer()
    {
        Player.position = CurrentCheckpoint.transform.position;
        Player.GetComponent<PlayerStats>().HealFull();

        // Optional: reset other states like enemies or hazards
    }
}
