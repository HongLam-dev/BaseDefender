using System.Collections.Generic;
using UnityEngine;

public class CharacterEquipment : MonoBehaviour
{
    [SerializeField] private Transform handRight;
    [SerializeField] private Transform handLeft;
    [SerializeField] private Transform pickaxe;
    [SerializeField] private Transform axe;
    [SerializeField] private Transform hammer;

    [SerializeField] private List<Transform> holdingItems;
    [SerializeField] private Transform[] characterDefaultItems;


    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void EquipItem(Transform itemToEquip)
    {
        holdingItems.Add(itemToEquip);
        itemToEquip.gameObject.SetActive(true);
    }

    private void UnequipItem(Transform item)
    {
        if(holdingItems.Contains(item))
        {
            item.gameObject.SetActive(false);
            holdingItems.Remove(item);
        }
    }

    private void UnequipAllItems()
    {
        foreach(Transform item in holdingItems)
        {
            item.gameObject.SetActive(false);
        }
        holdingItems.Clear();
    }

    public void EquipWorkEquipment(WorkType workType)
    {
        UnequipAllItems();
        switch (workType)
        {
            case WorkType.Lumbering:
                EquipItem(axe);
                break;
            case WorkType.Mining:
                EquipItem(pickaxe);
                break;
            case WorkType.Building:
                EquipItem(hammer);
                break;
        }
    }

    public void UnequipWorkEquipment(WorkType workType)
    {
        switch (workType)
        {
            case WorkType.Lumbering:
                UnequipItem(axe);
                break;
            case WorkType.Mining:
                UnequipItem(pickaxe);
                break;
            case WorkType.Building:
                UnequipItem(hammer);
                break;
        }
        foreach (Transform item in characterDefaultItems)
        {
            EquipItem(item);
        }
    }
}
