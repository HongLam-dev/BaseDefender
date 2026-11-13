using System;
using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.TextCore.Text;
using UnityEngine.UIElements;

public abstract class CharacterCombat: MonoBehaviour
{
    protected int teamID = 0;
    public int TeamID { set => teamID=value; get=>teamID; }

    [SerializeField]
    protected int atk = 10;
    public int Atk => atk;
    [SerializeField]
    protected SkillBase[] skills;

    [SerializeField] float targetCheckRadius;

    public event Action OnSkillFinish;

    protected List<SkillRuntime> runtimeSkills = new();
    public IReadOnlyList<SkillRuntime> Skills => runtimeSkills;

    protected SkillRuntime activeSkill { get; private set; }
    private SkillCastData activeSkillCastData;

    protected bool isCastingSkill=false;

    private Health health;

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
        health = GetComponent<Health>();
        if (health)
        {
            health.OnHurt += HandleHurt;
        }

        foreach (SkillBase skillData in skills)
        {
            runtimeSkills.Add(new SkillRuntime(skillData));
        }
    }

    virtual protected void Update()
    {
        if(activeSkill!=null)
        {
            activeSkill.Excute(Time.deltaTime,activeSkillCastData);
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

    private void FinishCurrentSkill()
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
            activeSkillCastData=new SkillCastData(this, targetCastPosition,target);
            activeSkill = skill;
            activeSkill.OnSkillFinish += FinishCurrentSkill;

            Vector3 direction = targetCastPosition - transform.position;
            OnBeginUsingSkill?.Invoke(this,new SkillEventArgs( skill.skillData.Animation, direction,skill.skillData.SpeedMultiplier));
            return true;
        }
        return false;
    }

    private void HandleHurt(DamageInfo damageInfo)
    {
        FinishCurrentSkill();
    }

}
