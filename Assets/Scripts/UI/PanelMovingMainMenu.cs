using UnityEngine;

public class PanelMovingMainMenu : MonoBehaviour
{
    [SerializeField] private int currentPanelIndex;
    [SerializeField] private string panelName;

    public void MovePanel()
    {
        MainMenuCameraBehavior.Instance.MoveCamera(currentPanelIndex, panelName);
        switch (panelName)
        {
            case "ToStart":
                {
                    UIManager.Instance.eventSystem.SetSelectedGameObject(UIManager.Instance.FirstSelected[2]);
                    break;
                }
            case "ToScore":
                {
                    UIManager.Instance.eventSystem.SetSelectedGameObject(UIManager.Instance.FirstSelected[3]);
                    break;
                }
            case "ToSettings":
                {
                    UIManager.Instance.eventSystem.SetSelectedGameObject(UIManager.Instance.FirstSelected[4]);
                    break;
                }
            default:
                break;
        }
    }
}
