using UnityEngine;

public class SetWorldSpaceEventCamera : MonoBehaviour
{
    [SerializeField] private Camera mainCam;

    private void Start()
    {
        mainCam = GetComponent<Camera>();
        foreach (GameObject worldCanvas in GameObject.FindGameObjectsWithTag("WorldSpaceCanvas"))
        {
            worldCanvas.GetComponent<Canvas>().worldCamera = mainCam;

            WorldSpaceUIParent inGamePanel = worldCanvas.GetComponent<WorldSpaceUIParent>();
            if (inGamePanel != null)
                worldCanvas.GetComponent<WorldSpaceUIParent>().camTransform = mainCam.transform;
        }
        
    }
}
