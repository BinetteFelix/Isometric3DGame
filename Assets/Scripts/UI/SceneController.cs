using UnityEngine;
using UnityEngine.InputSystem;

public class SceneController : MonoBehaviour
{
    #region COMPONENTS
    [SerializeField] private Animator pauseAnimator;
    private Player_Movement playerMovement;
    #endregion
    
    #region INPUT PARAMETERS
    [SerializeField] private InputAction PauseAction;
    private InputAction movementAction;
    #endregion

    #region STATE PARAMETERS
    public bool IsPaused { get; private set; }
    #endregion

    #region MISCELLANEOUS
    public static SceneController Instance;

    #endregion

    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        PauseAction.Enable();
        playerMovement = GameObject.FindGameObjectWithTag("Player").GetComponent<Player_Movement>();
        movementAction = playerMovement.movementAction;
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
                break;
            case false:
                Time.timeScale = 1;
                movementAction.Enable();
                break;
            default:
        }
        Debug.Log(Time.timeScale);
        pauseAnimator.SetBool("PauseState", IsPaused);
        pauseAnimator.SetTrigger("PauseInput");     
    }
    
}
