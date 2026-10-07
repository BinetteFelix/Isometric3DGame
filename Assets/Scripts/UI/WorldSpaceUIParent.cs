using UnityEngine;

public class WorldSpaceUIParent : MonoBehaviour
{
    private Transform camTransform;
    private Canvas canvasComponent;
    private CanvasGroup canvasGroup;
    void Start()
    {
        // Find the main camera automatically
        camTransform = Camera.main.transform;
        canvasComponent = GetComponent<Canvas>();
        canvasComponent.worldCamera = Camera.main;
        canvasGroup = GetComponent<CanvasGroup>();
    }

    void LateUpdate()
    {
        // Rotate the canvas to face the camera
        if (camTransform != null)
        {
            transform.LookAt(transform.position + camTransform.rotation * Vector3.forward,
                            camTransform.rotation * Vector3.up);
        }
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
    public void ResetAlpha()
    {
        canvasGroup.alpha = 1.0f;
    }
}