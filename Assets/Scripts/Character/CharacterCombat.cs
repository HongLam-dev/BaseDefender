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
    public int TeamID => character.TeamID;
    public Transform CasterTransform => transform;
    public Vector3 Forward => transform.forward;
    public int KnockBackForce => character.stats.KnockBackForce;
    public int Atk => character.stats.Atk; 
    [SerializeField] protected SkillBase[] skills; 

    protected Character character;

    public event Action OnSkillFinish;

    protected List<SkillRuntime> runtimeSkills = new();
    public IReadOnlyList<SkillRuntime> Skills => runtimeSkills;

    protected SkillRuntime activeSkill { get; private set; }
    private SkillCastData activeSkillCastData;

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
        character = GetComponent<Character>();

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
            activeSkillCastData=new SkillCastData(this, targetCastPosition,target);
            activeSkill = skill;
            activeSkill.OnSkillFinish += FinishCurrentSkill;

            Vector3 direction = targetCastPosition - transform.position;
            OnBeginUsingSkill?.Invoke(this,new SkillEventArgs( skill.skillData.Animation, direction.normalized,skill.skillData.SpeedMultiplier));
            return true;
        }
        return false;
    }
}
