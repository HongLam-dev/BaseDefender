using UnityEngine;

public class PlayerMovement : CharacterMovement
{

    [SerializeField] FixedJoystick movementJoystick;

    private Rigidbody rb;
    private Vector3 moveDirection;
    private Vector3 movementVelocity;


    protected void Start()
    {
        rb = GetComponent<Rigidbody>();
        movementJoystick.OnJoystickDrag += OnMove;
        movementJoystick.OnRelease += OnIdle;
    }

    public void OnMove(Vector2 movement)
    {
        moveDirection = new Vector3(movement.x, 0, movement.y).normalized;
        SetDirection(moveDirection);
    }

    public void OnIdle(Vector2 previousDirection)
    {
        moveDirection = Vector3.zero;
        SetDirection(Vector3.zero);
    }

    private void FixedUpdate()
    {
        movementVelocity = moveDirection * moveSpeed;

        Vector3 movement= movementVelocity + externalVelocity;
        rb.linearVelocity = new Vector3(movement.x, rb.linearVelocity.y, movement.z);

        externalVelocity = Vector3.zero;
    }

    public override void SetSpeedMultiplier(float multiplier)
    {
        speedMultiplier = multiplier;
    }
}
