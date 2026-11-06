using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public abstract class SkillBase : ScriptableObject
{
    [SerializeField]
    private string animation = "MeleeAttack";

    public string Animation => animation;
    [SerializeField]
    protected int baseDamage = 10;

    [SerializeField]
    protected int force = 1;

    [SerializeField]
    protected float baseCooldown = 1;

    public float BaseCooldown => baseCooldown;

    [SerializeField]
    protected float castRange = 5.0f;

    [SerializeField]
    private bool requireTarget = false;

    [SerializeField]
    private bool isHealingSkill = false;

    [SerializeField]
    private Sprite icon;

    public Sprite Icon=>icon;
    public bool IsHealingSkill => isHealingSkill;
    public bool RequireTarget => requireTarget;

    protected abstract int CalculateDamage(int atk);

    public abstract void ExcuteState(SkillCastData castData,int state);
 
    public virtual bool IsTargetInEffectiveRange(Vector2 casterPosition, Vector2 targetPosition)
    {
        Vector2 distance = targetPosition - casterPosition;
        if (math.abs(distance.x) > castRange)
            return false;
        return true;
    }
}

