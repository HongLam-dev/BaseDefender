using System;
using UnityEngine;

public class PlayerInteraction: MonoBehaviour,IWorker
{
    [SerializeField] PlayerResourceManager resourceManager;
    private PlayerMovement movement;

    float baseLumberingPower=1;
    float baseMiningPower = 1;
    float baseBuildingPower = 1;

    IWork nearbyWork;
    public event Action<WorkInfo> OnWork;
    public event Action<WorkInfo> OnStopWorking;

    public event Action<IWork> OnTargetWorkChange;
    public event Action<WorkInfo> OnNearByWorkProgressUpdate;

    bool isWorking = false;

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
            OnStopWorking?.Invoke(nearbyWork.GetWorkInfo());
        }
    }

    public void StartWorking()
    {
        if (nearbyWork==null)
            return;
        isWorking = true;
        OnWork?.Invoke(nearbyWork.GetWorkInfo());
    }

    public void GetResource(Reward reward)
    {
        resourceManager.AddResource(reward);
        OnNearByWorkProgressUpdate?.Invoke(nearbyWork.GetWorkInfo());
    }

    void OnTriggerStay(Collider collider)
    {
        IWork work = collider.GetComponent<IWork>();
        if (work!=null&&nearbyWork==null)
        {
            nearbyWork = work;
            ResourceNode resource= collider.GetComponent<ResourceNode>();
            OnTargetWorkChange?.Invoke(nearbyWork);
        }
    }

    void OnTriggerExit(Collider collider)
    {
        IWork work = collider.GetComponent<IWork>();
        if (nearbyWork!=null&&work==nearbyWork)
        {
            OnTargetWorkChange?.Invoke(null);
            if (isWorking)
            {
                isWorking = false;
                OnStopWorking?.Invoke(nearbyWork.GetWorkInfo());
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
