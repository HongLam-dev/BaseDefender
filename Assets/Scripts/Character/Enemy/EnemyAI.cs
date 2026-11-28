using System;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;
using static UnityEngine.AdaptivePerformance.Provider.AdaptivePerformanceSubsystemDescriptor;

public class EnemyAI : MonoBehaviour
{
    [SerializeField] float safeStoppingDistanceOffset = 0.3f;
    private Health target;
    private NPCCombat combat;
    private EnemyMovement movement;
    private Character character;
    private Health health;
    private Collider targetCollider;
    private CharacterStats stats;
    float smallestSkillRange;

    void Start()
    {
        movement = GetComponent<EnemyMovement>();
        combat = GetComponent<NPCCombat>();
        character=GetComponent<Character>();
        health = GetComponent<Health>();
        stats = GetComponent<CharacterStats>();
        health.OnHurt += HandleHurt;

        smallestSkillRange = combat.Skills[0].skillData.CastRange;
        foreach (SkillRuntime skill in combat.Skills)
        {
            if(skill.skillData.CastRange<smallestSkillRange)
            {
                smallestSkillRange = skill.skillData.CastRange;
            }
        }
    }

    void Update()
    {
        if (health.IsDead||character.IsStunned)
            return;
        if(target)
        {
            Vector3 closestPoint = targetCollider.ClosestPoint(transform.position);
            float distance = Vector3.Distance(transform.position, closestPoint);
            if (distance <=smallestSkillRange-safeStoppingDistanceOffset)
            {
                combat.TryUseAvailableSkill(target.transform, targetCollider);
                movement.Stop();
            }
            else
            {
                movement.FollowTarget(target.transform);
            }
        }
    }

    private void OnTriggerStay(Collider collision)
    { 
        Health oponentUnit = collision.GetComponent<Health>();
        if (!target&& oponentUnit &&!oponentUnit.IsDead && oponentUnit.TeamID!=stats.TeamID)
        {
            SetTarget(oponentUnit);
        }
    }

    private void HandleHurt(DamageInfo info)
    {
        if(info.caster.TeamID!=stats.TeamID)
        {
            SetTarget(info.caster.CasterTransform.GetComponent<Health>());
        }
    }
    
    private void SetTarget(Health target)
    {
        if (target)
            target.OnDeath -= LoseTarget;
        targetCollider = target.GetComponent<Collider>();
        this.target = target;
        target.OnDeath += LoseTarget;
    }

    private void LoseTarget()
    {
        target = null;
    }
}
