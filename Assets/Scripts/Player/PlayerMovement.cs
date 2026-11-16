using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

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

        rb.linearVelocity = movementVelocity + externalVelocity;

        externalVelocity = Vector3.zero;
    }

    public override void SetSpeedMultiplier(float multiplier)
    {
        speedMultiplier = multiplier;
    }
}
