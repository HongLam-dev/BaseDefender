using System;
using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.TextCore.Text;
using UnityEngine.UIElements;

public class CharacterCombat: MonoBehaviour
{
    [SerializeField]
    protected int teamID = 0;
    public int TeamID => teamID;

    [SerializeField]
    protected int atk = 10;
    public int Atk => atk;
    [SerializeField]
    protected SkillBase[] skills;

    public event Action<string,Vector2> BeginUsingSkill;

    protected List<SkillRuntime> runtimeSkills = new();
    public IReadOnlyList<SkillRuntime> Skills => runtimeSkills;

    protected SkillRuntime activeSkill { get; private set; }
    private SkillCastData activeSkillCastData;

    protected bool isCastingSkill=false;

    private Health health;
    private CharacterAnimationController animator;

    virtual protected void Awake()
    {
        health = GetComponent<Health>();
        animator = GetComponentInChildren<CharacterAnimationController>();

        if (health)
        {
            health.TeamID = teamID;
            health.OnHurt += HandleHurt;
        }
        if(animator)
        {
            animator.SkillAnimationFinishCallback += FinishCurrentSkill;
            animator.SkillAnimationEvent += RunCurrentSkill;
        }

        foreach (SkillBase skillData in skills)
        {
            runtimeSkills.Add(new SkillRuntime(skillData));
        }
    }

    virtual protected void Update()
    {
        CoolDownSkills(Time.deltaTime);
    }

    private void CoolDownSkills(float deltaTime)
    {
        foreach (SkillRuntime skill in runtimeSkills)
        {
            skill.Tick(deltaTime);
        }
    }

    private void FinishCurrentSkill()
    {
        if (activeSkill==null)
            return;
        activeSkill.Cooldown();
        activeSkill = null;   
    }

    private void RunCurrentSkill(int stage)
    {
        if (activeSkill == null)
            return;
        activeSkill.ExcuteState(activeSkillCastData,stage);
    }

    public virtual bool TryUseSkill(SkillRuntime skill,Vector2 targetCastPosition, Transform target=null,bool requireTargetInCastRange=false)
    {
        if (activeSkill==null&& skill.CanUse(transform.position,target))
        {
            if(requireTargetInCastRange&& !skill.skillData.IsTargetInEffectiveRange(transform.position, target.position))
            {
                return false;
            }

            activeSkillCastData=new SkillCastData(this, targetCastPosition,target);
            activeSkill = skill;
            Vector2 direction = targetCastPosition - (Vector2)transform.position;
            BeginUsingSkill?.Invoke(skill.skillData.Animation, direction);
            return true;
        }
        return false;
    }

    public virtual bool TryUseSkill(int skillIndex, Vector2 targetCastPosition, Transform target = null, bool requireTargetInCastRange = false)
    {
        if (skillIndex > skills.Count() - 1)
            return false;
        return TryUseSkill(runtimeSkills[skillIndex],targetCastPosition,target,requireTargetInCastRange);
    }

    private void HandleHurt(DamageInfo damageInfo)
    {
        FinishCurrentSkill();
    }

    private void OnDestroy()
    {
        if (animator)
        {
            animator.SkillAnimationFinishCallback -= FinishCurrentSkill;
            health.OnHurt -= HandleHurt;
        }
    }
}
