using System;
using System.Linq;
using UnityEngine;
[System.Serializable]
public class SkillRuntime
{
    public SkillBase skillData;
    private float cooldownRemaining=0;
    private int currentStage;
    private float timer=0;

    public float CooldownRemaining => cooldownRemaining;
    public float SkillCooldown => skillData.BaseCooldown;

    public event Action OnSkillFinish;

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
        return cooldownRemaining<=0;
    }

    public void Excute(float deltaTime, SkillCastData castData)
    {
        timer += deltaTime;
        for(int i=0;i<skillData.Stages.Count();i++)
        {
            if(timer>= skillData.Stages[i]&&currentStage<i)
            {
                currentStage = i;
                skillData.ExcuteStage(castData, currentStage);
                return;
            }
        }
        Finish();
    }

    public void Cooldown(float deltaTime)
    {
        if (cooldownRemaining > 0)
        {
            cooldownRemaining -= deltaTime;
        }
    }

    public SkillRuntime(SkillBase skillData) {
        this.skillData = skillData;
    }

    public void Finish()
    {
        timer = 0;
        currentStage = 0;
        cooldownRemaining = skillData.BaseCooldown;
        OnSkillFinish?.Invoke();
    }
}