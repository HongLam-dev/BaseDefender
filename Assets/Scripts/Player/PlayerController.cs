using System;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;
using UnityEngine.EventSystems;
public class PlayerController:MonoBehaviour
{
    [SerializeField] PlayerCombat combat;
    [SerializeField] IWorker worker;
    [SerializeField] CharacterMovement movement;
    [SerializeField] Character character;

    private void Start()
    {
        combat = GetComponent<PlayerCombat>();
        movement = GetComponent<CharacterMovement>();
        worker = GetComponent<IWorker>();
        character = GetComponent<Character>();
    }

    public void OnMove(Vector2 movement)
    {
        if (character.IsStunned)
            return;
        this.movement.SetDirection(new Vector3(movement.x, 0, movement.y).normalized);
    }

    public void OnIdle(Vector2 previousDirection)
    {
        if (character.IsStunned)
            return;
        movement.SetDirection(Vector3.zero);
    }

    public void BasicAttack()
    {
        if (character.IsStunned)
            return;
        combat.TryUseSkill(0, Vector2.zero);
    }

    public void UseSkill2(Vector2 input)
    {
        if (character.IsStunned)
            return;
        combat.TryUseSkill(1, input);
    }

    public void UseSkill3(Vector2 input)
    {
        if (character.IsStunned)
            return;
        combat.TryUseSkill(2, input);
    }
}