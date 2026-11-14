using System;
using Unity.VisualScripting;
using UnityEngine;
using static CharacterCombat;

public class Character: MonoBehaviour
{
    [SerializeField] int teamID=0;
    public int TeamID => teamID;

    [SerializeField] CharacterCombat combat;
    [SerializeField] IWorker worker;
    [SerializeField] Health health;
    [SerializeField] CharacterMovement movement;
    [SerializeField] float moveSpeed = 5;

    [SerializeField] float baseKnockBackForce = 1;
    [SerializeField] float knockBackResistance = 0;

    private Rigidbody rb;

    protected virtual void Start()
    {
        combat = GetComponent<CharacterCombat>();
        health = GetComponent<Health>();
        movement = GetComponent<CharacterMovement>();
        worker = GetComponent<IWorker>();
        rb = GetComponent<Rigidbody>();
        movement.SetMoveSpeed(moveSpeed);
        combat.TeamID = teamID;
        health.TeamID = teamID;
        combat.OnSkillFinish += HandleSkillFinish;
        combat.OnBeginUsingSkill += HandleSkillUsing;
        movement.OnMovement += HandleMovement;
        health.OnHurt += HandleHurt;
    }
    protected void HandleSkillFinish()
    {
        movement.SetSpeedMultiplier(1);
    }

    protected void HandleSkillUsing(object sender, SkillEventArgs args)
    {
        movement.SetSpeedMultiplier(args.MovementSpeedMultiplier);
    }

    protected void HandleMovement(Vector3 direction, float speedMultiplier)
    {
        if (worker != null)
            worker.StopWorking();
    }

    protected void HandleHurt(DamageInfo damageInfo)
    {
        movement.ApplyKnockback(damageInfo.direction.normalized, damageInfo.Force - knockBackResistance);
    }
}
