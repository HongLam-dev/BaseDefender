using UnityEngine;

[CreateAssetMenu(fileName = "NewConstructionData",
    menuName = "Game/ConstructionData"
)]
public class ConstructionData:ScriptableObject
{
    public string constructionName="No Name Building";
    public Sprite icon;
    public int workLoadToBuild = 100;

    public GameObject prefab;
   
}