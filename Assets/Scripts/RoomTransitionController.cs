using UnityEngine;
using UnityEngine.UI;

public class RoomTransitionController : MonoBehaviour
{
    private void Start()
    {
        // Initialize the first room
        InitializeFirstRoom();
    }

    private void InitializeFirstRoom()
    {
        // Find the DungeonGenerator
        DungeonGenerator generator = FindFirstObjectByType<DungeonGenerator>();
        if (generator == null)
        {
            Debug.LogError("No DungeonGenerator found in scene", this);
            return;
        }

        // We assume the first room is always room number 0
        // You might want to adjust this if your dungeon uses a different starting room
        GameObject startRoom = generator.SpawnRoom(0);
        if (startRoom == null)
        {
            Debug.LogError("Failed to spawn the starting room", this);
            return;
        }

        // Find the player
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player == null)
        {
            Debug.LogWarning("No player found with tag 'Player'", this);
            return;
        }

        // Position the player in the center of the room
        player.transform.position = startRoom.transform.position;
    }

    // Editor utility method to setup this controller in the scene
    [ContextMenu("Setup Room Transition System")]
    private void SetupRoomTransitionSystem()
    {
        // Make sure there's a RoomTransitionManager in the scene
        if (FindFirstObjectByType<RoomTransitionManager>() == null)
        {
            GameObject managerObj = new GameObject("RoomTransitionManager");
            RoomTransitionManager manager = managerObj.AddComponent<RoomTransitionManager>();

            // Create a default transition screen
            GameObject canvasObj = new GameObject("TransitionCanvas");
            Canvas canvas = canvasObj.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            canvas.sortingOrder = 999;

            CanvasScaler scaler = canvasObj.AddComponent<CanvasScaler>();
            scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            scaler.referenceResolution = new Vector2(1920, 1080);

            CanvasGroup group = canvasObj.AddComponent<CanvasGroup>();

            GameObject panel = new GameObject("BlackPanel");
            panel.transform.SetParent(canvasObj.transform, false);

            RectTransform rect = panel.AddComponent<RectTransform>();
            rect.anchorMin = Vector2.zero;
            rect.anchorMax = Vector2.one;
            rect.sizeDelta = Vector2.zero;

            UnityEngine.UI.Image image = panel.AddComponent<UnityEngine.UI.Image>();
            image.color = Color.black;

            canvasObj.transform.SetParent(managerObj.transform);
            canvasObj.SetActive(false);

            manager.transitionScreen = canvasObj;
        }

        Debug.Log("Room Transition System setup complete!");
    }
}