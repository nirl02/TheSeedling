using UnityEngine;

public class NexusArea : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        // Prüfe ob es der Spieler ist
        Player player = other.GetComponent<Player>();
        if (player != null)
        {
            player.EnterNexusArea(this);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // Prüfe ob es der Spieler ist
        Player player = other.GetComponent<Player>();
        if (player != null)
        {
            player.ExitNexusArea();
        }
    }
}
