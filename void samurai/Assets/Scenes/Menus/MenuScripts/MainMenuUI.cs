using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuUI : MonoBehaviour
{
    public void PlayGame()
    {
        SceneManager.LoadScene("Level0-Tutorial");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
