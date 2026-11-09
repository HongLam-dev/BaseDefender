using System;
using TMPro;
using UnityEngine;
using UnityEngine.AI;

public abstract class CharacterMovement : MonoBehaviour
{
    private Vector3 moveDirection;

    [SerializeField]
    protected float moveSpeed = 5f;

    [SerializeField]
    protected float interactionRange = 1.5f;
    public event Action<Vector3> OnDirectionChange;
    private Rigidbody rb;
    private bool canMove=true;

    protected virtual void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    protected virtual void FixedUpdate()
    {
       rb.linearVelocity = new Vector3(moveDirection.x * moveSpeed, rb.linearVelocity.y,moveDirection.z* moveSpeed) ;   
    }
    protected void SetDirection(Vector3 direction)
    {
        if (!canMove)
            return;
        this.moveDirection = direction;
        OnDirectionChange?.Invoke(direction);
    }

    protected void LockMovement(bool lockMovement)
    {
        if (lockMovement)
        {
            SetDirection(Vector3.zero);
            canMove = false;
        }
        else
            canMove = true;
    }
}