using UnityEngine;

public class MainMenu3DButton : MonoBehaviour
{
    [SerializeField] private GameObject Panel;
    [SerializeField] private Renderer rend;
    private PanelMovingMainMenu moving;
    private bool hasPressed;
    [SerializeField] private Color startColor;
    [SerializeField] private Color hoverColor;
    [SerializeField] private Color clickColor;

    private void Start()
    {
        rend.material.color = startColor;
        moving = GetComponent<PanelMovingMainMenu>();
    }
    private void Update()
    {
        if (InputManager.Instance.actions.UI.PressSouth.WasPressedThisFrame())
        {
            UIManager.Instance.StartGame();
        }
    }
    private void OnMouseDown()
    {
        rend.material.color = clickColor;
    }
    private void OnMouseEnter()
    {
        rend.material.color = hoverColor;
    }
    private void OnMouseExit()
    {
        rend.material.color = startColor;
    }
    private void OnMouseUp()
    {
        rend.material.color = startColor;
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
