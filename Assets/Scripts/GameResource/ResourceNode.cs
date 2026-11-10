using System;
using Unity.VisualScripting;
using UnityEngine;

public class ResourceNode : MonoBehaviour, IWork
{
    [SerializeField] private ItemData resourceItem;
    [SerializeField] private int remainingResource = 100;
    [SerializeField] private int resourcePerHarvest = 10;
    [SerializeField] private float baseHarvestTime = 2.0f;
    private float harvestTimer=0;
    private Reward harvestReward;
    [SerializeField] private WorkType workType;
    public ItemData ResourceItem => resourceItem;

    public void Start()
    {
        ItemStack itemStack;
        itemStack.amount = resourcePerHarvest;
        itemStack.item = resourceItem;
        harvestReward.reward = new();
        harvestReward.reward.Add(itemStack);
    }

    public void Work(float deltaTime, IWorker worker)
    {
        harvestTimer += deltaTime;
        int amount = 0;

        if (harvestTimer>=baseHarvestTime/worker.GetWorkPower(workType))
        {
            amount = Mathf.Min(
              resourcePerHarvest,
              remainingResource
            );
            remainingResource -= amount;
            harvestTimer = 0;
            RewardWorker(worker);
        }
        if (remainingResource <= 0)
        {
            Destroy(gameObject);
        }     
    }

    private void RewardWorker(IWorker worker)
    {
        worker.EarnReward(harvestReward);
    }

   public WorkInfo GetWorkInfo()
    {
        return new WorkInfo(remainingResource, workType, transform.position,resourceItem.icon);
    }
}
