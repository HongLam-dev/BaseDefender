using System;
using UnityEngine;

public class Character: MonoBehaviour
{
    [SerializeField] int teamID=0;
    public int TeamID => teamID;

    [SerializeField] CharacterCombat combat;
    [SerializeField] Health health;
    [SerializeField] CharacterMovement movement;
    [SerializeField] float moveSpeed = 5;

    private void Start()
    {
        combat = GetComponent<CharacterCombat>();
        health = GetComponent<Health>();
        movement = GetComponent<CharacterMovement>();
        movement.SetMoveSpeed(moveSpeed);
        combat.TeamID = teamID;
        health.TeamID = teamID;
    }

}
