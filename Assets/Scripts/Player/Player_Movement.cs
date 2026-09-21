using UnityEngine;
using UnityEngine.InputSystem;

public class Player_Movement : MonoBehaviour
{
    #region COMPONENTS
    private Rigidbody RB;
    private Animator p_Animator;
    #endregion

    [SerializeField] public InputAction movementAction;

    private Vector3 _moveInput;
    private Vector2 WalkDirection;
    private int AngleOffset = 45;
    
    private float _moveSpeed = 3;
    private float _animatingMoveSpeed = 1;

    private void Awake()
    {
        movementAction.Enable();
        RB = GetComponent<Rigidbody>();
        p_Animator = GetComponent<Animator>();
    }
    private void Update()
    {
        GatherInputs();
        if(!SceneController.Instance.IsPaused)
            Look();
    }

    private void FixedUpdate()
    {
        Move();
    }
    void GatherInputs()
    {
        _moveInput = new Vector3(movementAction.ReadValue<Vector2>().x, 0, movementAction.ReadValue<Vector2>().y);
    }
    private float CalculateLookDirection()
    {
        Vector2 positionOnScreen = Camera.main.WorldToViewportPoint(transform.position);
        Vector2 mouseOnScreen = Camera.main.ScreenToViewportPoint(Mouse.current.position.ReadValue());
        float angle = -AngleBetweenTwoPoints(positionOnScreen, mouseOnScreen) - AngleOffset;

        #region Walk Animation Direction

        #region Forward
        // Walk Forward Conditions
        if (mouseOnScreen.x > 0.5f && (mouseOnScreen.y > 0.25f && mouseOnScreen.y < 0.75f) && _moveInput.x > 0)
            WalkDirection = new Vector2(0, 1);
        else if (mouseOnScreen.x < 0.5f && (mouseOnScreen.y > 0.25f && mouseOnScreen.y < 0.75f) && _moveInput.x < 0)
            WalkDirection = new Vector2(0, 1);
        else if (mouseOnScreen.y > 0.5f && (mouseOnScreen.x > 0.25f && mouseOnScreen.x < 0.75f) && _moveInput.z > 0)
            WalkDirection = new Vector2(0, 1);
        else if (mouseOnScreen.y < 0.5f && (mouseOnScreen.x > 0.25f && mouseOnScreen.x < 0.75f) && _moveInput.z < 0)
            WalkDirection = new Vector2(0, 1);

        // Walk Forward in Corner
        else if (mouseOnScreen.y > 0.5f && (mouseOnScreen.x > 0.75f) && (_moveInput.x > 0 || _moveInput.z > 0) && !(_moveInput.x < 0 || _moveInput.z < 0))
            WalkDirection = new Vector2(0, 1);
        else if (mouseOnScreen.y < 0.5f && (mouseOnScreen.x > 0.75f) && (_moveInput.x > 0 || _moveInput.z < 0) && !(_moveInput.x < 0 || _moveInput.z > 0))
            WalkDirection = new Vector2(0, 1);
        else if (mouseOnScreen.y > 0.5f && (mouseOnScreen.x < 0.25f) && (_moveInput.x < 0 || _moveInput.z > 0) && !(_moveInput.x > 0 || _moveInput.z < 0))
            WalkDirection = new Vector2(0, 1);
        else if (mouseOnScreen.y < 0.5f && (mouseOnScreen.x < 0.25f) && (_moveInput.x < 0 || _moveInput.z < 0) && !(_moveInput.x > 0 || _moveInput.z > 0))
            WalkDirection = new Vector2(0, 1);
        #endregion

        #region Backward
        // Walk Backward Conditions
        else if (mouseOnScreen.x > 0.5f && (mouseOnScreen.y > 0.25f && mouseOnScreen.y < 0.75f) && _moveInput.x < 0)
            WalkDirection = new Vector2(0, -1);
        else if (mouseOnScreen.x < 0.5f && (mouseOnScreen.y > 0.25f && mouseOnScreen.y < 0.75f) && _moveInput.x > 0)
            WalkDirection = new Vector2(0, -1);
        else if (mouseOnScreen.y > 0.5f && (mouseOnScreen.x > 0.25f && mouseOnScreen.x < 0.75f) && _moveInput.z < 0)
            WalkDirection = new Vector2(0, -1);
        else if (mouseOnScreen.y < 0.5f && (mouseOnScreen.x > 0.25f && mouseOnScreen.x < 0.75f) && _moveInput.z > 0)
            WalkDirection = new Vector2(0, -1);

        // Walk Backward in Corner
        else if (mouseOnScreen.y > 0.5f && (mouseOnScreen.x > 0.75f) && (_moveInput.x < 0 || _moveInput.z < 0) && !(_moveInput.x > 0 || _moveInput.z > 0))
            WalkDirection = new Vector2(0, -1);
        else if (mouseOnScreen.y < 0.5f && (mouseOnScreen.x > 0.75f) && (_moveInput.x < 0 || _moveInput.z > 0) && !(_moveInput.x > 0 || _moveInput.z < 0))
            WalkDirection = new Vector2(0, -1);
        else if (mouseOnScreen.y > 0.5f && (mouseOnScreen.x < 0.25f) && (_moveInput.x > 0 || _moveInput.z < 0) && !(_moveInput.x < 0 || _moveInput.z > 0))
            WalkDirection = new Vector2(0, -1);
        else if (mouseOnScreen.y < 0.5f && (mouseOnScreen.x < 0.25f) && (_moveInput.x > 0 || _moveInput.z > 0) && !(_moveInput.x < 0 || _moveInput.z < 0))
            WalkDirection = new Vector2(0, -1);
        #endregion

        #region Left
        // Walk Left Conditions
        else if (mouseOnScreen.x > 0.5f && (mouseOnScreen.y > 0.25f && mouseOnScreen.y < 0.75f) && _moveInput.z > 0)
            WalkDirection = new Vector2(-1, 0);
        else if (mouseOnScreen.x < 0.5f && (mouseOnScreen.y > 0.25f && mouseOnScreen.y < 0.75f) && _moveInput.z < 0)
            WalkDirection = new Vector2(-1, 0);
        else if (mouseOnScreen.y > 0.5f && (mouseOnScreen.x > 0.25f && mouseOnScreen.x < 0.75f) && _moveInput.x < 0)
            WalkDirection = new Vector2(-1, 0);
        else if (mouseOnScreen.y < 0.5f && (mouseOnScreen.x > 0.25f && mouseOnScreen.x < 0.75f) && _moveInput.x > 0)
            WalkDirection = new Vector2(-1, 0);

        // Walk Left in Corner
        else if (mouseOnScreen.y > 0.5f && (mouseOnScreen.x > 0.75f) && (_moveInput.x < 0 && _moveInput.z > 0))
            WalkDirection = new Vector2(-1, 0);
        else if (mouseOnScreen.y < 0.5f && (mouseOnScreen.x > 0.75f) && (_moveInput.x > 0 && _moveInput.z > 0))
            WalkDirection = new Vector2(-1, 0);
        else if (mouseOnScreen.y > 0.5f && (mouseOnScreen.x < 0.25f) && (_moveInput.x < 0 || _moveInput.z < 0))
            WalkDirection = new Vector2(-1, 0);
        else if (mouseOnScreen.y < 0.5f && (mouseOnScreen.x < 0.25f) && (_moveInput.x > 0 || _moveInput.z < 0))
            WalkDirection = new Vector2(-1, 0);
        #endregion

        #region Right
        // Walk Right Conditions
        else if (mouseOnScreen.x > 0.5f && (mouseOnScreen.y > 0.25f && mouseOnScreen.y < 0.75f) && _moveInput.z < 0)
            WalkDirection = new Vector2(1, 0);
        else if (mouseOnScreen.x < 0.5f && (mouseOnScreen.y > 0.25f && mouseOnScreen.y < 0.75f) && _moveInput.z > 0)
            WalkDirection = new Vector2(1, 0);
        else if (mouseOnScreen.y < 0.5f && (mouseOnScreen.x > 0.25f && mouseOnScreen.x < 0.75f) && _moveInput.x < 0)
            WalkDirection = new Vector2(1, 0);
        else if (mouseOnScreen.y > 0.5f && (mouseOnScreen.x > 0.25f && mouseOnScreen.x < 0.75f) && _moveInput.x > 0)
            WalkDirection = new Vector2(1, 0);

        // Walk Right in Corner
        else if (mouseOnScreen.y > 0.5f && (mouseOnScreen.x > 0.75f) && (_moveInput.x > 0 && _moveInput.z < 0))
            WalkDirection = new Vector2(1, 0);
        else if (mouseOnScreen.y < 0.5f && (mouseOnScreen.x > 0.75f) && (_moveInput.x < 0 && _moveInput.z < 0))
            WalkDirection = new Vector2(1, 0);
        else if (mouseOnScreen.y > 0.5f && (mouseOnScreen.x < 0.25f) && (_moveInput.x > 0 || _moveInput.z > 0))
            WalkDirection = new Vector2(1, 0);
        else if (mouseOnScreen.y < 0.5f && (mouseOnScreen.x < 0.25f) && (_moveInput.x < 0 || _moveInput.z > 0))
            WalkDirection = new Vector2(1, 0);
        #endregion

        #region Idle
        // Idle Condition
        else
            WalkDirection = new Vector2(0, 0);
        #endregion

        p_Animator.SetFloat("WalkDirY", WalkDirection.y);
        p_Animator.SetFloat("WalkDirX", WalkDirection.x);
        p_Animator.SetFloat("TraversingSpeed", _animatingMoveSpeed);

        if (p_Animator.GetFloat("WalkDirY") > 0)
            _moveSpeed = 4;
        else if (p_Animator.GetFloat("WalkDirY") < 0)
            _moveSpeed = 3;
        else if (p_Animator.GetFloat("WalkDirX") > 0 || p_Animator.GetFloat("WalkDirX") > 0)
            _moveSpeed = 3.5f;
        #endregion

        return angle;
    }
    private float AngleBetweenTwoPoints(Vector3 a, Vector3 b)
    {
        return Mathf.Atan2(a.y - b.y, a.x - b.x) * Mathf.Rad2Deg;
    }

    void Look()
    {
        transform.rotation = Quaternion.Euler(new Vector3(0, CalculateLookDirection(), 0).ToIso());
    }
    void Move()
    {
        RB.MovePosition(transform.position + ((transform.position + _moveInput.ToIso()) - transform.position) * _moveInput.magnitude * _moveSpeed * Time.deltaTime);
    }
}