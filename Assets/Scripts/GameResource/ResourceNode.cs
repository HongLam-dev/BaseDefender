using System;
using Unity.VisualScripting;
using UnityEngine;

public class ResourceNode : MonoBehaviour
{
    [SerializeField] private ResourceType type;
    public ResourceType ResourceType => type;
    [SerializeField] private int remainingResource = 100;
    [SerializeField] private int resourcePerHarvest = 10;
    [SerializeField] private float baseHarvestTime = 2.0f;
    private float harvestTimer=0;

    public int Harvest(float deltaTime, float harvestPower)
    {
        harvestTimer += deltaTime;
        int amount = 0;

        if (harvestTimer>=baseHarvestTime/harvestPower)
        {
            amount = Mathf.Min(
              resourcePerHarvest,
              remainingResource
            );
            harvestTimer = 0;
        }
        remainingResource -= amount;
        if (remainingResource <= 0)
        {
            Destroy(gameObject);
        }
        return amount;
    }
}
