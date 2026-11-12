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

    bool isUsingSkill=false;
    void Start()
    {
        movement = GetComponent<EnemyMovement>();
        combat = GetComponent<EnemyCombat>();
        character=GetComponent<Character>();
        combat.OnSkillFinish += HandleSkillFinish;
    }

    void Update()
    {
        if(target)
        {
            if(combat.TryAttackTarget(target))
            {
                movement.Stop();
                isUsingSkill = true;
            }
            else if(!isUsingSkill)
            {
                movement.FollowTarget(target);
            }
        }
    }

    void HandleSkillFinish()
    {
        isUsingSkill = false;
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
