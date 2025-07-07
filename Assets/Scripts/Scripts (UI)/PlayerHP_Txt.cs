using UnityEngine;
using UnityEngine.UI;

public class PlayerHP : MonoBehaviour
{
    /* Text der Lebensanzeige */
    [SerializeField] private Text g_playerHP;

    /**
    * Update die Lebensanzeige.
    * @param hp - aktuelle Lebensanzahl
    */
    public void UpdateText(int hp)
    {
        g_playerHP.text = hp.ToString();
    }
}