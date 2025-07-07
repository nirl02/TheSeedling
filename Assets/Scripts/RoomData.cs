using UnityEngine;

[System.Serializable]
public class RoomData
{
    public string roomName;
    public GameObject roomPrefab;

    public bool hasNorthExit;
    public bool hasEastExit;
    public bool hasSouthExit;
    public bool hasWestExit;

    public string roomTag; // "combat", "shop", "deadend", "start", "end"

    // Verbindungsinformationen für die Map-Generierung
    [HideInInspector]
    public int roomNumber = -1;  // -1 = nicht zugewiesen

    [HideInInspector]
    public int northRoomNumber = -1;
    [HideInInspector]
    public int eastRoomNumber = -1;
    [HideInInspector]
    public int southRoomNumber = -1;
    [HideInInspector]
    public int westRoomNumber = -1;
}