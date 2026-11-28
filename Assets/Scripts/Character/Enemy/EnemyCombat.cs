
using Unity.VisualScripting;
using UnityEngine;

public class EnemyCombat:CharacterCombat
{
    public bool NPCTryUseSkill(SkillRuntime skill, Transform target, float surfaceDistance)
    {
        if (target==null|| !skill.CanUse(transform.position, target) || !skill.skillData.IsTargetInEffectiveRange(surfaceDistance))
            return false;
        Rigidbody targetRb = target.GetComponent<Rigidbody>();
        Vector3 targetCastPosition = target.transform.position;
        if (targetRb)
        {
            targetCastPosition = target.transform.position + skill.skillData.Stages[0] * targetRb.linearVelocity;
            if(skill.skillData is DirectionProjectileSkill projectileSkill)
            {
                float t = (targetCastPosition - transform.position).magnitude / projectileSkill.ProjectileSpeed;
                targetCastPosition = targetCastPosition + t * targetRb.linearVelocity;
            }
        }

        return TryUseSkill(skill,targetCastPosition,target);
    }

    public bool TryAttackTarget(Transform target,float surfaceDistance)
    {
        foreach(SkillRuntime skill in runtimeSkills)
        {
            if(NPCTryUseSkill(skill,target,surfaceDistance))
            {
                return true;
            }
        }
        return false;
    }
}
