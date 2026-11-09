using System;
using System.Collections;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.EventSystems;

public class CharacterAnimationController : MonoBehaviour
{
    [SerializeField] private Animator animator;

    [SerializeField] private float hitEffectTime = 0.2f;

    [SerializeField] private Color hitEffectColor = Color.red;

    [SerializeField] private float rotationSpeed = 5.0f;
    private CharacterCombat characterCombat;
    private CharacterMovement characterMovement;
    private CharacterEquipment characterEquipment;

    private Health heath;
    private SpriteRenderer spriteRenderer;

    private Vector2 currentDirection = Vector2.down;
    public event Action SkillAnimationFinishCallback;
    public event Action<int> SkillAnimationEvent;

    void Start()
    {
        animator=GetComponent<Animator>();
        characterCombat = GetComponent<CharacterCombat>();
        characterMovement = GetComponent<CharacterMovement>();
        characterEquipment = GetComponent<CharacterEquipment>();

        heath = GetComponentInParent<Health>();
        spriteRenderer = GetComponent<SpriteRenderer>();

     //   GetComponent<PlayerMovement>().OnStartHarvestingResource += StartResourceHarvestingAnimation;
     //   GetComponent<PlayerMovement>().OnStopHarvestingResource += StopResourceHarvestingAnimation;

        heath.OnHurt += HandleHurt;
        heath.OnDie += HandleDie;
        characterMovement.OnDirectionChange += HandleMovement;
        characterCombat.BeginUsingSkill += HandleUsingSkill;
    }



    private void HandleMovement(Vector3 moveDirection)
    {
        AnimationState state = AnimationState.Idle;

        if (moveDirection == Vector3.zero)
            moveDirection = currentDirection;
        else
        {
            RotateCharacter(moveDirection,false);
            state = AnimationState.Run;
        }
        SetAnimation(state, moveDirection);
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

    private void HandleUsingSkill(string animation, Vector2 strikeDirection)
    {
        if(!HasTrigger(animation))
        {
            OnSkillAnimationFinish();
            Debug.Log("No trigger");
            return;
        }
        Vector2 direction;
        if (math.abs(strikeDirection).x < math.abs(strikeDirection.y))
        {
            if (strikeDirection.y > 0)
                direction = new Vector2(0, 1);
            else
                direction = new Vector2(0, -1);
        }
        else
        {
            if (strikeDirection.x > 0)
                direction = new Vector2(1, 0);
            else
                direction = new Vector2(-1, 0);
        }
        SetSkillAnimation(animation, direction);
    }

    private void OnSkillAnimationEvent(int stage)
    {
        SkillAnimationEvent?.Invoke(stage);
    }

    private void HandleHurt(DamageInfo damageInfo)
    {
        //SetAnimation(AnimationState.Hurt);
        StartCoroutine(HurtEffect());
    }

    private IEnumerator HurtEffect()
    {
        spriteRenderer.color = hitEffectColor;
        yield return new WaitForSeconds(hitEffectTime);
        spriteRenderer.color = Color.white;
    }

    private void StartResourceHarvestingAnimation(Vector3 direction,ResourceType type)
    {
        RotateCharacter(direction,true);

        switch(type)
        {
            case ResourceType.Wood:
                characterEquipment.EquipAxe();
                SetAnimation(AnimationState.Chopping);
                break;
            default:
                characterEquipment.EquipPickaxe();
                SetAnimation(AnimationState.Mining);
                break;
        }

    }
    private void StopResourceHarvestingAnimation( ResourceType type)
    {
        switch (type)
        {
            case ResourceType.Wood:
                animator.SetBool("Chopping",false);
                characterEquipment.UnequipAxe();
                break;
            default:
                animator.SetBool("Mining", false);
                characterEquipment.UnequipPickaxe();
                break;
        }

    }

    private void HandleDie()
    {
        SetAnimation(AnimationState.Dead);
    }

    private void OnSkillAnimationFinish()
    {
        SkillAnimationFinishCallback?.Invoke();
    }
    private void SetAnimation(AnimationState state)
    {
        SetAnimation(state, currentDirection);
    }

    public void DieAnimationFinish()
    {
        transform.parent.gameObject.SetActive(false);
    }


    private void SetSkillAnimation(string animation,Vector2 direction)
    {
        animator.SetTrigger(animation);
    }

    private void SetAnimation(AnimationState state, Vector2 direction)
    {
        if (state== AnimationState.Run)
        {             
            animator.SetBool("IsRunning", true);
        }
        else
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

            animator.SetBool("IsRunning", false);
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
