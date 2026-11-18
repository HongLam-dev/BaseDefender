using System;
using System.Collections;
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

    [SerializeField] private float afterDeathTime = 2f;
    public CharacterStats stats { get; private set; }

    protected virtual void Start()
    {
        combat = GetComponent<CharacterCombat>();
        health = GetComponent<Health>();
        movement = GetComponent<CharacterMovement>();
        worker = GetComponent<IWorker>();
        stats = GetComponent<CharacterStats>();

        health.TeamID = teamID;
        movement.SetMoveSpeed(stats.moveSpeed);

        combat.OnSkillFinish += HandleSkillFinish;
        combat.OnBeginUsingSkill += HandleSkillUsing;
        movement.OnMovement += HandleMovement;
        health.OnHurt += HandleHurt;
        health.OnDeath += HandleDeath;
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
        movement.ApplyKnockback(damageInfo.knockBackDirection.normalized, damageInfo.Force - stats.knockBackResistance);
    }

    protected void HandleDeath()
    {
        movement.Stop();
        movement.enabled = false;
        combat.enabled = false;
        if (worker != null)
            worker.StopWorking();
        StartCoroutine(DeathCoroutine());
    }

    private IEnumerator DeathCoroutine()
    {
        yield return new WaitForSeconds(afterDeathTime);
        gameObject.SetActive(false);
    }
}
