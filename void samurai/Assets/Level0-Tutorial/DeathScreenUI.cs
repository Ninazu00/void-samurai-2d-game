using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DeathScreenUI : MonoBehaviour
{
    public GameObject panel;
    private PlayerController player;

    void Start()
    {
        panel.SetActive(false);
        player = FindObjectOfType<PlayerController>();
    }

    public void ShowDeath()
    {
        panel.SetActive(true);
        Time.timeScale = 0f;
        player.enabled = false;
    }

    public void Respawn()
    {
        Time.timeScale = 1f;

        FindObjectOfType<LevelManager>().RespawnPlayer();
        player.enabled = true;

        panel.SetActive(false);
    }

    public void GoToMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }
}
