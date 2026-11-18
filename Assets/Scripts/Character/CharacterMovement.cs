using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;


public abstract  class CharacterMovement : MonoBehaviour
{
    protected float moveSpeed = 5f;
    protected float speedMultiplier=1;
    public event Action<Vector3,float> OnMovement;
    protected Vector3 externalVelocity;
    protected Vector3 moveDirection=Vector3.zero;

    public virtual void SetMoveSpeed(float speed)
    {
        moveSpeed = speed;
    }

    public void SetDirection(Vector3 direction)
    {
        moveDirection = direction;
        OnMovement?.Invoke(direction,speedMultiplier);
    }

    public abstract void SetSpeedMultiplier(float multiplier);
    public virtual void Stop()
    {
        speedMultiplier = 0;
    }

    public void ApplyKnockback(Vector3 direction, float force)
    {
        externalVelocity += direction.normalized * force;
    }
}
