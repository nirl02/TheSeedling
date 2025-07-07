using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 20f;

    void Update()
    {
        float moveX = Input.GetAxisRaw("Horizontal"); // A/D or Left/Right
        float moveZ = Input.GetAxisRaw("Vertical");   // W/S or Up/Down

        Vector3 move = new Vector3(moveX, 0f, moveZ).normalized;

        transform.Translate(move * moveSpeed * Time.deltaTime, Space.World);
    }
}
