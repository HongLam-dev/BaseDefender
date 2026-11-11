using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FixedJoystick : Joystick
{
    public event Action<Vector2> OnJoystickDrag;
    public event Action<Vector2> OnRelease;

    private Vector2 lastDirection;

    private void Update()
    {
        if(Direction.sqrMagnitude>0.001)
        {
            OnJoystickDrag?.Invoke(Direction);
            lastDirection = Direction;
        }
        else if (lastDirection.sqrMagnitude > 0.001)
        {
            OnRelease?.Invoke(lastDirection);
            lastDirection = Direction;
        }
    }
}