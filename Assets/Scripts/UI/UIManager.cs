using UnityEngine;
using UnityEngine.InputSystem;
using Utility;

public class UIManager : SingletonBehaviour<UIManager>
{
    #region COMPONENTS
    [SerializeField] private Animator pauseAnimator;
    private Player_Movement playerMovement;
    [SerializeField] private GameObject pausePanel; 
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] public Texture2D cursor;

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
        SetCursorState(CursorLockMode.Confined, CursorMode.Auto, cursor);
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
                SetCursorState(CursorLockMode.None, CursorMode.Auto, default);
                break;
            case false:
                Time.timeScale = 1;
                movementAction.Enable();
                SetCursorState(CursorLockMode.Confined, CursorMode.Auto, cursor);
                break;
            default:
        }
        pauseAnimator.SetBool("PauseState", IsPaused);
        pauseAnimator.SetTrigger("PauseInput");     
    }
    public void PlayerDead()
    {
        gameOverPanel.SetActive(true);
        Time.timeScale = 0;
        playerMovement.movementAction.Disable();
    }

    public void SetCursorState(CursorLockMode lockmode, CursorMode mode, Texture2D texture)
    {
        Cursor.lockState = lockmode;
        Cursor.SetCursor(texture, Vector2.zero, mode);
    }
    public override void Instantiate()
    {
    }
}