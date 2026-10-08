using UnityEngine;

public class MainMenu3DButton : MonoBehaviour
{
    [SerializeField] private GameObject Panel;
    [SerializeField] private Renderer rend;
    private PanelMovingMainMenu moving;
    private bool hasPressed;

    private void Start()
    {
        moving = GetComponent<PanelMovingMainMenu>();
    }
    private void OnMouseDown()
    {
        rend.material.color = Color.gray3;
    }
    private void OnMouseUp()
    {
        rend.material.color = Color.black;
        if (moving != null )
            moving.MovePanel();
        if (MainMenuButtonEnabler.HasInstance && this.name != "PlayButton" && !hasPressed)
            MainMenuButtonEnabler.Instance.SetButtonsActive();
        if (this.name == "PlayButton")
            UIManager.Instance.StartGame();
        hasPressed = true;
        Invoke(nameof(ResetPressedState), 0.5f);
    }
    private void ResetPressedState()
    {
        hasPressed = false;
    }
}
