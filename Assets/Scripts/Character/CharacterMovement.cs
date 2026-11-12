using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.InputSystem;


public abstract  class CharacterMovement : MonoBehaviour
{
    protected float moveSpeed = 5f;

    public event Action<Vector3> OnMovement;
    public event Action OnIdle;

    public virtual void SetMoveSpeed(float speed)
    {
        moveSpeed = speed;
    }

    protected void SetDirection(Vector3 direction)
    {
        if (direction.sqrMagnitude > 0.001)
            OnMovement?.Invoke(direction);
        else
            OnIdle?.Invoke();
    }

}
