using Mono.Cecil;
using System;
using UnityEngine;
using UnityEngine.InputSystem;


[RequireComponent(typeof(CharacterCombat))]
public class PlayerMovement : MonoBehaviour
{
    private Vector3 moveDirection;

    [SerializeField]
    protected float moveSpeed = 5f;

    [SerializeField]
    protected float interactionRange = 1.5f;
    private Rigidbody rb;
    private bool canMove = true;

    private CharacterAnimationController animator;

    protected void Start()
    {
        animator = GetComponent<CharacterAnimationController>();
        rb = GetComponent<Rigidbody>();
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        Vector2 movement = context.ReadValue<Vector2>();
        SetDirection(new Vector3(movement.x, 0, movement.y));
    }

    void FixedUpdate()
    {
        rb.linearVelocity = new Vector3(moveDirection.x * moveSpeed, rb.linearVelocity.y, moveDirection.z * moveSpeed);
    }
    void SetDirection(Vector3 direction)
    {   
        if (!canMove)
            return;
        moveDirection = direction;
        animator.HandleMovement(moveDirection);    
    }

    void LockMovement(bool lockMovement)
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
