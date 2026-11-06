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
        characterCombat = GetComponentInParent<CharacterCombat>();
        characterMovement = GetComponentInParent<CharacterMovement>();
        heath = GetComponentInParent<Health>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        if(heath)
        {
            heath.OnHurt += HandleHurt;
            heath.OnDie += HandleDie;
        }
        if(characterCombat)
        {
            characterMovement.OnDirectionChange += HandleMovement;
            characterCombat.BeginUsingSkill += HandleUsingSkill;
        }
    }
    private void HandleMovement(Vector2 moveDirection)
    {
        AnimationState state = AnimationState.Idle;

        if (moveDirection == Vector2.zero)
            moveDirection = currentDirection;
        else
        {
            RotateCharacter(moveDirection);
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
    private void RotateCharacter(Vector3 direction)
    {
        if (direction.sqrMagnitude < 0.001f)
            return;

        Quaternion targetRotation =
            Quaternion.LookRotation(new Vector3(direction.x,0,direction.y).normalized);

        transform.rotation = Quaternion.Slerp(
            transform.rotation,
            targetRotation,
            rotationSpeed * Time.deltaTime
        );
    }

    private void HandleStopMining()
    {
        characterEquipment.UnequipPickaxe();
        animator.SetBool("Mining", false);
    }

    private void HandleStopChopping()
    {
        characterEquipment.UnequipAxe();
        animator.SetBool("Chopping", false);
    }

    private void OnDestroy()
    {
        if (heath)
        {
            heath.OnHurt -= HandleHurt;
            heath.OnDie -= HandleDie;
        }
        if (characterCombat)
        {
            characterMovement.OnDirectionChange -= HandleMovement;
            characterCombat.BeginUsingSkill -= HandleUsingSkill;
        }
    }

}
