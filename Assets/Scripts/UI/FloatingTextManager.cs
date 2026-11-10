using System;
using System.Collections.Generic;
using UnityEngine;

public class FloatingTextManager : MonoBehaviour
{
    [SerializeField] Transform container;
    [SerializeField] int maxPoolSize=20;
    [SerializeField] GameObject floatingTextPrefab;

    [SerializeField] Transform player;
    [SerializeField] PlayerResourceManager resourceManager;

    [SerializeField] Vector3 floatingTextStartPositionOffset = new Vector3(0,0,1);

    int currentIndex=0;
    List<FloatingText> pool = new();

    private void Start()
    {
        resourceManager.OnResourceAdded += HandleResourceAdded;
    }

    private void HandleResourceAdded(ItemStack stack)
    {
        GetText().Initialize(player.transform.position+floatingTextStartPositionOffset,stack.amount,stack.item.icon);
    }

    FloatingText GetText()
    {
        if (pool.Count < maxPoolSize)
        {
            GameObject obj = Instantiate(floatingTextPrefab, container);
            FloatingText text = obj.GetComponent<FloatingText>();
            pool.Add(text);
        }

        if (currentIndex >= pool.Count)
            currentIndex = 0;

        return pool[currentIndex++];
    }

}
