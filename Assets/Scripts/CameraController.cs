using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;
    public Vector3 offset = new Vector3(0f, 10f, -10f);
    public float fieldOfView;
    private Quaternion fixedRotation;

    void Start()
    {
        fixedRotation = transform.rotation;
        fieldOfView = 40.0f;
    }

    void LateUpdate()
    {
        transform.position = target.position + offset;
        transform.rotation = fixedRotation;
        Camera.main.fieldOfView = fieldOfView;
    }
}