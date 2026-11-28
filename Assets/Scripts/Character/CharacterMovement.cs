using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;


public class CharacterMovement : MonoBehaviour
{
    protected float moveSpeed = 5f;
    protected float speedMultiplier=1;
    public event Action<Vector3,float> OnMovement;
    protected Vector3 externalVelocity;
    protected Vector3 moveDirection=Vector3.zero;
    protected Rigidbody rb;
    private Vector3 movementVelocity;

    protected virtual void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    public virtual void SetMoveSpeed(float speed)
    {
        moveSpeed = speed;
    }

    public void SetDirection(Vector3 direction)
    {
        moveDirection = direction.normalized;
        OnMovement?.Invoke(direction,speedMultiplier);
    }

    public virtual void Stop()
    {
        speedMultiplier = 0;
    }

    public void ApplyKnockback(Vector3 direction, float force)
    {
        externalVelocity += direction.normalized * force;
    }

   protected virtual void FixedUpdate()
    {
        movementVelocity = moveDirection * moveSpeed;

        Vector3 movement = movementVelocity * speedMultiplier + externalVelocity;
        rb.linearVelocity = new Vector3(movement.x, rb.linearVelocity.y, movement.z);

        externalVelocity = Vector3.zero;
    }

    public void SetSpeedMultiplier(float multiplier)
    {
        speedMultiplier = multiplier;
    }

}
