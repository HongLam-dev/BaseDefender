using System;
using UnityEngine;
using UnityEngine.InputSystem;


[RequireComponent(typeof(CharacterCombat))]
public class PlayerController : MonoBehaviour
{
    [SerializeField] private SkillBase basicAttack;
    [SerializeField] private PlayerInput input;

    [SerializeField] private LayerMask targetLayer;

    private CharacterCombat combat;
    private CharacterMovement movement;

    private SkillRuntime runtimeBasicAttack;
    private bool attackBtnPressed = false;
    private Vector2 currentMoveDirection = Vector2.zero;

    private InputAction[] skillActions;

    private Transform target;
    public event Action<Transform> OnTargetChange;

    protected void Awake()
    {
        input = GetComponent<PlayerInput>();
        combat = GetComponent<CharacterCombat>();
        movement = GetComponent<CharacterMovement>();
        GetComponent<CharacterAnimationController>().SkillAnimationFinishCallback += UnlockMovement;

        skillActions = new[]
    {
        input.actions.FindAction("Player/Skill1"),
        input.actions.FindAction("Player/Skill2")
    };
        runtimeBasicAttack = new SkillRuntime(basicAttack);

        for (int i = 0; i < skillActions.Length; i++)
        {
            int index = i;
            skillActions[index].performed += ctx => UseSkill(index);
        }
    }

    void UseSkill(int index)
    {
        Vector2 mousePosition =
    Camera.main.ScreenToWorldPoint(
   Mouse.current.position.ReadValue()
     );
        if (combat.TryUseSkill(index, mousePosition, target))
        {
            movement.LockMovement(true);
        }
    }


    public void OnAttack(InputAction.CallbackContext context)
    {
        if (context.performed)
            attackBtnPressed = true;
        else if (context.canceled)
            attackBtnPressed = false;
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        currentMoveDirection = context.ReadValue<Vector2>();
    }

    protected void Update()
    {
        FindTargetFromMousePosition();
        runtimeBasicAttack.Tick(Time.deltaTime);
        HandleInput();
    }

    private void HandleInput()
    {
        if (attackBtnPressed)
        {
            if (basicAttack)
            {
                Vector2 mousePosition =
            Camera.main.ScreenToWorldPoint(
             Mouse.current.position.ReadValue()
            );
                if (combat.TryUseSkill(runtimeBasicAttack, mousePosition, target))
                {
                    movement.LockMovement(true);
                }
            }
        }
        else
        {
            movement.SetDirection(currentMoveDirection);
        }
    }

    void FindTargetFromMousePosition()
    {
        Vector2 mousePosition =
              Camera.main.ScreenToWorldPoint(
                  Mouse.current.position.ReadValue()
              );

        Collider2D hit = Physics2D.OverlapPoint(
            mousePosition, targetLayer
        );
        if (hit)
        {
            if (hit.GetComponent<CharacterCombat>())
            {
                target = hit.transform;
                OnTargetChange?.Invoke(target);
                return;
            }
        }
    }

    private void UnlockMovement()
    {
        movement.LockMovement(false);
    }
}
