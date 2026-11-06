using UnityEngine;
public struct SkillCastData
{
    public CharacterCombat caster;
    public Vector2 targetPosition;
    public Transform target;

    public SkillCastData(CharacterCombat caster, Vector2 targetPosition, Transform target=null) {
        this.caster = caster;
        this.target = target;
        this.targetPosition = targetPosition; 
    }
}
