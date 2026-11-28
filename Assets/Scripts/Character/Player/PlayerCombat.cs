using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.Windows;

public class PlayerCombat : CharacterCombat {

    Transform FindSuitableTargetInCastRange(Vector3 casterPosition,float castRadius)
    {
        Collider[] hits= Physics.OverlapSphere(casterPosition, castRadius);
        foreach(Collider hit in hits)
        {
            Health hitTarget = hit.GetComponent<Health>();
            if (hitTarget&&hitTarget.TeamID!=TeamID)
            {
                return hit.transform;
            }
        }
        return null;
    }

    public bool TryUseSkill(int skillIndex,Vector2 input)
    {
        if (skillIndex >= skills.Count())
            return false;

        Transform target = FindSuitableTargetInCastRange(transform.position, skills[skillIndex].CastRange);
        if (!runtimeSkills[skillIndex].CanUse(transform.position, target))
            return false;

        Vector3 targetPosition=transform.position;
        if (runtimeSkills[skillIndex].skillData.IsSkillSHot)
        {
            if(target)
            {
                targetPosition = target.position;
            }
        }
        else
        {
            float distance = input.magnitude * skills[skillIndex].CastRange;

            Vector3 direction = new Vector3(input.x, 0f, input.y);
            targetPosition =
            transform.position + direction.normalized * distance;
        }
        return TryUseSkill(runtimeSkills[skillIndex], targetPosition, target);
    }
}
