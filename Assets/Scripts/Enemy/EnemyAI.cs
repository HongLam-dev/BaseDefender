using System;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    [SerializeField] private LayerMask oponentLayer;
    private Transform target;
    private EnemyCombat combat;
    private EnemyMovement movement;
    private Character character;
    private Health health;

    void Start()
    {
        movement = GetComponent<EnemyMovement>();
        combat = GetComponent<EnemyCombat>();
        character=GetComponent<Character>();
        health = GetComponent<Health>();

        health.OnHurt += HandleHurt;
    }

    void Update()
    {
        if (character.IsDead)
            return;
        if(target)
        {
            combat.TryAttackTarget(target);
            movement.FollowTarget(target);
        }
    }

    private void OnTriggerStay(Collider collision)
    { 
        Character detectedCharacter = collision.GetComponent<Character>();
        if (!target&&detectedCharacter&&detectedCharacter.TeamID!=character.TeamID)
        {
            target = collision.transform;
        }
    }

    private void HandleHurt(DamageInfo info)
    {
        if(info.caster.TeamID!=character.TeamID)
        {
            
        }
    }
}
