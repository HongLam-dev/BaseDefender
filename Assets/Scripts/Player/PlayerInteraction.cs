using System;
using UnityEngine;

public class PlayerInteraction: MonoBehaviour,IWorker
{
    [SerializeField] PlayerResourceManager resourceManager;
    [SerializeField] PlayerUIController ui;

    private PlayerMovement movement;

    float baseLumberingPower=1;
    float baseMiningPower = 1;
    float baseBuildingPower = 1;

    IWork nearbyWork;

    bool isWorking = false;

    public event Action<Vector3,WorkType> OnWork;
    public event Action<WorkType> OnStopWorking;

    private void Start()
    {
        movement = GetComponent<PlayerMovement>();
        movement.OnMovement += HandlePlayerMove;
    }

    public float GetWorkPower(WorkType workType)
    {
        switch(workType)
        {
            case WorkType.Lumbering:
                return baseLumberingPower;
            case WorkType.Mining:
                return baseMiningPower;
            case WorkType.Building:
                return baseBuildingPower;
            default:
                return 0;
        }
    }

    public void HandlePlayerMove(Vector3 direction)
    {
        if(isWorking)
        {
            isWorking = false;
            OnStopWorking?.Invoke(nearbyWork.GetWorkInfo().type);
        }
    }

    public void StartWorking()
    {
        if (nearbyWork==null)
            return;
        isWorking = true;
        OnWork?.Invoke(nearbyWork.GetWorkInfo().position-transform.position,nearbyWork.GetWorkInfo().type);
    }

    public void EarnReward(Reward reward)
    {
        resourceManager.AddResource(reward);
        ui.UpdateWorkloadUI(nearbyWork.GetWorkInfo().remainingWorkload);
    }

    void OnTriggerEnter(Collider collider)
    {
        IWork work = collider.GetComponent<IWork>();
        if (work!=null&&nearbyWork==null)
        {
            nearbyWork = work;
            ResourceNode resource= collider.GetComponent<ResourceNode>();
            ui.DisplayWorkDetail(nearbyWork.GetWorkInfo().remainingWorkload, nearbyWork.GetWorkInfo().icon);
        }
    }

    void OnTriggerExit(Collider collider)
    {
        IWork work = collider.GetComponent<IWork>();
        if (nearbyWork!=null&&work==nearbyWork)
        {
            ui.DisableWorkDetail();       
            if(isWorking)
            {
                isWorking = false;
                OnStopWorking?.Invoke(nearbyWork.GetWorkInfo().type);
            }
            nearbyWork = null;
        }
    }

    private void Update()
    {
        if(isWorking&&nearbyWork!=null)
        {
            nearbyWork.Work(Time.deltaTime, this);
        }
    }
}
