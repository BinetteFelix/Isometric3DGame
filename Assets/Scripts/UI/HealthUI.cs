using UnityEngine;
using UnityEngine.UI;
public class HealthUI : MonoBehaviour
{
    public Image HealthBar;

    CanvasGroup canvasGroup;

    private void Start()
    {
        canvasGroup = GetComponent<CanvasGroup>();
    }
    private void Update()
    {
        if (HealthBar.fillAmount == 0)
        {
            canvasGroup.alpha -= Time.deltaTime;
            if (canvasGroup.alpha <= 0)
            {
                gameObject.SetActive(false);
            }
        }
    }
    public void ResetAlpha()
    {
        canvasGroup.alpha = 1;
    }
}
