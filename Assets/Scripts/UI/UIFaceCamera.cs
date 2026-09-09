using UnityEngine;

public class UIFaceCamera : MonoBehaviour
{
    private Transform camTransform;

    void Start()
    {
        // Find the main camera automatically
        camTransform = Camera.main.transform;
    }

    void LateUpdate()
    {
        // Rotate the canvas to face the camera
        transform.LookAt(transform.position + camTransform.rotation * Vector3.forward,
                         camTransform.rotation * Vector3.up);
    }
}
