using UnityEngine;
public struct SkillCastData
{
    public ISkillCaster caster;
    public Vector3 targetPosition;
    public Transform target;
    public Vector3 projectileOrigin;
    public SkillCastData(ISkillCaster caster, Vector3 targetPosition,Vector3 projectileOrigin, Transform target=null) {
        this.caster = caster;
        this.target = target;
        this.targetPosition = targetPosition;
        this.projectileOrigin = projectileOrigin;
    }
}
