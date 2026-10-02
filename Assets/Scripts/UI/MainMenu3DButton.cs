using UnityEngine;

public class MainMenu3DButton : MonoBehaviour
{
    [SerializeField] private GameObject Panel;
    [SerializeField] private Renderer rend;

    private void OnMouseDown()
    {
        rend.material.color = Color.gray3;
    }
    private void OnMouseUp()
    {
        rend.material.color = Color.black;
        Panel.SetActive(true);
    }
}
