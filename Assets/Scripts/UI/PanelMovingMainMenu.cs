using UnityEngine;

public class PanelMovingMainMenu : MonoBehaviour
{
    [SerializeField] private MainMenuCameraBehavior cameraBehavior;
    [SerializeField] private int currentPanelIndex;
    [SerializeField] private string panelName;

    public void MovePanel()
    {
        cameraBehavior.MoveCamera(currentPanelIndex, panelName);
    }
}
