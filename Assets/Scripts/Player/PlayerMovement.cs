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

    public event Action<Vector3> OnMovement;
    public event Action OnIdle;

    protected void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        Vector2 movement = context.ReadValue<Vector2>().normalized;
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

        if (moveDirection.sqrMagnitude > 0.001)
            OnMovement?.Invoke(direction);
        else
            OnIdle?.Invoke();
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
