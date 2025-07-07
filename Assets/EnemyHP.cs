using UnityEngine;
using TMPro;

public class EnemyHP : MonoBehaviour
{
    /* Transform des Spielers fuer die Ausrichtung */
    private Transform g_target;
    /* Text der Lebensanzeige */
    [SerializeField] private TMP_Text g_enemyHP;

    /**
    * Zu Begin Spielertransform holen
    */
    void Start()
    {
        g_target = GameObject.FindGameObjectWithTag("Player").transform.root.transform;
    }

    // Update is called once per frame
    void Update()
    {
        // Drehe Hp-Anzeige zum Spieler
        transform.LookAt(g_target);
        transform.Rotate(0, 180, 0);
    }

    /**
    * Update die Lebensanzeige.
    * @param hp - aktuelle Lebensanzahl
    */
    public void UpdateText(int hp)
    {
        g_enemyHP.text = hp.ToString();
    }
}
