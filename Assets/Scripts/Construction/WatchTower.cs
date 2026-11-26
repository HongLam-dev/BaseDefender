
using System;
using Unity.VisualScripting;
using UnityEngine;

public class WatchTower:Construction,ISkillCaster
{
    [SerializeField] int atk=50;
    [SerializeField] int knockBackForce=50;
    [SerializeField] SkillBase skill;
    [SerializeField] Transform towerFirePoint;
    [SerializeField] LineRenderer lineRenderer;

    private Animator animator;
    private SkillRuntime runtimeSkill;
    private SkillRuntime activeSkill;

    public Transform CasterTransform => towerFirePoint;
    public int TeamID => teamID;
    public int Atk => atk;
    public int KnockBackForce => knockBackForce;

    Health target;

    override protected void Start()
    {
        base.Start();
        runtimeSkill = new SkillRuntime(skill);
        animator = GetComponentInChildren<Animator>();
        GetComponentInChildren<SphereCollider>().radius = skill.CastRange;
        lineRenderer.SetPosition(0, towerFirePoint.position);
    }

    protected override void Update()
    {
        runtimeSkill.Cooldown(Time.deltaTime);
        if (target)
        {
            if(activeSkill == null && runtimeSkill.CanUse(transform.position, target.transform))
            {
                activeSkill = runtimeSkill;
                Rigidbody targetRb = target.GetComponent<Rigidbody>();
                Vector3 targetCastPosition = target.transform.position;
                if (targetRb)
                {
                    targetCastPosition = target.transform.position + skill.Stages[0] * targetRb.linearVelocity;
                    if (skill is SimpleProjectileSkill projectileSkill)
                    {
                        float t = (targetCastPosition - transform.position).magnitude / projectileSkill.ProjectileSpeed;
                        targetCastPosition = targetCastPosition + t * targetRb.linearVelocity;
                    }
                }

                activeSkill.SetCastData(new SkillCastData(this, targetCastPosition,towerFirePoint.position, target.transform));
                activeSkill.OnSkillFinish += FinishCurrentSkill;
                animator.SetTrigger("Attack");
            }
            lineRenderer.SetPosition(1, target.transform.position);
        }
        if(activeSkill!=null)
        {
            activeSkill.Excute(Time.fixedDeltaTime);
        }
    }    

    private void OnTriggerStay(Collider other)
    {
        Health targetHealth = other.GetComponent<Health>();
        if (!target&& targetHealth&& !targetHealth.IsDead &&targetHealth.TeamID!=teamID)
        {
            target = targetHealth;
            lineRenderer.enabled = true;
            target.OnDeath += LoseTarget;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(target&&target.transform==other.transform)
        {
            LoseTarget();
        }
    }
    public void FinishCurrentSkill()
    {
        if (activeSkill == null)
            return;
        activeSkill = null;
    }

    private void LoseTarget()
    {
        lineRenderer.enabled = false;
        target = null;
    }
}
