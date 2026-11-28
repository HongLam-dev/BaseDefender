
using System;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

public class WatchTower : Construction
{
    [SerializeField] Transform towerFirePoint;
    [SerializeField] LineRenderer lineRenderer;
    private Animator animator;
    Health target;
    private Collider targetCollider;
    NPCCombat combat;

    override protected void Start()
    {
        base.Start();
        animator = GetComponentInChildren<Animator>();
        combat = GetComponent<NPCCombat>();

        lineRenderer.SetPosition(0, towerFirePoint.position);
        combat.OnBeginUsingSkill += StartAttackAimation;
    }

    protected override void Update()
    {
        if (health.IsDead)
            return;
        if (target)
        {
            Vector3 closestPoint = targetCollider.ClosestPoint(transform.position);
            float distance = Vector3.Distance(transform.position, closestPoint);
            combat.TryUseAvailableSkill(target.transform,targetCollider);
            lineRenderer.SetPosition(1, target.transform.position);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        Health targetHealth = other.GetComponent<Health>();
        if (!target && targetHealth && !targetHealth.IsDead && targetHealth.TeamID != stats.TeamID)
        {
            target = targetHealth;
            targetCollider = target.GetComponent<Collider>();
            lineRenderer.enabled = true;
            target.OnDeath += LoseTarget;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (target && target.transform == other.transform)
        {
            LoseTarget();
        }
    }
    private void LoseTarget()
    {
        lineRenderer.enabled = false;
        target = null;
    }

    private void StartAttackAimation(object sender,CharacterCombat.SkillEventArgs args)
    {
        animator.SetTrigger("Attack");
    }
}
