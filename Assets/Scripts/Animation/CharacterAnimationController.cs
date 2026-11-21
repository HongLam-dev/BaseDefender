using System;
using System.Collections;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.EventSystems;
using static CharacterCombat;

public class CharacterAnimationController : MonoBehaviour
{
    [SerializeField] private Animator animator;

    [SerializeField] private float rotationSpeed = 5.0f;

    private Character character;
    private CharacterEquipment characterEquipment;
    private Health heatlh;
    private CharacterMovement movement;
    private CharacterCombat combat;
    IWorker worker;

    [Header("Hurt Effect")]
    [SerializeField] private Color flashColor = Color.red;
    [SerializeField] private float flashDuration = 0.15f;

    private SkinnedMeshRenderer[] renderers;
    private MaterialPropertyBlock propertyBlock;
    private Coroutine flashCoroutine;

    private static readonly int BaseColor =
        Shader.PropertyToID("_BaseColor");

    private void Awake()
    {
        renderers = GetComponentsInChildren<SkinnedMeshRenderer>();
        propertyBlock = new MaterialPropertyBlock();
    }

    void Start()
    {
        animator = GetComponentInChildren<Animator>();
        characterEquipment = GetComponent<CharacterEquipment>();
        movement = GetComponent<CharacterMovement>();
        combat = GetComponent<CharacterCombat>();
        worker = GetComponent<IWorker>();
        heatlh= GetComponent<Health>();
        character = GetComponent<Character>();

        movement.OnMovement += HandleMovement;
        combat.OnBeginUsingSkill += SetSkillAnimation;
        heatlh.OnHurt += HandleHurt;
        heatlh.OnDeath += HandleDeath;
        character.OnStunned += HandleStunned;
        character.OnStunRecover += HandleStunRecorver;

        if(worker!=null)
        {
            worker.OnWork += StartWorkAnimation;
            worker.OnStopWorking += StopWorkAnimation;
        }
    }

    public void HandleMovement(Vector3 moveDirection,float speedMultiplier)
    {
        RotateCharacter(moveDirection,false);
        animator.SetFloat("SpeedMultiplier", moveDirection.sqrMagnitude>0.001f ? speedMultiplier : 0f);
    }

    private void HandleHurt(DamageInfo damageInfo)
    {
        Flash();
    }

    private void HandleStunned()
    {
        animator.SetBool("IsStunned", true);
    }

    private void HandleStunRecorver()
    {
        animator.SetBool("IsStunned", false);
    }

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

    private void HandleDeath()
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
        if (direction.sqrMagnitude < 000.1f)
            return;
        direction.y = 0;
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

    public void Flash()
    {
        if (flashCoroutine != null)
            StopCoroutine(flashCoroutine);

        flashCoroutine = StartCoroutine(FlashRoutine());
    }

    private IEnumerator FlashRoutine()
    {
        foreach (var renderer in renderers)
        {
            renderer.GetPropertyBlock(propertyBlock);
            propertyBlock.SetColor(BaseColor, flashColor);
            renderer.SetPropertyBlock(propertyBlock);
        }

        yield return new WaitForSeconds(flashDuration);

        foreach (var renderer in renderers)
        {
            renderer.SetPropertyBlock(null);
        }

        flashCoroutine = null;
    }

}
