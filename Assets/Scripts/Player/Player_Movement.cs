using UnityEngine;
using UnityEngine.InputSystem;

public class Player_Movement : MonoBehaviour
{
    #region COMPONENTS
    private Rigidbody RB;
    #endregion

    [SerializeField] public InputAction movementAction;

    private Vector3 _moveInput;
    private int AngleOffset = 45;
    
    private float _moveSpeed = 5;

    private void Awake()
    {
        movementAction.Enable();
        RB = GetComponent<Rigidbody>();
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