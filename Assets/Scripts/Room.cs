using UnityEngine;
using System.Collections.Generic;

public class Room : MonoBehaviour
{
    public RoomData data;

    public Transform northSpawnPoint;
    public Transform eastSpawnPoint;
    public Transform southSpawnPoint;
    public Transform westSpawnPoint;

    public GameObject northExit;
    public GameObject eastExit;
    public GameObject southExit;
    public GameObject westExit;

    // Turret spawn points - pool of possible turret positions
    [Header("Turret Spawn Points")]
    public List<Transform> turretSpawnPoints = new List<Transform>();

    // Debug flags
    public bool debugExits = false;
    public bool debugSpawnPoints = false;
    public bool debugTurretSpawnPoints = false;

    private void Awake()
    {
        // Make sure we have data
        if (data == null)
        {
            Debug.LogError($"Room {gameObject.name} has no RoomData assigned!", this);
        }
    }

    private void Start()
    {
        // Setup room exits based on RoomData
        SetupExits();

        // Setup exit triggers
        SetupExitTriggers();

        // Validate turret spawn points
        ValidateTurretSpawnPoints();
    }

    public void SetupExits()
    {
        if (data == null)
        {
            return;
        }

        // Activate or deactivate exits based on RoomData
        if (northExit)
        {
            northExit.SetActive(data.hasNorthExit);
        }

        if (eastExit)
        {
            eastExit.SetActive(data.hasEastExit);
        }

        if (southExit)
        {
            southExit.SetActive(data.hasSouthExit);
        }

        if (westExit)
        {
            westExit.SetActive(data.hasWestExit);
        }
    }

    private void SetupExitTriggers()
    {
        if (data == null)
            return;

        if (northExit && data.hasNorthExit)
            SetupExitTrigger(northExit, Direction.North);

        if (eastExit && data.hasEastExit)
            SetupExitTrigger(eastExit, Direction.East);

        if (southExit && data.hasSouthExit)
            SetupExitTrigger(southExit, Direction.South);

        if (westExit && data.hasWestExit)
            SetupExitTrigger(westExit, Direction.West);
    }

    private void SetupExitTrigger(GameObject exit, Direction direction)
    {
        // Make sure the exit GameObject is active
        if (!exit.activeSelf)
        {
            exit.SetActive(true); // Force activate it
        }

        // Check if the exit has a collider
        Collider collider = exit.GetComponent<Collider>();
        if (collider == null)
        {
            collider = exit.AddComponent<BoxCollider>();
            ((BoxCollider)collider).size = new Vector3(2f, 3f, 2f); // Adjust as needed
        }

        // Make sure it's marked as a trigger
        collider.isTrigger = true;

        // Make sure the exit has an ExitTrigger component
        ExitTrigger trigger = exit.GetComponent<ExitTrigger>();
        if (trigger == null)
        {
            trigger = exit.AddComponent<ExitTrigger>();
        }

        // Configure the trigger
        trigger.direction = direction;
        trigger.parentRoom = this;
    }

    private int GetConnectedRoomNumber(Direction direction)
    {
        if (data == null) return -1;

        switch (direction)
        {
            case Direction.North: return data.northRoomNumber;
            case Direction.East: return data.eastRoomNumber;
            case Direction.South: return data.southRoomNumber;
            case Direction.West: return data.westRoomNumber;
            default: return -1;
        }
    }

    public Transform GetSpawnPointForDirection(Direction direction)
    {
        switch (direction)
        {
            case Direction.North:
                if (northSpawnPoint == null)
                {
                    return transform; // Fallback to room origin
                }
                return northSpawnPoint;
            case Direction.East:
                if (eastSpawnPoint == null)
                {
                    return transform; // Fallback to room origin
                }
                return eastSpawnPoint;
            case Direction.South:
                if (southSpawnPoint == null)
                {
                    return transform; // Fallback to room origin
                }
                return southSpawnPoint;
            case Direction.West:
                if (westSpawnPoint == null)
                {
                    return transform; // Fallback to room origin
                }
                return westSpawnPoint;
            default:
                return transform;
        }
    }

    #region Turret Spawn Point Methods

    /// <summary>
    /// Returns all available turret spawn points in this room
    /// </summary>
    /// <returns>List of Transform positions where turrets can spawn</returns>
    public List<Transform> GetTurretSpawnPoints()
    {
        return new List<Transform>(turretSpawnPoints);
    }

