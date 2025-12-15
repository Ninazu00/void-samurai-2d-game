using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance;
    public GameObject CurrentCheckpoint;
    public Transform Player;

    // Start is called before the first frame update
    void Start()
    {
        if (Player == null)
        {
            GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
            if (playerObj != null)
                Player = playerObj.transform;
            else
                Debug.LogError("Player not found in scene");
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void Awake()
    {
        Instance = this;
    }
    

    public void SetCheckpoint(GameObject checkpoint)
    {
        CurrentCheckpoint = checkpoint;
    }

    public void RespawnPlayer()
    {
        Player.position = CurrentCheckpoint.transform.position;
        Player.GetComponent<PlayerStats>().HealFull();

        // Enemies and hazards can be reset here later
    }
}
