using UnityEngine;
using UnityEngine.InputSystem;

public class Player_Movement : MonoBehaviour
{
    [SerializeField] public InputAction movementAction;
    private Vector3 _input;
    [SerializeField] private Rigidbody RB;
    private float _moveSpeed = 5;
    private float _sensitivity = 1080;

    private void Awake()
    {
        movementAction.Enable();
    }
    private void Update()
    {
        GatherInputs();
        Look();
    }

    private void FixedUpdate()
    {
        Move();
    }
    void GatherInputs()
    {
        _input = new Vector3(movementAction.ReadValue<Vector2>().x, 0, movementAction.ReadValue<Vector2>().y);
    }

    void Look()
    {
        if (_input != Vector3.zero)
        {
            var relative = ((transform.position + _input.ToIso()) - transform.position);
            var rot = Quaternion.LookRotation(relative, Vector3.up);

            transform.rotation = Quaternion.RotateTowards(transform.rotation, rot, _sensitivity * Time.deltaTime);
        }
    }

    void Move()
    {
        RB.MovePosition(transform.position + transform.forward * _input.magnitude * _moveSpeed * Time.deltaTime);
    }
}
