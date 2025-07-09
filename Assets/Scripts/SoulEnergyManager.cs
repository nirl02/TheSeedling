using UnityEngine;
using UnityEngine.SceneManagement;


public class SoulEnergyManager : MonoBehaviour
{
    [Header("Nexus Portal Settings")]
    [SerializeField] private int totalEnergyCollected = 0;
    [SerializeField] private int energyRequiredForWin = 40;

    [SerializeField] private SoulEnergyProgressText progressText;

    [Header("Debug Info")]
    [SerializeField] private bool showDebugInfo = true;
    //[SerializeField] private Text progressText;

    private void Start()
    {
        totalEnergyCollected = 0;
        energyRequiredForWin = 40;
        progressText.UpdateText(totalEnergyCollected, energyRequiredForWin);
        // Stelle sicher, dass dieser Manager über Scene-Wechsel bestehen bleibt
        DontDestroyOnLoad(gameObject);

        // Falls bereits ein anderer Manager existiert, zerstöre diesen
        SoulEnergyManager[] managers = FindObjectsOfType<SoulEnergyManager>();
        if (managers.Length > 1)
        {
            Destroy(gameObject);
            return;
        }

        if (showDebugInfo)
        {
            Debug.Log($"SoulEnergyManager initialisiert - Fortschritt: {totalEnergyCollected}/{energyRequiredForWin}");
        }

        
    }

    private void Update()
    {
        // Debug-Taste zum Zurücksetzen des Fortschritts
        if (Input.GetKeyDown(KeyCode.F2))
        {
            ResetProgress();
        }

        // Debug-Taste zum Anzeigen des aktuellen Fortschritts
        if (Input.GetKeyDown(KeyCode.F3))
        {
            ShowProgress();
        }
    }

    /**
    * Fügt Energie zum Nexus Portal hinzu
    * @param amount - Menge der hinzuzufügenden Energie
    */
    public void AddEnergy(int amount)
    {
        totalEnergyCollected += amount;
        progressText.UpdateText(totalEnergyCollected,energyRequiredForWin);

        if (showDebugInfo)
        {
            Debug.Log($"Energie zum Nexus hinzugefügt: +{amount} = {totalEnergyCollected}/{energyRequiredForWin}");
        }

        // Prüfe Win Condition
        if (totalEnergyCollected >= energyRequiredForWin)
        {
            TriggerWinCondition();
        }
    }

    /**
    * Löst die Win Condition aus
    */
    private void TriggerWinCondition()
    {
        SceneManager.LoadScene("Add Victory", LoadSceneMode.Single);
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        Debug.Log("WIN CONDITION ERREICHT! Nexus Portal aktiviert!");

        // Hier kannst du verschiedene Win-Aktionen einfügen:
        // - Spezielle Szene laden
        // - Win-UI anzeigen
        // - Spezielle Effekte abspielen

        // Beispiel: Lade eine Win-Szene (falls vorhanden)
        // SceneManager.LoadScene("WinScene");

        // Oder zeige eine Win-Message und pausiere das Spiel
        Time.timeScale = 0f;
        Debug.Log("Spiel pausiert - Du hast gewonnen!");
    }

    /**
    * Debug Funktion: Zeigt den aktuellen Fortschritt an
    */
    private void ShowProgress()
    {
        Debug.Log($"NEXUS FORTSCHRITT: {totalEnergyCollected}/{energyRequiredForWin} Seelenenergie gesammelt");

        float percentage = (float)totalEnergyCollected / energyRequiredForWin * 100f;
        Debug.Log($"Fortschritt: {percentage:F1}%");
    }

    /**
    * Debug Funktion: Setzt den Fortschritt zurück
    */
    private void ResetProgress()
    {
        totalEnergyCollected = 0;
        Time.timeScale = 1f; // Falls das Spiel pausiert war
        Debug.Log("Nexus Fortschritt zurückgesetzt!");
    }

    /**
    * Getter für die gesammelte Energie (für UI oder andere Systeme)
    */
    public int GetCollectedEnergy()
    {
        return totalEnergyCollected;
    }

    /**
    * Getter für die benötigte Energie (für UI oder andere Systeme)
    */
    public int GetRequiredEnergy()
    {
        return energyRequiredForWin;
    }

    /**
    * Getter für den Fortschritt in Prozent (für UI oder andere Systeme)
    */
    public float GetProgressPercentage()
    {
        return (float)totalEnergyCollected / energyRequiredForWin * 100f;
    }
}