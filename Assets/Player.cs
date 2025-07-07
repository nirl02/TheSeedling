//using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

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
    Gamepad gamepad;

    /* Nexus System */
    private bool inNexusArea = false;
    private NexusArea currentNexusArea;

    void Start()
    {
        g_playerHP = g_playerMaxHP;
        soulEnergy = 0;
        g_hpText.UpdateText(g_playerHP);
    }

    void Update()
    {
        // Debug Button für +5 Energie
        if (Input.GetKeyDown(KeyCode.F1))
        {
            DebugAddEnergy();
        }

        // E-Taste für Nexus Übertragung
        if (Input.GetKeyDown(KeyCode.E) && inNexusArea && soulEnergy > 0 || gamepad != null && gamepad.buttonSouth.isPressed && inNexusArea && soulEnergy > 0)
        {
            TransferEnergyToNexus();
        }
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

    /**
    * Debug Funktion: Fügt 5 Seelenenergie hinzu (F1 Taste)
    */
    private void DebugAddEnergy()
    {
        int energyGain = 5;
        int oldEnergy = soulEnergy;
        soulEnergy = Mathf.Min(soulEnergy + energyGain, maxSoulEnergy);

        Debug.Log($"DEBUG: Energie hinzugefügt - Alt: {oldEnergy}, Neu: {soulEnergy}/{maxSoulEnergy}");

        // Hier kannst du später UI-Updates für die Seelenenergie-Anzeige hinzufügen
    }

    /**
    * Überträgt alle Seelenenergie zum Nexus Portal
    */
    private void TransferEnergyToNexus()
    {
        if (currentNexusArea != null && soulEnergy > 0)
        {
            // Finde den SoulEnergyManager
            SoulEnergyManager energyManager = FindObjectOfType<SoulEnergyManager>();
            if (energyManager != null)
            {
                energyManager.AddEnergy(soulEnergy);
                Debug.Log($"Seelenenergie übertragen: {soulEnergy} -> Nexus Portal");
                soulEnergy = 0; // Alle Energie übertragen

                // Hier kannst du später UI-Updates für die Seelenenergie-Anzeige hinzufügen
            }
            else
            {
                Debug.LogError("SoulEnergyManager nicht gefunden!");
            }
        }
    }

    /**
    * Wird vom NexusArea Collider aufgerufen
    */
    public void EnterNexusArea(NexusArea nexusArea)
    {
        inNexusArea = true;
        currentNexusArea = nexusArea;
        Debug.Log("Nexus Area betreten - Drücke E um Seelenenergie zu übertragen");
    }

    /**
    * Wird vom NexusArea Collider aufgerufen
    */
    public void ExitNexusArea()
    {
        inNexusArea = false;
        currentNexusArea = null;
        Debug.Log("Nexus Area verlassen");
    }
}