using UnityEngine;

public class WorldSpaceUIParent : MonoBehaviour
{
    private Transform camTransform;
    Canvas canvasComponent;

    void Start()
    {
        // Find the main camera automatically
        camTransform = Camera.main.transform;
        canvasComponent = GetComponent<Canvas>();
        canvasComponent.worldCamera = Camera.main;
    }

    void LateUpdate()
    {
        // Rotate the canvas to face the camera
        transform.LookAt(transform.position + camTransform.rotation * Vector3.forward,
                         camTransform.rotation * Vector3.up);
    }
    public void DestroyWorldspaceObjects()
    {
        foreach (RectTransform obj in GetComponentsInChildren<RectTransform>())
        {
            if (obj != this.gameObject.GetComponent<RectTransform>())
                Destroy(obj.gameObject);
        }
        canvasComponent.worldCamera = Camera.main;
    }
}