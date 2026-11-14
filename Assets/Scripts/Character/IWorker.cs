using System;
using UnityEngine;

public interface IWorker
{
    public event Action<WorkInfo> OnWork;
    public event Action<WorkInfo> OnStopWorking;
    public float GetWorkPower(WorkType workType);
    public void GetResource(Reward reward);
    public void StopWorking();
}

