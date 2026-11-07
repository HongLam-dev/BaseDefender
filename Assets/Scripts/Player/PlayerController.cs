using Mono.Cecil;
using System;
using UnityEngine;
using UnityEngine.InputSystem;


[RequireComponent(typeof(CharacterCombat))]
public class PlayerController : MonoBehaviour
{
    [SerializeField] private SkillBase basicAttack;
    [SerializeField] private PlayerInput input;

    [SerializeField] private LayerMask targetLayer;

    [SerializeField] private PlayerResourceManager resourceManager;

    private CharacterCombat combat;
    private CharacterMovement movement;

    private SkillRuntime runtimeBasicAttack;
    private bool attackBtnPressed = false;
    private Vector2 currentMoveDirection = Vector2.zero;

    private InputAction[] skillActions;

    private Transform target;
    public event Action<Transform> OnTargetChange;
    public event Action<Vector3, ResourceType> OnStartHarvestingResource;
    public event Action<ResourceType> OnStopHarvestingResource;

    private ResourceNode targetResource;

    private bool isHarvesting;

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
        if(targetResource)
        {
            resourceManager.AddResource(targetResource.Harvest(Time.deltaTime,1),targetResource.ResourceType);
        }
        runtimeBasicAttack.Tick(Time.deltaTime);
        HandleInput();
    }

    private void HandleInput()
    {
        if (attackBtnPressed)
        {
            Transform target = FindTargetFromMousePosition();
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
            if (target&&!targetResource)
            {
                ResourceNode resource = target.GetComponent<ResourceNode>();
                if (resource)
                {
                    targetResource = resource;
                    OnStartHarvestingResource?.Invoke(resource.transform.position - transform.position,resource.ResourceType);
                    return;
                }
            }
        }
        else
        {
            if(currentMoveDirection!=Vector2.zero)
            {
                if (targetResource)
                {
                    OnStopHarvestingResource?.Invoke(targetResource.ResourceType);
                    targetResource = null;
                }
            }
            movement.SetDirection(new Vector3(currentMoveDirection.x, 0, currentMoveDirection.y));
        }
    }

    Transform FindTargetFromMousePosition()
    {
        Ray ray = Camera.main.ScreenPointToRay(
            Mouse.current.position.ReadValue()
        );

        if (Physics.Raycast(ray, out RaycastHit hit, 100f, targetLayer))
        {
            OnTargetChange?.Invoke(hit.transform);

            return hit.transform;
        }

        return null;
    }

    private void UnlockMovement()
    {
        movement.LockMovement(false);
    }

}
