using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


// Credits for Script: https://www.youtube.com/@grove-of-gnomes @ https://www.youtube.com/watch?v=MNUYe0PWNNs&ab_channel=RehopeGames comment
public class PauseMenu : MonoBehaviour
{
    bool gamePaused = false;
    [SerializeField] GameObject pauseMenu;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && gamePaused == false)
        {
            Time.timeScale = 0;
            gamePaused = true;
            pauseMenu.SetActive(true);
        }
        else if ((Input.GetKeyDown(KeyCode.Escape) && gamePaused == true))
        {
            Time.timeScale = 1;
            gamePaused = false;
            pauseMenu.SetActive(false);
        }
    }

    public void LoadStartMenu()
    {
        SceneManager.LoadScene("StartMenu");
    }

    public void Continue()
    {
        Time.timeScale = 1;
        gamePaused = false;
        pauseMenu.SetActive(false);
    }

        public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Quit the game.");
    }
}