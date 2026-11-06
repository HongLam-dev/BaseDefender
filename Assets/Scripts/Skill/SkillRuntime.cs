using UnityEngine;
[System.Serializable]
public class SkillRuntime
{
    public SkillBase skillData;
    public float cooldownRemaining=0;

    public float CooldownProgress => cooldownRemaining/skillData.BaseCooldown;

    public bool CanUse(Vector2 casterPosition, Transform target)
    {
        if (skillData.RequireTarget)
        {
            if (!target
                || !target.GetComponent<Health>()
                || !skillData.IsTargetInEffectiveRange(casterPosition, target.position)       
               )
                return false;
        }
        return CooldownProgress<=0;
    }

    public void ExcuteState(SkillCastData castData,int stage)
    {
        skillData.ExcuteState(castData,stage);
    }
    public void Tick(float deltaTime)
    {
        if (cooldownRemaining > 0)
            cooldownRemaining -= deltaTime;
    }

    public SkillRuntime(SkillBase skillData) {
        this.skillData = skillData;
    }

    public void Cooldown()
    {
        cooldownRemaining = skillData.BaseCooldown;
    }
}