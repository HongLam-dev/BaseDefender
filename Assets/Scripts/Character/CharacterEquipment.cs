using System.Collections.Generic;
using UnityEngine;

public class CharacterEquipment : MonoBehaviour
{
    [SerializeField] private Transform handRight;
    [SerializeField] private Transform handLeft;
    [SerializeField] private Transform pickaxe;
    [SerializeField] private Transform axe;

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


    public void EquipPickaxe()
    {
        UnequipAllItems();
        EquipItem(pickaxe);
    }

    public void UnequipPickaxe()
    {
        UnequipItem(pickaxe);
        foreach(Transform item in characterDefaultItems)
        {
            EquipItem(item);
        }
    }

    public void EquipAxe()
    {
        UnequipAllItems();
        EquipItem(axe);
    }

    public void UnequipAxe()
    {
        UnequipItem(axe);
        foreach (Transform item in characterDefaultItems)
        {
            EquipItem(item);
        }
    }
}
