using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class PlayerMovement : CharacterMovement
{
    private Rigidbody rb;
    private Vector3 moveDirection;
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
    void FixedUpdate()
    {
        rb.linearVelocity = new Vector3(moveDirection.x * moveSpeed * speedMultiplier
            , rb.linearVelocity.y,
            moveDirection.z * moveSpeed*speedMultiplier);
    }

    public override void SetSpeedMultiplier(float multiplier)
    {
        speedMultiplier = multiplier;
    }
}