    /// <summary>
    /// Returns a specific turret spawn point by index
    /// </summary>
    /// <param name="index">Index of the spawn point</param>
    /// <returns>Transform of the spawn point, or null if index is invalid</returns>
    public Transform GetTurretSpawnPoint(int index)
    {
        if (index >= 0 && index < turretSpawnPoints.Count)
        {
            return turretSpawnPoints[index];
        }
        return null;
    }

    /// <summary>
    /// Returns the number of available turret spawn points
    /// </summary>
    /// <returns>Count of turret spawn points</returns>
    public int GetTurretSpawnPointCount()
    {
        return turretSpawnPoints.Count;
    }

    /// <summary>
    /// Checks if there are any turret spawn points available
    /// </summary>
    /// <returns>True if there are turret spawn points, false otherwise</returns>
    public bool HasTurretSpawnPoints()
    {
        return turretSpawnPoints.Count > 0;
    }

    /// <summary>
    /// Validates all turret spawn points and removes null references
    /// </summary>
    private void ValidateTurretSpawnPoints()
    {
        for (int i = turretSpawnPoints.Count - 1; i >= 0; i--)
        {
            if (turretSpawnPoints[i] == null)
            {
                Debug.LogWarning($"Room {gameObject.name}: Turret spawn point at index {i} is null, removing from list.", this);
                turretSpawnPoints.RemoveAt(i);
            }
        }

        if (debugTurretSpawnPoints)
        {
            Debug.Log($"Room {gameObject.name}: Found {turretSpawnPoints.Count} valid turret spawn points.", this);
        }
    }

    /// <summary>
    /// Adds a new turret spawn point to the list
    /// </summary>
    /// <param name="spawnPoint">Transform to add as a turret spawn point</param>
    public void AddTurretSpawnPoint(Transform spawnPoint)
    {
        if (spawnPoint != null && !turretSpawnPoints.Contains(spawnPoint))
        {
            turretSpawnPoints.Add(spawnPoint);
        }
    }

    /// <summary>
    /// Removes a turret spawn point from the list
    /// </summary>
    /// <param name="spawnPoint">Transform to remove</param>
    public void RemoveTurretSpawnPoint(Transform spawnPoint)
    {
        turretSpawnPoints.Remove(spawnPoint);
    }

    #endregion

    #region Debug Visualization

    private void OnDrawGizmos()
    {
        // Debug turret spawn points
        if (debugTurretSpawnPoints && turretSpawnPoints != null)
        {
            Gizmos.color = Color.red;
            foreach (Transform spawnPoint in turretSpawnPoints)
            {
                if (spawnPoint != null)
                {
                    Gizmos.DrawWireSphere(spawnPoint.position, 0.5f);
                    Gizmos.DrawRay(spawnPoint.position, spawnPoint.forward * 2f);
                }
            }
        }

        // Debug player spawn points
        if (debugSpawnPoints)
        {
            Gizmos.color = Color.blue;
            if (northSpawnPoint != null)
                Gizmos.DrawWireCube(northSpawnPoint.position, Vector3.one);
            if (eastSpawnPoint != null)
                Gizmos.DrawWireCube(eastSpawnPoint.position, Vector3.one);
            if (southSpawnPoint != null)
                Gizmos.DrawWireCube(southSpawnPoint.position, Vector3.one);
            if (westSpawnPoint != null)
                Gizmos.DrawWireCube(westSpawnPoint.position, Vector3.one);
        }

        // Debug exits
        if (debugExits)
        {
            Gizmos.color = Color.green;
            if (northExit != null && northExit.activeSelf)
                Gizmos.DrawWireCube(northExit.transform.position, Vector3.one * 2f);
            if (eastExit != null && eastExit.activeSelf)
                Gizmos.DrawWireCube(eastExit.transform.position, Vector3.one * 2f);
            if (southExit != null && southExit.activeSelf)
                Gizmos.DrawWireCube(southExit.transform.position, Vector3.one * 2f);
            if (westExit != null && westExit.activeSelf)
                Gizmos.DrawWireCube(westExit.transform.position, Vector3.one * 2f);
        }
    }

    #endregion
}

// Hilfreiche Enumeration für Richtungen
public enum Direction
{
    North,
    East,
    South,
    West
}