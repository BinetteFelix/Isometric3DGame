using UnityEngine;
using UnityEngine.InputSystem;
using Utility;

public class UIManager : SingletonBehaviour<UIManager>
{
    #region COMPONENTS
    [SerializeField] private Animator pauseAnimator;
    private Player_Movement playerMovement;
    [SerializeField] private GameObject pausePanel;
    #endregion
    
    #region INPUT PARAMETERS
    [SerializeField] private InputAction PauseAction;
    private InputAction movementAction;
    #endregion

    #region STATE PARAMETERS
    public bool IsPaused { get; private set; }
    #endregion

    #region MISCELLANEOUS

    #endregion

    private void Start()
    {
        PauseAction.Enable();
        playerMovement = GameObject.FindGameObjectWithTag("Player").GetComponent<Player_Movement>();
        movementAction = playerMovement.movementAction;
        pausePanel.SetActive(true);
        Cursor.lockState = CursorLockMode.Confined;
    }
    private void Update()
    {
        #region INPUT HANDLER
        if (PauseAction.WasPressedThisFrame())
        {
            Pause();
        }
        #endregion
    }
    public void Pause()
    {
        IsPaused = !IsPaused;

        switch (IsPaused)
        {
            case true:
                Time.timeScale = 0;
                movementAction.Disable();
                Cursor.lockState = CursorLockMode.None;
                break;
            case false:
                Time.timeScale = 1;
                movementAction.Enable();
                Cursor.lockState = CursorLockMode.Confined;
                break;
            default:
        }
        pauseAnimator.SetBool("PauseState", IsPaused);
        pauseAnimator.SetTrigger("PauseInput");     
    }

    public override void Instantiate()
    {
    }
}