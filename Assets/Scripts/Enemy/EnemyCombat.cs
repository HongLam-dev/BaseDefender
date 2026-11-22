
using UnityEngine;

public class EnemyCombat:CharacterCombat
{
    public bool NPCTryUseSkill(SkillRuntime skill, Vector3 targetCastPosition, Transform target)
    {
        if (target==null|| !skill.CanUse(transform.position, target) || !skill.skillData.IsTargetInEffectiveRange(transform.position, target.transform.position))
            return false;
        Rigidbody targetRb = target.GetComponent<Rigidbody>();
        targetCastPosition = target.transform.position + skill.skillData.Stages[0] * targetRb.linearVelocity;
        if (targetRb&& skill.skillData is SimpleProjectileSkill projectileSkill)
        {
            float t = (targetCastPosition - transform.position).magnitude / projectileSkill.ProjectileSpeed;
            targetCastPosition = targetCastPosition + t * targetRb.linearVelocity;
        }

        return TryUseSkill(skill,targetCastPosition,target);
    }

    public bool TryAttackTarget(Transform target)
    {
        foreach(SkillRuntime skill in runtimeSkills)
        {
            if(NPCTryUseSkill(skill,target.transform.position,target))
            {
                return true;
            }
        }
        return false;
    }
}
