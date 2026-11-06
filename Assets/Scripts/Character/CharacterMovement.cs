using System;
using UnityEngine;

public class CharacterMovement : MonoBehaviour
{

    private Vector2 moveDirection;

    [SerializeField]
    protected float moveSpeed = 5f;
    public event Action<Vector2> OnDirectionChange;
    private Rigidbody rb;
    private bool canMove=true;

    virtual protected void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }
    virtual protected void FixedUpdate()
    {
        rb.linearVelocity = new Vector3(moveDirection.x * moveSpeed, rb.linearVelocity.y,moveDirection.y * moveSpeed) ;
    }
    public void SetDirection(Vector2 direction)
    {
        if (!canMove)
            return;
        this.moveDirection = direction;
        OnDirectionChange?.Invoke(direction);
    }

    public void LockMovement(bool lockMovement)
    {
        if (lockMovement)
        {
            SetDirection(Vector2.zero);
            canMove = false;
        }
        else
            canMove = true;
    }
}