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
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        // Debug Button f�r +5 Energie
        if (Input.GetKeyDown(KeyCode.F1))
        {
            DebugAddEnergy();
        }

        // E-Taste f�r Nexus �bertragung
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
            Time.timeScale = 0f;
            Debug.Log("Spieler gestorben!");
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            // Level neuladen
            SceneManager.LoadScene("Add Defeat", LoadSceneMode.Additive);
        }

        // Update der Lebensanzeige
        g_hpText.UpdateText(g_playerHP);
        hp_bar.UpdateHealthBar(g_playerHP);

    }

    /**
    * Wird aufgerufen wenn ein Gegner stirbt - Spieler erh�lt Seelenenergie
    */
    public void OnEnemyKilled()
    {
        // Zuf�llige Seelenenergie zwischen 2 und 6
        int energyGain = Random.Range(2, 7); // 7 ist exklusiv, also 2-6

        // Seelenenergie hinzuf�gen, aber nicht �ber Maximum
        soulEnergy = Mathf.Min(soulEnergy + energyGain, maxSoulEnergy);

        Debug.Log($"Seelenenergie erhalten: {energyGain}, Gesamt: {soulEnergy}/{maxSoulEnergy}");

        // Hier kannst du sp�ter UI-Updates f�r die Seelenenergie-Anzeige hinzuf�gen
    }

    /**
    * Debug Funktion: F�gt 5 Seelenenergie hinzu (F1 Taste)
    */
    private void DebugAddEnergy()
    {
        int energyGain = 5;
        int oldEnergy = soulEnergy;
        soulEnergy = Mathf.Min(soulEnergy + energyGain, maxSoulEnergy);

        Debug.Log($"DEBUG: Energie hinzugef�gt - Alt: {oldEnergy}, Neu: {soulEnergy}/{maxSoulEnergy}");

        // Hier kannst du sp�ter UI-Updates f�r die Seelenenergie-Anzeige hinzuf�gen
    }

    /**
    * �bertr�gt alle Seelenenergie zum Nexus Portal
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
                Debug.Log($"Seelenenergie �bertragen: {soulEnergy} -> Nexus Portal");
                soulEnergy = 0; // Alle Energie �bertragen

                // Hier kannst du sp�ter UI-Updates f�r die Seelenenergie-Anzeige hinzuf�gen
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
        Debug.Log("Nexus Area betreten - Dr�cke E um Seelenenergie zu �bertragen");
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