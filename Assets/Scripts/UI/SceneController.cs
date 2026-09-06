using UnityEngine;
using UnityEngine.InputSystem;

public class SceneController : MonoBehaviour
{
    public static SceneController Instance;
    [SerializeField] private InputAction PauseAction;

    
    [SerializeField] private Animator pauseAnimator;

    [SerializeField] private GameObject pauseMenu;

    public bool IsPaused { get; private set; }

    private void Start()
    {
     
    }
    private void Update()
    {
        

        #region INPUT HANDLER
        if (PauseAction.IsPressed())
        {
            Debug.Log("AGNAGN");
            Pause();
        }
        #endregion
    }


    public void Pause()
    {
        IsPaused = !IsPaused;

        pauseAnimator.SetBool("PauseState", IsPaused);
        pauseAnimator.SetTrigger("PauseInput");
    }
    
}
