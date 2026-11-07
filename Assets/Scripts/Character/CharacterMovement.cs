using System;
using TMPro;
using UnityEngine;
using UnityEngine.AI;

public class CharacterMovement : MonoBehaviour
{
    private NavMeshAgent agent;
    private Vector3 moveDirection;

    [SerializeField]
    protected float moveSpeed = 5f;

    [SerializeField]
    protected float interactionRange = 1.5f;
    public event Action<Vector3> OnDirectionChange;
    private Rigidbody rb;
    private bool canMove=true;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        agent= GetComponent<NavMeshAgent>();

        agent.speed = moveSpeed;
        agent.isStopped = true;
    }

    public void MoveToDestination(Vector3 destination)
    {
        agent.isStopped = false;
        agent.SetDestination(destination);
    }

    void FixedUpdate()
    {
        if(agent.isStopped)
            rb.linearVelocity = new Vector3(moveDirection.x * moveSpeed, rb.linearVelocity.y,moveDirection.z* moveSpeed) ;
        else
        {
            OnDirectionChange?.Invoke(agent.desiredVelocity);
        }    
    }
    public void SetDirection(Vector3 direction)
    {
        if (!canMove)
            return;

        StopAgent();
        this.moveDirection = direction;
        OnDirectionChange?.Invoke(direction);
    }

    public void StopAgent()
    {
        agent.isStopped = true;
    }

    public void LockMovement(bool lockMovement)
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