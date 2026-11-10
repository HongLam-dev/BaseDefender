
using System.Collections.Generic;
using System;
using UnityEngine;
public class PlayerResourceManager : MonoBehaviour
{
    List<ItemStack> itemStacks=new();
    public event Action<ItemStack> OnResourceAdded;

    private void Start()
    {
       
    }

    public void AddResource(Reward reward)
    {
        foreach(ItemStack itemStack in reward.reward)
        {
            AddItems(itemStack);
        }
    }

    public void AddItems(ItemStack stack)
    {
        if (stack.amount == 0)
            return;
        for(int i=0;i<itemStacks.Count;i++)
        {
            if (itemStacks[i].item==stack.item)
            {
                itemStacks[i].amount+= stack.amount;
                OnResourceAdded?.Invoke(stack);
                return;
            }
        }
        ItemStack newStack = new();
        newStack.item = stack.item;
        newStack.amount = stack.amount;
        itemStacks.Add(newStack);
        OnResourceAdded?.Invoke(stack);
    }

    public int GetResourceAmount(string id)
    {
        foreach(ItemStack stack in itemStacks)
        {
            if(stack.item.id==id)
            {
                return stack.amount;
            }
        }
        return 0;
    }
}
