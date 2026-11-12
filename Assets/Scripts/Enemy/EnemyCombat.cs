
using UnityEngine;

public class EnemyCombat:CharacterCombat
{
    public override bool TryUseSkill(SkillRuntime skill, Vector3 targetCastPosition, Transform target = null)
    {

        if (!target||!skill.skillData.IsTargetInEffectiveRange(transform.position, target.transform.position))
            return false;
        return base.TryUseSkill(skill,targetCastPosition,target);
    }

    public bool TryAttackTarget(Transform target)
    {
        foreach(SkillRuntime skill in runtimeSkills)
        {
            if(TryUseSkill(skill,target.transform.position,target))
            {
                return true;
            }
        }
        return false;
    }
}
