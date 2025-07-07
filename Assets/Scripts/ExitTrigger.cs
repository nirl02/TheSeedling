using UnityEngine;

public class ExitTrigger : MonoBehaviour
{
    public Direction direction;
    public Room parentRoom;

    private bool isExitActive = true;
    private Collider triggerCollider;

    private void Awake()
    {
        // Referenz zum Collider speichern
        triggerCollider = GetComponent<Collider>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!isExitActive) return;

        if (other.CompareTag("Player"))
        {
            // Prüfe, ob dieser Ausgang zu einem anderen Raum führt
            if (parentRoom == null || parentRoom.data == null)
            {
                Debug.LogError("Exit Trigger hat keinen gültigen Parent Room oder keine RoomData!", this);
                return;
            }

            int nextRoomNumber = -1;

            // Bestimme den verbundenen Raum basierend auf der Ausgangsrichtung
            switch (direction)
            {
                case Direction.North:
                    nextRoomNumber = parentRoom.data.northRoomNumber;
                    break;
                case Direction.East:
                    nextRoomNumber = parentRoom.data.eastRoomNumber;
                    break;
                case Direction.South:
                    nextRoomNumber = parentRoom.data.southRoomNumber;
                    break;
                case Direction.West:
                    nextRoomNumber = parentRoom.data.westRoomNumber;
                    break;
            }

            // Prüfe, ob ein gültiger Raum verbunden ist
            if (nextRoomNumber == -1)
            {
                Debug.LogWarning($"Kein Raum in Richtung {direction} verbunden!", this);
                return;
            }

            // Exit vorübergehend deaktivieren
            DisableExit();

            // Führe den Raumwechsel durch
            int currentRoomNumber = parentRoom.data.roomNumber;

            // Use the RoomTransitionManager instead of directly using the DungeonGenerator
            RoomTransitionManager.Instance.TransitionToRoom(currentRoomNumber, nextRoomNumber, direction);
        }
    }

    // Methode zum Deaktivieren des Ausgangs
    public void DisableExit()
    {
        isExitActive = false;
        // Do NOT disable the collider, or bounds.Intersects won't work!
    }

    // Methode zum erneuten Aktivieren des Ausgangs
    public void EnableExit()
    {
        isExitActive = true;
    }
}