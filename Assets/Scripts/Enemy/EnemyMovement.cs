using System;
using UnityEngine;
using UnityEngine.AI;
public class EnemyMovement:CharacterMovement
{
    [SerializeField] NavMeshAgent agent;

    private Transform target;
    private float stopRange= 2f;

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

    public void FollowTarget(Transform target, float stopRange=2f)
    {
        this.target = target;
        this.stopRange = stopRange;
        agent.isStopped = false;
    }

    private void Update()
    {
        if(!agent.isStopped)
            SetDirection(agent.velocity);
        if(target)
        {
            if(Vector3.Distance(transform.position,target.position)<=stopRange)
            {
                Stop();
            }
            else
                agent.SetDestination(target.position);
        }
    }

    public void Stop()
    {
        SetDirection(Vector3.zero);
        agent.isStopped = true;
        target = null;
    }
}
