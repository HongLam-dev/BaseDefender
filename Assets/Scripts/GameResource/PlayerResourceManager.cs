
using UnityEngine;
public class PlayerResourceManager : MonoBehaviour
{
    private int wood=0;
    private int crystal=0;
    private int stone=0;
    private int soul=0;
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

    public void AddResource(Reward reward)
    {
        foreach(ItemStack itemStack in reward.reward)
        {
            if(itemStack.item.id=="resource_wood")
            {
                wood += itemStack.amount;
                ui.UpdateWoodUI(wood);
            }
            else if (itemStack.item.id == "resource_stone")
            {
                stone += itemStack.amount;
                ui.UpdateStoneUI(stone);
            }
            else if (itemStack.item.id == "resource_crystal")
            {
                crystal += itemStack.amount;
                ui.UpdateCrystalUI(crystal);
            }
            else if (itemStack.item.id == "resource_soul")
            {
               soul += itemStack.amount;
                ui.UpdateSoulUI(soul);
            }
        }
    }
}
