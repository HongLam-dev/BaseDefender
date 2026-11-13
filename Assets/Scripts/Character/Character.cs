using System;
using UnityEngine;
using static CharacterCombat;

public class Character: MonoBehaviour
{
    [SerializeField] int teamID=0;
    public int TeamID => teamID;

    [SerializeField] CharacterCombat combat;
    [SerializeField] Health health;
    [SerializeField] CharacterMovement movement;
    [SerializeField] float moveSpeed = 5;


    protected virtual void Start()
    {
        combat = GetComponent<CharacterCombat>();
        health = GetComponent<Health>();
        movement = GetComponent<CharacterMovement>();
        movement.SetMoveSpeed(moveSpeed);
        combat.TeamID = teamID;
        health.TeamID = teamID;
        combat.OnSkillFinish += HandleSkillFinish;
        combat.OnBeginUsingSkill += HandleSkillUsing;
    }
    protected void HandleSkillFinish()
    {
        movement.SetSpeedMultiplier(1);
    }

    protected void HandleSkillUsing(object sender, SkillEventArgs args)
    {
        movement.SetSpeedMultiplier(args.MovementSpeedMultiplier);
    }
}
