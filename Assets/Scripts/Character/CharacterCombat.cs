using System;
using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.TextCore.Text;
using UnityEngine.UIElements;

public abstract class CharacterCombat: MonoBehaviour,ISkillCaster
{
    public int TeamID => stats.TeamID;
    public Transform CasterTransform => transform;
    public int KnockBackForce => stats.KnockBackForce;
    public int Atk => stats.Atk; 
    [SerializeField] protected SkillBase[] skills;
    [SerializeField] Transform projectileOrigin;
    protected CharacterStats stats;

    public event Action OnSkillFinish;

    protected List<SkillRuntime> runtimeSkills = new();
    public IReadOnlyList<SkillRuntime> Skills => runtimeSkills;

    protected SkillRuntime activeSkill { get; private set; }

    protected bool isCastingSkill=false;
    public event EventHandler<SkillEventArgs> OnBeginUsingSkill;
    public class SkillEventArgs : EventArgs
    {
        public string AnimationName { get; }
        public Vector3 CastDirection { get; }
        public float MovementSpeedMultiplier { get; }

        public SkillEventArgs(
            string animationName,
            Vector3 direction,
            float movementSpeedMultiplier)
        {
            AnimationName = animationName;
            CastDirection = direction;
            MovementSpeedMultiplier = movementSpeedMultiplier;
        }
    }

    virtual protected void Awake()
    {
        stats = GetComponent<CharacterStats>();

        foreach (SkillBase skillData in skills)
        {
            runtimeSkills.Add(new SkillRuntime(skillData));
        }

        if (!projectileOrigin)
            projectileOrigin = transform;
    }

    virtual protected void Update()
    {
        if(activeSkill!=null)
        {
            activeSkill.Excute(Time.deltaTime);
        }
        CoolDownSkills(Time.deltaTime);
    }

    private void CoolDownSkills(float deltaTime)
    {
        foreach (SkillRuntime skill in runtimeSkills)
        {
            skill.Cooldown(deltaTime);
        }
    }

    public void FinishCurrentSkill()
    {
        if (activeSkill==null)
            return;
        OnSkillFinish?.Invoke();
        activeSkill = null;   
    }

    public virtual bool TryUseSkill(SkillRuntime skill,Vector3 targetCastPosition, Transform target=null)
    {
        if (activeSkill==null&& skill.CanUse(transform.position,target))
        {
            activeSkill = skill;
            activeSkill.SetCastData(new SkillCastData(this, targetCastPosition,projectileOrigin.position, target));
            activeSkill.OnSkillFinish += FinishCurrentSkill;

            Vector3 direction = targetCastPosition - transform.position;
            OnBeginUsingSkill?.Invoke(this,new SkillEventArgs( skill.skillData.Animation, direction.normalized,skill.skillData.SpeedMultiplier));
            return true;
        }
        return false;
    }
}
