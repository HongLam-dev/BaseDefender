using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.InputSystem;


public abstract  class CharacterMovement : MonoBehaviour
{
    protected float moveSpeed = 5f;
    protected float speedMultiplier=1;
    public event Action<Vector3,float> OnMovement;
    public virtual void SetMoveSpeed(float speed)
    {
        moveSpeed = speed;
    }

    protected void SetDirection(Vector3 direction)
    {
        OnMovement?.Invoke(direction,speedMultiplier);
    }

    public abstract void SetSpeedMultiplier(float multiplier);
}
