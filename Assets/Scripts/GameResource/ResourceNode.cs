using System;
using Unity.VisualScripting;
using UnityEngine;

public class ResourceNode : MonoBehaviour, IWork
{
    [SerializeField] private ItemData resourceItem;
    [SerializeField] private int startAmount=100;
    private int remainingResource;
    [SerializeField] private int resourcePerHarvest = 10;
    [SerializeField] private float baseHarvestTime = 2.0f;
    private float harvestTimer=0;
    private Reward harvestReward;
    [SerializeField] private WorkType workType;
    public ItemData ResourceItem => resourceItem;

    public event Action<int> OnWorkProgress;
    public void Start()
    {
        remainingResource = startAmount;
        ItemStack itemStack=new();
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
            OnWorkProgress?.Invoke(remainingResource);
        }
        if (remainingResource <= 0)
        {
            Destroy(gameObject);
        }     
    }

    private void RewardWorker(IWorker worker)
    {
        worker.GetResource(harvestReward);
    }

   public WorkInfo GetWorkInfo()
    {
        return new WorkInfo(startAmount,remainingResource, workType, transform.position,resourceItem.icon);
    }

    public bool IsDone()
    {
        return remainingResource <= 0;
    }
}
