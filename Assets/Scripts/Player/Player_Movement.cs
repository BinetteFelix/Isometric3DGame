using UnityEngine;

public class Player_Movement : MonoBehaviour
{
    private Vector3 _input;
    [SerializeField] private Rigidbody RB;
    private float _moveSpeed = 5;
    private float _sensitivity = 1080;
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
        _input = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
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
