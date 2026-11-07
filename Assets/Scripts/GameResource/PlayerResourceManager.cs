
using UnityEngine;
public class PlayerResourceManager : MonoBehaviour
{
    [Header("Resources")]
    [SerializeField] private int wood;
    [SerializeField] private int crystal;
    [SerializeField] private int stone;

    public int Wood => wood;
    public int Crystal => crystal;
    public int Stone => stone;

    public void AddResource(int amount, ResourceType type)
    {
        if (amount <= 0) return;

        switch (type)
        {
            case ResourceType.Wood:
                wood += amount;
                break;

            case ResourceType.Crystal:
                crystal += amount;
                break;

            case ResourceType.Stone:
                stone += amount;
                break;
        }

        Debug.Log($"Added {amount} {type}");
    }
}
