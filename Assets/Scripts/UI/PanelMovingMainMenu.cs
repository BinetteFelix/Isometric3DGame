using UnityEngine;

public class PanelMovingMainMenu : MonoBehaviour
{
    [SerializeField] private int currentPanelIndex;
    [SerializeField] private string panelName;

    public void MovePanel()
    {
        MainMenuCameraBehavior.Instance.MoveCamera(currentPanelIndex, panelName);
    }
}
