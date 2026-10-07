using UnityEngine;

public class MainMenu3DButton : MonoBehaviour
{
    [SerializeField] private GameObject Panel;
    [SerializeField] private Renderer rend;
    PanelMovingMainMenu moving;

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
    }
}
