using UnityEngine;
using UnityEngine.SceneManagement;
public class StartMenu : MonoBehaviour
{
        public void PlayGame()
    {
        // Load Starting Level
        SceneManager.LoadScene("MGDS");
    }

        public void LoadTutorial()
    {
        SceneManager.LoadScene("Tutorial");
    }

            public void LoadStartMenu()
    {
        SceneManager.LoadScene("StartMenu");
    }

                public void LoadCredits()
    {
        SceneManager.LoadScene("Credits");
    }


    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Quit the game.");
    }
}
