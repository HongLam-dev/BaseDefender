using System;
using UnityEngine;
using static CharacterCombat;

public class PlayerInteraction: MonoBehaviour,IWorker
{
    [SerializeField] PlayerResourceManager resourceManager;

    float baseLumberingPower=1;
    float baseMiningPower = 1;
    float baseBuildingPower = 1;

    IWork nearbyWork;
    public event Action<WorkInfo> OnWork;
    public event Action<WorkInfo> OnStopWorking;

    public event Action<IWork> OnTargetWorkChange;
    public event Action<WorkInfo> OnNearByWorkProgressUpdate;

    bool isWorking = false;

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
                StopWorking();
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

    public void StopWorking()
    {
        if (!isWorking)
            return;
        isWorking = false;
        OnStopWorking?.Invoke(nearbyWork.GetWorkInfo());
    }
}
