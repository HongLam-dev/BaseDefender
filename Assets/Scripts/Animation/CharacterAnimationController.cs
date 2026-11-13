using System;
using System.Collections;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.EventSystems;
using static CharacterCombat;

public class CharacterAnimationController : MonoBehaviour
{
    [SerializeField] private Animator animator;

    //[SerializeField] private float hitEffectTime = 0.2f;
    private float currentAnimationSpeed;
    [SerializeField] private Color hitEffectColor = Color.red;

    [SerializeField] private float rotationSpeed = 5.0f;
    private CharacterEquipment characterEquipment;

    private Health heatlh;
    private Vector2 currentDirection = Vector2.down;
    CharacterMovement movement;
    CharacterCombat combat;
    IWorker worker;

    void Start()
    {
        animator=GetComponent<Animator>();
        characterEquipment = GetComponent<CharacterEquipment>();
        movement = GetComponent<CharacterMovement>();
        combat = GetComponent<CharacterCombat>();
        worker = GetComponent<IWorker>();

        movement.OnMovement += HandleMovement;
        combat.OnBeginUsingSkill += SetSkillAnimation;

        if(worker!=null)
        {
            worker.OnWork += StartWorkAnimation;
            worker.OnStopWorking += StopWorkAnimation;
        }
    }

    public void HandleMovement(Vector3 moveDirection,float speedMultiplier)
    {
        RotateCharacter(moveDirection,false);
        animator.SetFloat("SpeedMultiplier", moveDirection.sqrMagnitude>0.001 ? speedMultiplier : 0f);
    }

    private bool HasTrigger(string triggerName)
    {
        foreach (AnimatorControllerParameter parameter in animator.parameters)
        {
            if (parameter.name == triggerName &&
                parameter.type == AnimatorControllerParameterType.Trigger)
            {
                return true;
            }
        }

        return false;
    }

    private void HandleHurt(DamageInfo damageInfo)
    {
        //SetAnimation(AnimationState.Hurt);
      //  StartCoroutine(HurtEffect());
    }

 /*   private IEnumerator HurtEffect()
    {
   
    }*/

    private void StartWorkAnimation(WorkInfo info)
    {
        RotateCharacter(info.position-transform.position, true);

        switch(info.type)
        {
            case WorkType.Lumbering:
                characterEquipment.EquipAxe();
                SetAnimation(AnimationState.Chopping);
                break;
            case WorkType.Mining:
                characterEquipment.EquipPickaxe();
                SetAnimation(AnimationState.Mining);
                break;
        }

    }
    private void StopWorkAnimation(WorkInfo info)
    {
        switch (info.type)
        {
            case WorkType.Lumbering:
                animator.SetBool("Chopping",false);
                characterEquipment.UnequipAxe();
                break;
            case WorkType.Mining:
                animator.SetBool("Mining", false);
                characterEquipment.UnequipPickaxe();
                break;
        }

    }

    private void HandleDie()
    {
        SetAnimation(AnimationState.Dead);
    }

    public void DieAnimationFinish()
    {
        transform.parent.gameObject.SetActive(false);
    }
    private void SetSkillAnimation(object sender, SkillEventArgs args)
    {
        RotateCharacter(args.CastDirection, true);
        animator.SetTrigger(args.AnimationName);
    }

    private void SetAnimation(AnimationState state)
    {
        if (state == AnimationState.Hurt)
        {
            animator.SetTrigger("GetHit");
        }
        else if (state == AnimationState.Mining)
        {
            characterEquipment.EquipPickaxe();
            animator.SetBool("Mining", true);
        }
        else if (state == AnimationState.Chopping)
        {
            characterEquipment.EquipAxe();
            animator.SetBool("Chopping", true);
        }
        else if (state == AnimationState.Dead)
        {
            animator.SetBool("IsDead", true);
        }
    }

    private void RotateCharacter(Vector3 direction, bool instantRotate)
    {
        if (direction.sqrMagnitude < 0.001f)
            return;

        Quaternion targetRotation =
            Quaternion.LookRotation(direction.normalized);

        if (!instantRotate)
            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                targetRotation,
                rotationSpeed * Time.deltaTime
            );
        else
            transform.rotation = targetRotation;
    }
}
