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

    [SerializeField] float targetCheckRadius;

    public event Action<string,Vector2> BeginUsingSkill;

    protected List<SkillRuntime> runtimeSkills = new();
    public IReadOnlyList<SkillRuntime> Skills => runtimeSkills;

    protected SkillRuntime activeSkill { get; private set; }
    private SkillCastData activeSkillCastData;

    protected bool isCastingSkill=false;

    private Health health;

    virtual protected void Awake()
    {
        health = GetComponent<Health>();
        if (health)
        {
            health.TeamID = teamID;
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
        activeSkill = null;   
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
            activeSkill.OnSkillFinish += FinishCurrentSkill;

            Vector2 direction = targetCastPosition - (Vector2)transform.position;
            BeginUsingSkill?.Invoke(skill.skillData.Animation, direction);
            return true;
        }
        return false;
    }

    private void HandleHurt(DamageInfo damageInfo)
    {
        FinishCurrentSkill();
    }

}
