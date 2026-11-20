
using UnityEngine;

public class EnemyCombat:CharacterCombat
{
    public bool NPCTryUseSkill(SkillRuntime skill, Vector3 targetCastPosition, Transform target)
    {
        if (target==null||!skill.skillData.IsTargetInEffectiveRange(transform.position, target.transform.position))
            return false;
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
