
using UnityEngine;
public class PlayerResourceManager : MonoBehaviour
{
    [Header("Resources")]
    [SerializeField] private int wood=0;
    [SerializeField] private int crystal=0;
    [SerializeField] private int stone=0;
    [SerializeField] private int soul=0;
    [Header("UI")]
    [SerializeField] private PlayerUIController ui;

    public int Wood => wood;
    public int Crystal => crystal;
    public int Stone => stone;
    public int Soul => soul;

    private void Start()
    {
        ui.UpdateWoodUI(wood);
        ui.UpdateStoneUI(stone);
        ui.UpdateCrystalUI(crystal);
        ui.UpdateSoulUI(soul);
    }

    public void AddResource(int amount, ResourceType type)
    {
        if (amount <= 0) return;

        switch (type)
        {
            case ResourceType.Wood:
                ui.UpdateWoodUI(wood);
                wood += amount;
                break;

            case ResourceType.Stone:
                stone += amount;
                ui.UpdateStoneUI(stone);
                break;
            case ResourceType.Crystal:
                crystal += amount;
                ui.UpdateCrystalUI(crystal);
                break;
            case ResourceType.Soul:
                soul += amount;
                ui.UpdateSoulUI(soul);
                break;
        }
    }
}
