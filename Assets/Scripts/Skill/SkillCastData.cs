using UnityEngine;
public struct SkillCastData
{
    public ISkillCaster caster;
    public Vector3 targetPosition;
    public Transform target;

    public SkillCastData(ISkillCaster caster, Vector3 targetPosition, Transform target=null) {
        this.caster = caster;
        this.target = target;
        this.targetPosition = targetPosition; 
    }
}
