//using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    /* Spieler Lebensanzahl */

    public int g_playerMaxHP;
    [SerializeField] public int g_playerHP;


    /* Text der Lebensanzahl */
    [SerializeField] private PlayerHP g_hpText;

    /* HP Bar */
    [SerializeField] private HP_Bar hp_bar;

    [SerializeField] GameObject pauseMenu;
    bool gamePaused = false;


    void Start()
    {
        g_playerHP = g_playerMaxHP;
        g_hpText.UpdateText(g_playerHP);
    }


    /**
    * Der Spieler wird getroffen und verliert Leben.
    * @param dmg - Schadensanzahl
    */
    public void OnHit(int dmg)
    {
        // Leben abziehen
        g_playerHP -= dmg;
        Debug.Log("Player says Ouch!");

        // Spieler auf/unter 0 Leben stirbt
        if (g_playerHP <= 0)
        {
            Debug.Log("Spieler gestorben!");
            // Level neuladen
            string currentSceneName = SceneManager.GetActiveScene().name;
            SceneManager.LoadScene(currentSceneName, LoadSceneMode.Single);
        }

        // Update der Lebensanzeige
        g_hpText.UpdateText(g_playerHP);
        hp_bar.UpdateHealthBar(g_playerHP);
        
    }

   
}

