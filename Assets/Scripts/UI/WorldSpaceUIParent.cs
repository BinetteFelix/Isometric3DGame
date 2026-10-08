using UnityEngine;

public class WorldSpaceUIParent : MonoBehaviour
{
    public Transform camTransform;
    private CanvasGroup canvasGroup;
    void Start()
    {
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
    }
    public void ResetAlpha()
    {
        canvasGroup.alpha = 1.0f;
    }
}