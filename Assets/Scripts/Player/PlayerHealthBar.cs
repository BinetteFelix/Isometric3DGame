using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthBar : MonoBehaviour
{
    public Image HealthBar;
    private Vector3 offset = new Vector3(0, 2.5f, 0);
    private Player_Movement player;
    private CanvasGroup canvasGroup;
    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player_Movement>();
        canvasGroup = GetComponentInParent<CanvasGroup>();

    }
    private void Update()
    {
        if (player != null)
        {
            transform.position = player.transform.position + offset;
        }
        if (HealthBar.fillAmount == 0)
        {
            canvasGroup.alpha -= Time.deltaTime;
            if (canvasGroup.alpha <= 0)
            {
                gameObject.SetActive(false);
            }

        }
    }
}
