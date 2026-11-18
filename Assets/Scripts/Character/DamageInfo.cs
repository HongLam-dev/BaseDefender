using UnityEngine;
public struct DamageInfo
{
    public ISkillCaster caster;
    public int Damage;
    public float Force;

    public Vector3 knockBackDirection;
    public DamageInfo(ISkillCaster caster, int damge, float force,Vector3 knockBackDirection)
    {
        this.caster = caster;
        Damage = damge;
        Force = force;
        this.knockBackDirection = knockBackDirection;
    }
}