using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using Utility;

public class UIManager : SingletonBehaviour<UIManager>
{
    #region COMPONENTS
    [SerializeField] private Animator pauseAnimator;
    private Player_Movement playerMovement;
    [SerializeField] private GameObject pausePanel; 
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] WorldSpaceUIParent worldSpaceCanvas;
    [SerializeField] public Texture2D cursor;
    [SerializeField] private EventSystem eventSystem;
    [SerializeField] private GameObject[] FirstSelected;
    [SerializeField] private List<GameObject> ammoType;
    [SerializeField] private TextMeshProUGUI ammoAmount;
    #endregion

    #region INPUT PARAMETERS
    [SerializeField] private InputAction PauseAction;
    private InputAction movementAction;
    #endregion

    #region STATE PARAMETERS
    public bool IsPaused { get; private set; }
    private int gameSceneIndex = 1;
    #endregion

    #region MISCELLANEOUS
    public bool JustResetGame;
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
                eventSystem.SetSelectedGameObject(FirstSelected[0]);
                break;
            case false:
                Time.timeScale = 1;
                movementAction.Enable();
                SetCursorState(CursorLockMode.Confined, CursorMode.Auto, cursor);
                break;
        }
        pauseAnimator.SetBool("PauseState", IsPaused);
        pauseAnimator.SetTrigger("PauseInput");     
    }
    public void PlayerDead()
    {
        gameOverPanel.SetActive(true);
        Time.timeScale = 0;
        playerMovement.movementAction.Disable();
        SetCursorState(CursorLockMode.None, CursorMode.Auto, default);
        eventSystem.SetSelectedGameObject(FirstSelected[1]);
    }
    public void SetCursorState(CursorLockMode lockmode, CursorMode mode, Texture2D texture)
    {
        Cursor.lockState = lockmode;
        Cursor.SetCursor(texture, Vector2.zero, mode);
    }
    public void ResetGame()
    {
        //MarkerHandler.Instance.ResetList();
        SetCursorState(CursorLockMode.Confined, CursorMode.Auto, cursor);
        Time.timeScale = 1;
        gameOverPanel.SetActive(false);
        worldSpaceCanvas.DestroyWorldspaceObjects();
        worldSpaceCanvas.ResetAlpha();
        JustResetGame = true;
        ExperienceHandler.Instance.ResetLevel();
        Invoke(nameof(ResetActualScene), 0.1f);
    }
    private void ResetActualScene()
    {
        SceneManager.LoadScene(gameSceneIndex);
    }
    public void SetAmmoUI()
    {
        WeaponCollection.Instance.SetUI(ammoType);
        WeaponCollection.Instance.Weapons[1].GetComponent<EyeBall>().tearAmountText = ammoAmount;
        WeaponCollection.Instance.Weapons[0].GetComponent<PlayerFireGun>().bulletAmount = ammoAmount;
    }
    #region INSTANCE HANDLER
    public override void Instantiate()
    {
    }
    #endregion
}