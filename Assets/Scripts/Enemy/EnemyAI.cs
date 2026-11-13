using System;
using UnityEngine;

[RequireComponent(typeof(Character))]
[RequireComponent(typeof(EnemyCombat))]
[RequireComponent(typeof(EnemyMovement))]
public class EnemyAI : MonoBehaviour
{
    [SerializeField] private LayerMask oponentLayer;
    private Transform target;
    private EnemyCombat combat;
    private EnemyMovement movement;
    private Character character;

    void Start()
    {
        movement = GetComponent<EnemyMovement>();
        combat = GetComponent<EnemyCombat>();
        character=GetComponent<Character>();
    }

    void Update()
    {
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


}
