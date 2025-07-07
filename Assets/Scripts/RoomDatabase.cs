using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;

[CreateAssetMenu(fileName = "RoomDatabase", menuName = "Game/Room Database")]
public class RoomDatabase : ScriptableObject
{
    public List<RoomData> allRooms = new List<RoomData>();

    // Methoden zum Finden passender Räume
    public List<RoomData> GetRoomsWithNorthEntrance()
    {
        return allRooms.Where(room => room.hasNorthExit).ToList();
    }

    public List<RoomData> GetRoomsWithEastEntrance()
    {
        return allRooms.Where(room => room.hasEastExit).ToList();
    }

    public List<RoomData> GetRoomsWithSouthEntrance()
    {
        return allRooms.Where(room => room.hasSouthExit).ToList();
    }

    public List<RoomData> GetRoomsWithWestEntrance()
    {
        return allRooms.Where(room => room.hasWestExit).ToList();
    }

    // Methoden zum Filtern nach Tags
    public List<RoomData> GetRoomsWithTag(string tag)
    {
        return allRooms.Where(room => room.roomTag == tag).ToList();
    }

    // Hilfreiche Methode, um Räume nach Ausgängen zu filtern
    public List<RoomData> GetRoomsWithExits(bool north, bool east, bool south, bool west)
    {
        return allRooms.Where(room =>
            room.hasNorthExit == north &&
            room.hasEastExit == east &&
            room.hasSouthExit == south &&
            room.hasWestExit == west).ToList();
    }
}

#if UNITY_EDITOR
// Editor-Erweiterung für einfacheres Hinzufügen von Räumen
[CustomEditor(typeof(RoomDatabase))]
public class RoomDatabaseEditor : Editor
{
    private GameObject roomPrefab;
    private string roomTag = "normal";

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Raum hinzufügen", EditorStyles.boldLabel);

        roomPrefab = (GameObject)EditorGUILayout.ObjectField("Raum Prefab", roomPrefab, typeof(GameObject), false);
        roomTag = EditorGUILayout.TextField("Raum Tag", roomTag);

        if (GUILayout.Button("Raum zur Datenbank hinzufügen") && roomPrefab != null)
        {
            RoomDatabase database = (RoomDatabase)target;
            Room roomComponent = roomPrefab.GetComponent<Room>();

            if (roomComponent != null && roomComponent.data != null)
            {
                // Existierendes RoomData vom Prefab verwenden
                database.allRooms.Add(roomComponent.data);
                EditorUtility.SetDirty(target);
            }
            else
            {
                // Neues RoomData erstellen
                RoomData newRoom = new RoomData();
                newRoom.roomName = roomPrefab.name;
                newRoom.roomPrefab = roomPrefab;
                newRoom.roomTag = roomTag;

                // Wenn das Prefab eine Room-Komponente hat, Ausgänge prüfen
                if (roomComponent != null)
                {
                    newRoom.hasNorthExit = roomComponent.northExit != null;
                    newRoom.hasEastExit = roomComponent.eastExit != null;
                    newRoom.hasSouthExit = roomComponent.southExit != null;
                    newRoom.hasWestExit = roomComponent.westExit != null;

                    // Wenn das Prefab noch keine RoomData hat, eine erstellen und zuweisen
                    roomComponent.data = newRoom;
                    EditorUtility.SetDirty(roomComponent);
                }

                database.allRooms.Add(newRoom);
                EditorUtility.SetDirty(target);
            }
        }
    }
}
#endif