using System;
using System.Collections;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using static CharacterCombat;

public class Character: MonoBehaviour
{
    [SerializeField] CharacterCombat combat;
    [SerializeField] IWorker worker;
    [SerializeField] Health health;
    [SerializeField] CharacterMovement movement;

    [SerializeField] private float afterDeathTime = 2f;
    public CharacterStats stats { get; private set; }

    private float stunTime = 0;
    public event Action OnStunned;
    public event Action OnStunRecover;
    public bool IsStunned => stunTime > 0;

    private void Start()
    {
        combat = GetComponent<CharacterCombat>();
        health = GetComponent<Health>();
        movement = GetComponent<CharacterMovement>();
        worker = GetComponent<IWorker>();
        stats = GetComponent<CharacterStats>();

        health.TeamID = stats.TeamID;
        health.Defense = stats.Defense;
        health.MaxHealth = stats.MaxHealth;

        movement.SetMoveSpeed(stats.moveSpeed);

        combat.OnSkillFinish += HandleSkillFinish;
        combat.OnBeginUsingSkill += HandleSkillUsing;
        movement.OnMovement += HandleMovement;
        health.OnHurt += HandleHurt;
        health.OnDeath += HandleDeath;
    }

    private void Update()
    {
        if(stunTime>0)
        {
            stunTime -= Time.deltaTime;
            if(stunTime<=0)
            {
                StunRecover();
            }
        }
    }


    private void Stun(float stunTime)
    {
        if (this.stunTime<=0)
        {
            if (worker != null)
                worker.StopWorking();
            movement.Stop();
            movement.SetSpeedMultiplier(0);
            combat.FinishCurrentSkill();
            OnStunned?.Invoke();
        }
        this.stunTime = math.max(this.stunTime, stunTime);
    }

    private void StunRecover()
    {
        movement.SetSpeedMultiplier(1);
        OnStunRecover?.Invoke();
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
        movement.ApplyKnockback(damageInfo.knockBackDirection.normalized, damageInfo.Force *stats.knockBackMultiplier);
        float stunTime = damageInfo.stunTime * stats.stunMultiplier;
        if(stunTime > 0.01)
        {
            Stun(stunTime);
        }
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
