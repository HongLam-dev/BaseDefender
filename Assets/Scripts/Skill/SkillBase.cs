using System;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public abstract class SkillBase : ScriptableObject
{
    [SerializeField]  private string animation = "MeleeAttack";

    [SerializeField] float[] stages;

    [SerializeField] protected int baseDamage = 10;

    [SerializeField] protected int force = 1;

    [SerializeField] protected float baseCooldown = 1;

    [SerializeField] protected float castRange = 5.0f;

    [SerializeField]  private bool requireTarget = false;
    [SerializeField] private float speedMultiplier = 0;
    [SerializeField] private bool isSkillshot = true;
    [SerializeField] private bool isHealingSkill = false;

    [SerializeField] private Sprite icon;

    public Sprite Icon=>icon;
    public bool IsHealingSkill => isHealingSkill;
    public bool RequireTarget => requireTarget;
    public float BaseCooldown => baseCooldown;
    public float CastRange => castRange;
    public float[] Stages => stages;
    public float SpeedMultiplier => speedMultiplier;
    public string Animation => animation;
    public bool IsSkillSHot => isSkillshot;

    protected abstract int CalculateDamage(int atk);

    public abstract void ExcuteStage(SkillCastData castData,int state);
 
    public virtual bool IsTargetInEffectiveRange(Vector3 casterPosition, Vector3 targetPosition)
    {
        if (Vector3.Distance(casterPosition,targetPosition)>castRange)
            return false;
        return true;
    }

    protected virtual DamageInfo CalculateDamgeInfo(SkillCastData castData)
    {
        return new DamageInfo(castData.caster.TeamID, CalculateDamage(castData.caster.Atk), force,castData.targetPosition- castData.caster.transform.position);
    }
}

