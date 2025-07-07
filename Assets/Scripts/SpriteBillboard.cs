using UnityEngine;

public class SpriteBillboard : MonoBehaviour
{
    private Transform cam;

    void Start()
    {
        cam = Camera.main.transform;
    }

    void LateUpdate()
    {
        // Look at the camera but maintain upright orientation (no pitch/roll)
        Vector3 lookPos = transform.position + cam.rotation * Vector3.forward;
        Vector3 up = cam.rotation * Vector3.up;
        transform.LookAt(lookPos, up);
    }
}
