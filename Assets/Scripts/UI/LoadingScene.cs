using UnityEngine;

public class LoadingScene : MonoBehaviour
{
    private void OnEnable()
    {
        Invoke(nameof(LoadFinished), 0.1f);
    }
    private void LoadFinished()
    {
        gameObject.SetActive(false);
    }
}
