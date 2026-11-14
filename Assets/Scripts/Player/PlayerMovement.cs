using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class PlayerMovement : CharacterMovement
{
    private Rigidbody rb;
    private Vector3 moveDirection;
    private Vector3 movementVelocity;
    protected void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        Vector2 movement = context.ReadValue<Vector2>().normalized;
        moveDirection = new Vector3(movement.x, 0, movement.y);
        SetDirection(moveDirection*speedMultiplier);
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
