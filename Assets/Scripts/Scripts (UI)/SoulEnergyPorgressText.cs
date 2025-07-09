using UnityEngine;
using UnityEngine.UI;

public class SoulEnergyProgressText : MonoBehaviour
{
    /* Text der Lebensanzeige */
    [SerializeField] private Text energyProgressText;

    /**
    * Update die Energy die der Portalbekommen hat.
    */
    public void UpdateText(int energyProgress, int maxEnergy)
    {
        energyProgressText.text = energyProgress.ToString()
                                    + "/"
                                    + maxEnergy.ToString();
    }
}