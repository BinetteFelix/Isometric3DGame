using UnityEngine;
using UnityEngine.UI;
public class HealthUI : MonoBehaviour
{
    public Image HealthBar;

    CanvasGroup canvasGroup;

    float fadeVariable;

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
                Destroy(gameObject);
            }
        }
    }
}
