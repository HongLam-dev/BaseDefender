using UnityEngine;

[CreateAssetMenu(
    fileName = "New Item",
    menuName = "Game/Items/Item Data",
    order = 1
)]
public class ItemData : ScriptableObject
{
    public string id;
    public string itemName;
    public Sprite icon;
    public string description;
}