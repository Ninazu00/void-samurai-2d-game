using UnityEngine;
using UnityEngine.SceneManagement; 

public class LevelTransition: MonoBehaviour
{
    public string nextLevel;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            SceneManager.LoadScene(nextLevel);
        }
    }
}
