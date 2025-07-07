using UnityEngine;

public class SpriteDirectionalController : MonoBehaviour
{

    private void LateUpdate()
    {
        Vector3 camForwardVector = new Vector3(Camera.main.transform.forward.x, 0f, Camera.main.transform.forward.z);
        Debug.DrawRay(Camera.main.transform.position, camForwardVector * 5f, Color.blueViolet);
    }
}
