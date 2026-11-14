using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;
public class EnemyMovement:CharacterMovement
{
    [SerializeField] NavMeshAgent agent;
    [SerializeField] float stoppingDistance = 1.5f;
    private Transform target;
    private Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        agent.stoppingDistance = stoppingDistance;
    }
    public override void SetMoveSpeed(float speed)
    {
        agent.speed = speed;
        base.SetMoveSpeed(speed);
    }

    public void MoveToDestination(Vector3 destination)
    {
        agent.SetDestination(destination);
        agent.isStopped = false;
    }

    public void FollowTarget(Transform target)
    {
        this.target = target;
    }

    private void Update()
    {
        SetDirection(agent.velocity*speedMultiplier);
        if(target)
        {
            MoveToDestination(target.position);
        }
    }

    public void Stop()
    {
        SetDirection(Vector3.zero);
        agent.isStopped = true;
        target = null;
    }

    public override void SetSpeedMultiplier(float multiplier)
    {
        agent.speed = moveSpeed*multiplier;
        speedMultiplier = multiplier;
    }

    private void FixedUpdate()
    {
        rb.linearVelocity = externalVelocity;
        externalVelocity = Vector3.zero;
    }
}
