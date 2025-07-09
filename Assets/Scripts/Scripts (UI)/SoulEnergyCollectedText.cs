using UnityEngine;
using UnityEngine.UI;

public class SoulEnergyCollectedText : MonoBehaviour
{
    /* Text der Lebensanzeige */
    [SerializeField] private Text energyProgressText;

    /**
    * Update die Energy die der Portalbekommen hat.
    * maxEnergy = maximale Energie die gleichzeitig getragen werden darf.
    */
    public void UpdateText(int energyProgress, int maxEnergy)
    {
        energyProgressText.text = energyProgress.ToString()
                                    + "/"
                                    + maxEnergy.ToString();
    }
}