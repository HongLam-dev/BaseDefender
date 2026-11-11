using UnityEngine;
public struct SkillCastData
{
    public CharacterCombat caster;
    public Vector3 targetPosition;
    public Transform target;

    public SkillCastData(CharacterCombat caster, Vector3 targetPosition, Transform target=null) {
        this.caster = caster;
        this.target = target;
        this.targetPosition = targetPosition; 
    }
}
