using System;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    [SerializeField] float safeStoppingDistanceOffset = 0.3f;
    private Health target;
    private EnemyCombat combat;
    private EnemyMovement movement;
    private Character character;
    private Health health;

    void Start()
    {
        movement = GetComponent<EnemyMovement>();
        combat = GetComponent<EnemyCombat>();
        character=GetComponent<Character>();
        health = GetComponent<Health>();

        health.OnHurt += HandleHurt;

        float smallestSkillRange = combat.Skills[0].skillData.CastRange;
        foreach (SkillRuntime skill in combat.Skills)
        {
            if(skill.skillData.CastRange<smallestSkillRange)
            {
                smallestSkillRange = skill.skillData.CastRange;
            }
        }
        movement.SetStoppingDistance(smallestSkillRange-safeStoppingDistanceOffset);
    }

    void Update()
    {
        if (health.IsDead||character.IsStunned)
            return;
        if(target)
        {
            if (target.IsDead)
            {
                target = null;
            }
            else
            {
                combat.TryAttackTarget(target.transform);
                movement.FollowTarget(target.transform);
            }

        }
    }

    private void OnTriggerStay(Collider collision)
    { 
        Health oponentUnit = collision.GetComponent<Health>();
        if (!target&&oponentUnit&&oponentUnit.TeamID!=character.TeamID)
        {
            target = oponentUnit;
        }
    }

    private void HandleHurt(DamageInfo info)
    {
        if(info.caster.TeamID!=character.TeamID)
        {
            target = info.caster.CasterTransform.GetComponent<Health>();
        }
    }
}
