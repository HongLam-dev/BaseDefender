using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;
public class EnemyMovement:CharacterMovement
{
    [SerializeField] NavMeshAgent agent;
    private Transform target;

    override protected void Start()
    {
        agent.updatePosition = false;
        agent.updateRotation = false;
        base.Start();
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
        if(target)
        {
            MoveToDestination(target.position);
        }
        SetDirection(agent.velocity);
    }

    public override void Stop()
    {
        SetDirection(Vector3.zero);
        agent.isStopped = true;
        target = null;
    }

    public void SetStoppingDistance(float stoppingDistance)
    {
        agent.stoppingDistance = stoppingDistance;
    }

    override protected void FixedUpdate()
    {
        base.FixedUpdate();
        agent.nextPosition = rb.position; 
    }
}
