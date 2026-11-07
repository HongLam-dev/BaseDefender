using Mono.Cecil;
using System;
using UnityEngine;
using UnityEngine.InputSystem;


[RequireComponent(typeof(CharacterCombat))]
public class PlayerController : MonoBehaviour
{
    [SerializeField] private PlayerInput input;

    [SerializeField] private LayerMask targetLayer;

    [SerializeField] private PlayerResourceManager resourceManager;
    [SerializeField] private float interactDistance=1;

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

    bool isHarvesting;
    private PlayerState currentState = PlayerState.Idle;

    protected void Awake()
    {
        input = GetComponent<PlayerInput>();
        combat = GetComponent<CharacterCombat>();
        movement = GetComponent<CharacterMovement>();
        GetComponent<CharacterAnimationController>().SkillAnimationFinishCallback += StopCastingSkill;

        skillActions = new[]
    {
        input.actions.FindAction("Player/Skill1"),
        input.actions.FindAction("Player/Skill2")
    };

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
            SetState(PlayerState.CastingSkill);
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

        if(currentState==PlayerState.HarvestingResource)
        {
            if(isHarvesting)
                resourceManager.AddResource(targetResource.Harvest(Time.deltaTime, 1), targetResource.ResourceType);
            else
            {
                Vector3 closestPoint = targetResource.GetComponent<Collider>().ClosestPoint(transform.position);

                float distance = Vector3.Distance(
                    transform.position,
                    closestPoint
                );

                if (distance <= interactDistance)
                {
                    movement.StopAgent();
                    OnStartHarvestingResource?.Invoke(targetResource.transform.position - transform.position, targetResource.ResourceType);
                    isHarvesting = true;
                }
            }     
        }
        HandleInput();
    }

    private void HandleInput()
    {
        if (attackBtnPressed)
        {
            Transform target = FindTargetFromMousePosition();

            if (target)
            {
                ResourceNode resource = target.GetComponent<ResourceNode>();
                if (resource!=targetResource)
                {
                    if(isHarvesting)
                        OnStopHarvestingResource?.Invoke(targetResource.ResourceType);
                    targetResource = resource;
                    SetState(PlayerState.HarvestingResource);
                    return;
                }
                else
                {
                    Vector2 mousePosition =
                    Camera.main.ScreenToWorldPoint(
                    Mouse.current.position.ReadValue()
                    );
                    if (combat.TryUseSkill(0, mousePosition, target))
                    {
                        SetState(PlayerState.CastingSkill);
                    }
                }
            }
        }
        else
        {
            if(currentMoveDirection!=Vector2.zero)
            {
                SetState(PlayerState.Running);
            }
            else if(currentState==PlayerState.Running)
            {
                SetState(PlayerState.Idle);
            }      
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

    private void StopCastingSkill()
    {
        SetState(PlayerState.Idle);
    }

    private void SetState(PlayerState newState)
    {
        DisableCurrentState(newState);
        switch (newState)
        {
            case PlayerState.Idle:
                movement.SetDirection(Vector3.zero);
                break;
            case PlayerState.CastingSkill:
                movement.LockMovement(true);
                break;
            case PlayerState.Running:
                movement.SetDirection(new Vector3(currentMoveDirection.x, 0, currentMoveDirection.y));
                break;
            case PlayerState.HarvestingResource:
                movement.MoveToDestination(targetResource.transform.position);           
                break;
        }
        currentState = newState;
    }
    private void DisableCurrentState(PlayerState newState)
    {
        switch (currentState)
        {
            case PlayerState.Idle:
                break;
            case PlayerState.CastingSkill:
                movement.LockMovement(false);
                break;
            case PlayerState.Running:
                break;
            case PlayerState.HarvestingResource:           
                isHarvesting = false;
                if(newState!=PlayerState.HarvestingResource)
                {
                    OnStopHarvestingResource?.Invoke(targetResource.ResourceType);
                    targetResource = null;
                }
                break;
        }
    }
}
