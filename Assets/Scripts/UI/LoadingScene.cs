using UnityEngine;

public class LoadingScene : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnEnable()
    {
        Invoke(nameof(LoadFinished), 0.1f);
    }
    private void LoadFinished()
    {
        gameObject.SetActive(false);
    }
}
