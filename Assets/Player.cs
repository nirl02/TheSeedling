//using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    /* Spieler Lebensanzahl */

    public int g_playerMaxHP;
    [SerializeField] public int g_playerHP;

    /* Seelenenergie System */
    [SerializeField] public int soulEnergy = 0;
    [SerializeField] public int maxSoulEnergy = 20;

    /* Text der Lebensanzahl */
    [SerializeField] private PlayerHP g_hpText;

    /* HP Bar */
    [SerializeField] private HP_Bar hp_bar;

    [SerializeField] GameObject pauseMenu;
    bool gamePaused = false;


    void Start()
    {
        g_playerHP = g_playerMaxHP;
        soulEnergy = 0;
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

    /**
    * Wird aufgerufen wenn ein Gegner stirbt - Spieler erhält Seelenenergie
    */
    public void OnEnemyKilled()
    {
        // Zufällige Seelenenergie zwischen 2 und 6
        int energyGain = Random.Range(2, 7); // 7 ist exklusiv, also 2-6

        // Seelenenergie hinzufügen, aber nicht über Maximum
        soulEnergy = Mathf.Min(soulEnergy + energyGain, maxSoulEnergy);

        Debug.Log($"Seelenenergie erhalten: {energyGain}, Gesamt: {soulEnergy}/{maxSoulEnergy}");

        // Hier kannst du später UI-Updates für die Seelenenergie-Anzeige hinzufügen
    }


}