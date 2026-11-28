
using Unity.VisualScripting;
using UnityEngine;

public class NPCCombat:CharacterCombat
{
    public bool TryUseSkillOnTarget(SkillRuntime skill, Transform target, Collider targetCollider)
    {
        Vector3 closestPoint = targetCollider.ClosestPoint(transform.position);
        float distance = Vector3.Distance(transform.position, closestPoint);
        if (target==null|| !skill.CanUse(transform.position, target) || !skill.skillData.IsTargetInEffectiveRange(distance))
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

    public bool TryUseAvailableSkill(Transform target, Collider targetCollider)
    {
        foreach(SkillRuntime skill in runtimeSkills)
        {
            if(TryUseSkillOnTarget(skill,target,targetCollider))
            {
                return true;
            }
        }
        return false;
    }
}
