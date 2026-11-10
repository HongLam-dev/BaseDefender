using System;
using UnityEngine;

public interface IWorker
{
    public float GetWorkPower(WorkType workType);
    public void EarnReward(Reward reward);
}

