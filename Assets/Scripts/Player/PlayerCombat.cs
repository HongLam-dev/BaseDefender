using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.Windows;

public class PlayerCombat : CharacterCombat {

    [SerializeField] LayerMask targetLayer;

    public void BasicAttack()
    {
        TryUseSkill(0, Vector2.zero);
    }

    public void UseSkill2(Vector2 input)
    {

        TryUseSkill(1,input);
    }

    public void UseSkill3(Vector2 input)
    {
        TryUseSkill(2, input);
    }

    Transform FindSuitableTargetInCastRange(Vector3 casterPosition,float castRadius)
    {
        Collider[] hits= Physics.OverlapSphere(casterPosition, castRadius);
        foreach(Collider hit in hits)
        {
            if (hit.GetComponent<Character>())
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
