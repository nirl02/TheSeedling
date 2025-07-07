using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class RoomTransitionManager : MonoBehaviour
{
    [Header("Transition Settings")]
    public float transitionTime = 0.5f;
    public GameObject transitionScreen;

    private static RoomTransitionManager _instance;
    public static RoomTransitionManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindFirstObjectByType<RoomTransitionManager>();
                if (_instance == null)
                {
                    GameObject obj = new GameObject("RoomTransitionManager");
                    _instance = obj.AddComponent<RoomTransitionManager>();
                }
            }
            return _instance;
        }
    }

    private GameObject currentRoom;
    private bool isTransitioning = false;

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
            return;
        }
        _instance = this;
        DontDestroyOnLoad(gameObject);

        if (transitionScreen == null)
        {
            transitionScreen = CreateDefaultTransitionScreen();
        }

        transitionScreen.SetActive(false);
    }

    public void TransitionToRoom(int fromRoomNumber, int toRoomNumber, Direction entryDirection)
    {
        if (isTransitioning)
            return;

        StartCoroutine(TransitionCoroutine(fromRoomNumber, toRoomNumber, entryDirection));
    }

    private IEnumerator TransitionCoroutine(int fromRoomNumber, int toRoomNumber, Direction entryDirection)
    {
        isTransitioning = true;

        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player == null)
        {
            Debug.LogError("No player object found with tag 'Player'");
            isTransitioning = false;
            yield break;
        }

        // Deaktiviere Spielersteuerung zu Beginn der Transition
        ThirdPersonMovement playerMovement = player.GetComponent<ThirdPersonMovement>();
        if (playerMovement != null)
        {
            playerMovement.DisablePlayerControl();
        }
        else
        {
            Debug.LogWarning("Player doesn't have ThirdPersonMovement component");
        }

        transitionScreen.SetActive(true);

        yield return FadeTransitionScreen(0f, 1f, transitionTime / 2);

        DungeonGenerator generator = FindFirstObjectByType<DungeonGenerator>();
        if (generator == null)
        {
            Debug.LogError("No DungeonGenerator found in scene");

            // Aktiviere Spielersteuerung wieder bei Fehler
            if (playerMovement != null)
            {
                playerMovement.EnablePlayerControl();
                Debug.Log("all done");
            }

            isTransitioning = false;
            yield break;
        }

        GameObject oldRoom = null;
        foreach (GameObject room in GameObject.FindGameObjectsWithTag("Room"))
        {
            Room roomComponent = room.GetComponent<Room>();
            if (roomComponent != null && roomComponent.data != null && roomComponent.data.roomNumber == fromRoomNumber)
            {
                oldRoom = room;
                break;
            }
        }

        if (oldRoom != null)
        {
            Destroy(oldRoom);
        }

        // Spawn the new room at position (0,0,0)
        GameObject newRoom = generator.SpawnRoom(toRoomNumber);
        if (newRoom == null)
        {
            Debug.LogError($"Failed to spawn room {toRoomNumber}");

            // Aktiviere Spielersteuerung wieder bei Fehler
            if (playerMovement != null)
            {
                playerMovement.EnablePlayerControl();
            }

            isTransitioning = false;
            yield break;
        }

        Room newRoomComponent = newRoom.GetComponent<Room>();
        if (newRoomComponent == null)
        {
            Debug.LogError("New room doesn't have a Room component");

            // Aktiviere Spielersteuerung wieder bei Fehler
            if (playerMovement != null)
            {
                playerMovement.EnablePlayerControl();
            }

            isTransitioning = false;
            yield break;
        }

        Direction oppositeDirection = GetOppositeDirection(entryDirection);

        Transform spawnPoint = newRoomComponent.GetSpawnPointForDirection(oppositeDirection);

        player.transform.position = spawnPoint.position;

        yield return new WaitForSeconds(0.2f);

        yield return FadeTransitionScreen(1f, 0f, transitionTime / 2);

        transitionScreen.SetActive(false);

        GameObject exitObject = null;
        switch (oppositeDirection)
        {
            case Direction.North:
                exitObject = newRoomComponent.northExit;
                break;
            case Direction.East:
                exitObject = newRoomComponent.eastExit;
                break;
            case Direction.South:
                exitObject = newRoomComponent.southExit;
                break;
            case Direction.West:
                exitObject = newRoomComponent.westExit;
                break;
        }

        if (exitObject != null && exitObject.activeSelf)
        {
            ExitTrigger exitTrigger = exitObject.GetComponent<ExitTrigger>();
            if (exitTrigger != null)
            {
                exitTrigger.DisableExit();

                // Start a coroutine to re-enable the exit when the player is no longer in contact
                StartCoroutine(ReEnableExitWhenPlayerExits(exitTrigger, player));
            }
        }

        // Aktiviere Spielersteuerung wieder am Ende der Transition
        if (playerMovement != null)
        {
            playerMovement.EnablePlayerControl();
        }

        isTransitioning = false;
    }

    private IEnumerator ReEnableExitWhenPlayerExits(ExitTrigger exit, GameObject player)
    {
        Collider exitCollider = exit.GetComponent<Collider>();
        if (exitCollider == null)
            yield break;

        Collider playerCollider = player.GetComponent<Collider>();
        if (playerCollider == null)
            yield break;

        while (exitCollider.bounds.Intersects(playerCollider.bounds))
        {
            yield return null;
        }

        exit.EnableExit();
    }

    private Direction GetOppositeDirection(Direction dir)
    {
        switch (dir)
        {
            case Direction.North: return Direction.South;
            case Direction.South: return Direction.North;
            case Direction.East: return Direction.West;
            case Direction.West: return Direction.East;
            default: return dir;
        }
    }

    private IEnumerator FadeTransitionScreen(float startAlpha, float endAlpha, float duration)
    {
        CanvasGroup canvasGroup = transitionScreen.GetComponent<CanvasGroup>();
        if (canvasGroup == null)
        {
            Debug.LogWarning("No CanvasGroup found on transition screen. Fade effect will not work.");
            yield break;
        }

        float elapsed = 0f;
        while (elapsed < duration)
        {
            canvasGroup.alpha = Mathf.Lerp(startAlpha, endAlpha, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        canvasGroup.alpha = endAlpha;
    }

    private GameObject CreateDefaultTransitionScreen()
    {
        GameObject canvasObject = new GameObject("TransitionCanvas");
        Canvas canvas = canvasObject.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvas.sortingOrder = 999; // Make sure it renders on top

        CanvasScaler scaler = canvasObject.AddComponent<CanvasScaler>();
        scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        scaler.referenceResolution = new Vector2(1920, 1080);

        CanvasGroup canvasGroup = canvasObject.AddComponent<CanvasGroup>();

        GameObject panel = new GameObject("BlackPanel");
        panel.transform.SetParent(canvasObject.transform, false);

        RectTransform panelRect = panel.AddComponent<RectTransform>();
        panelRect.anchorMin = Vector2.zero;
        panelRect.anchorMax = Vector2.one;
        panelRect.sizeDelta = Vector2.zero;

        UnityEngine.UI.Image image = panel.AddComponent<UnityEngine.UI.Image>();
        image.color = Color.black;

        canvasObject.transform.SetParent(transform);

        return canvasObject;
    }
}